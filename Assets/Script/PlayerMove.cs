using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float MaxSpeed;
    public float MaxHight;
    
    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator anim;
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
        if(rigid.velocity.y > MaxHight)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, MaxHight);
            
        }
        
    }
    private void Update()
    {
        //버튼 땔 때 정지 가속도 감소 추가
        if(Input.GetButtonUp("Horizontal"))
        {

            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;           
        }
        if (Input.GetButtonDown("Jump"))
        {
            if (Mathf.Abs(rigid.velocity.y) < 0.1f)
            {
               rigid.AddForce(Vector2.up * MaxHight, ForceMode2D.Impulse);
            }
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
