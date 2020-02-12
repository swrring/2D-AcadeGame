using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float MaxSpeed;
    public float JumpPower;
    
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
    BoxCollider2D gamemanager;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void FixedUpdate()
    {
        //캐릭터 좌/우 이동 설정
        float h = Input.GetAxisRaw("Horizontal");
        
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
      
       
        if(rigid.velocity.x > MaxSpeed)
        {
            rigid.velocity = new Vector2(MaxSpeed, rigid.velocity.y);
           
        }
        if(rigid.velocity.x < MaxSpeed*(-1))
        {
            rigid.velocity = new Vector2(MaxSpeed*-1, rigid.velocity.y);
        }
        if(rigid.velocity.y > JumpPower)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, JumpPower);
            
        }

        //Ray cast 사용 예시
        //Debug.DrawRay(rigid.position, Vector3.down,new Color(0, 1, 0));
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1,LayerMask.GetMask("Tile"));
        if (rigid.velocity.y < 0)
        {
            if (rayhit.collider != null)
            {
                if (rayhit.distance < 0.5f)
                {
                    anim.SetBool("isJumping", false);
                }
                //Debug.Log(rayhit.collider.name);
            }
        }
        
        
    }
    private void Update()
    {
        //점프 
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping"))
        {
            if (Mathf.Abs(rigid.velocity.y) < 0.1f)
            {
                rigid.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }
        }





        //버튼 땔 때 정지 가속도 감소 추가
        if (Input.GetButtonUp("Horizontal"))
        {

            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);          
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;           
        }       


        if(Mathf.Abs(rigid.velocity.x) <= 0.4)
        {
            anim.SetBool("isWalking", false);
        }
        else
        {
            anim.SetBool("isWalking", true);
        }
        
    }
    
}
