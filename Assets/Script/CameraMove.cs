using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Transform PlayerPoision;
    Vector3 Offset;
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerPoision = GameObject.FindGameObjectWithTag("Player").transform;
        Offset = transform.position - PlayerPoision.position;        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = PlayerPoision.position + Offset;
        if (PlayerPoision.position.y < -5)
        {
            transform.position = new Vector2 (PlayerPoision.position.x,0);
        }
    }
}
