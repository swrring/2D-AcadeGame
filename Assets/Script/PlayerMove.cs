using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager maneger;
    public float MaxSpeed;
    public float JumpPower;
    public AudioClip attacksound;
    public AudioClip jumpsound;
    public AudioClip itemsound;
    public AudioClip diesound;
    public AudioClip hitsound;
    public AudioClip finishsound;



    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    CapsuleCollider2D playercollider;
    AudioSource audios;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        playercollider = GetComponent<CapsuleCollider2D>();
        audios = GetComponent<AudioSource>();

    }
    void FixedUpdate()
    {
        //vect.x = Input.GetAxisRaw("Horizontal");
        //rigid.velocity = vect * MaxSpeed;    
    }
    void Update()
    {
        //캐릭터 좌&우 이동 설정
        float h = Input.GetAxisRaw("Horizontal");

        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //최대 좌&우 이동속도 제한
        if (rigid.velocity.x > MaxSpeed)
        {
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);

        }
        if (rigid.velocity.x < MaxSpeed * (-1))
        {
            rigid.velocity = new Vector2(MaxSpeed * -1, rigid.velocity.y);
        }
        //이동 버튼을 땔 때 정지 가속도 감소 추가
        if (Input.GetButtonUp("Horizontal"))
        {

            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.3f, rigid.velocity.y);
        }
        // 좌&우 방향키 입력에 따라 캐릭터 모션 반전
        if (Input.GetButton("Horizontal"))
        {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        //캐릭터 X축 가속도를 기준으로 걷기 동작과 대기 동작을 실행
        if (Mathf.Abs(rigid.velocity.x) <= 0.4)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }


        //캐릭터 점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            //공중에 떠있지 않은 상태일 경우 위쪽으로 점프 파워만큼 이동시키고, 점프 상태로 전환
            if (Mathf.Abs(rigid.velocity.y) < 0.1f)
            {
                rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
                PlaySound("jump");
            }
        }

        //최대 점프 높이 제한
        if (rigid.velocity.y > JumpPower)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, JumpPower);
        }

        //Ray cast 사용 예시 및 디버그
        //Debug.DrawRay(rigid.position, Vector3.down,new Color(0, 1, 0));

        //캐릭터 아래방향으로 레이를 쏘아 어떤 레이어와 닿았는지를 체크
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Tile"));

        //캐릭터의 Y축이 0보다 작을 경우는 점프로 간주
        if (rigid.velocity.y < 0)
        {
            //캐릭터 아래에 충돌한 레이어가 있을 경우
            if (rayhit.collider != null)
            {
                //레이의 길이를 고려해 캐릭터가 지면과 닿았을 떄 점프 상태를 해제
                if (rayhit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
                //Debug.Log(rayhit.collider.name);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy")
        {
            //점프 공격(밟기)
            if (rigid.velocity.y < 0 && transform.position.y > other.transform.position.y)
            {
                OnAttack(other.transform);
            }
            else
            {
                OnDamaged(other.transform.position);
            }
            PlaySound("attack");
        }
        else if (other.collider.tag == "Spike")
        {
            OnDamaged(other.transform.position);
        }



    }
    void OnDamaged(Vector2 posion)
    {
        maneger.HPdown();
        gameObject.layer = 11;
        sprite.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - posion.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7, ForceMode2D.Impulse);
        PlaySound("hit");
        anim.SetTrigger("Hit");
        Invoke("OffDamaged", 2);

    }
    void OffDamaged()
    {
        gameObject.layer = 10;
        sprite.color = new Color(1, 1, 1, 1);
    }
    void OnAttack(Transform enemy)
    {
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        rigid.AddForce(Vector2.up * 13, ForceMode2D.Impulse);
        maneger.score += 30;
        enemyMove.OnDamaged();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
            bool isBoronz = collision.gameObject.name.Contains("Bronz");
            bool isSilver = collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if (isBoronz)
                maneger.score += 10;
            else if (isSilver)
                maneger.score += 50;
            else if (isGold)
                maneger.score += 300;
            PlaySound("item");
        }
        else if (collision.gameObject.tag == "Finish")
        {
            collision.gameObject.SetActive(false);
            PlaySound("finish");
            maneger.NextStage();
        }
    }


    public void OnDie()
    {
        //스프라이트 알파값 적용   
        sprite.color = new Color(1, 1, 1, 0.3f);

        //스프라이트 반전
        sprite.flipY = true;
        playercollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        PlaySound("die");
    }
    public void PlayerPosionZero()
    {
        rigid.velocity = Vector2.zero;
    }

    void PlaySound(string type)
    {
        switch (type)
        {
            case "jump":
                audios.clip = jumpsound;
                break;
            case "attack":
                audios.clip = attacksound;
                break;
            case "hit":
                audios.clip = hitsound;
                break;
            case "die":
                audios.clip = diesound;
                break;
            case "item":
                audios.clip = itemsound;
                break;
            case "finish":
                audios.clip = finishsound;
                break;
        }
        audios.Play();
    }
}
