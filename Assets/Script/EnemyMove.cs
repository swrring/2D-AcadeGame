using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer sprit;
    BoxCollider2D enemycollider;

    public int nextmove;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprit = GetComponent<SpriteRenderer>();
        enemycollider = GetComponent<BoxCollider2D>();
        Invoke("AI",4);
    }
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextmove*2, rigid.velocity.y);
        Vector2 vect = new Vector2(rigid.position.x+nextmove*0.2f, rigid.position.y);
        //Debug.DrawRay(vect, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D ray = Physics2D.Raycast(vect, Vector3.down, 1,LayerMask.GetMask("Tile"));
        if(ray.collider == null)
        {
            Turn();           
        }
    }
    void AI()
    {
        nextmove = Random.Range(-1, 2);
        float randomTime = Random.Range(2f, 5f);        
       

        anim.SetInteger("WalkSpeed", nextmove);
        if (nextmove != 0)
        {
            sprit.flipX = nextmove == 1;
        }

        Invoke("AI", randomTime);

    }
    void Turn()
    {
        CancelInvoke();
        nextmove *= -1;
        sprit.flipX = nextmove == 1;        
        Invoke("AI", 4);
    }
    public void OnDamaged()
    {
        //스프라이트 알파값 적용   
        sprit.color = new Color(1, 1, 1, 0.3f);
        nextmove = 0;
        //스프라이트 반전
        sprit.flipY = true;
        enemycollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("OnDestroy", 3);
    }
    void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
