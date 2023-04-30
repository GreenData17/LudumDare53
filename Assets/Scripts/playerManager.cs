using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playerManager : MonoBehaviour
{
    public static playerManager instance;

    private Rigidbody2D body;
    public bool canMove = true;
    public float Speed = 5.0f;

    public SpriteRenderer sprite;



    void Start()
    {
        if(instance == null) 
            instance = this;
        else
            Destroy(this);

        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        Movement();
        TurnBoat();
    }

    void Movement(){
        if(!canMove) 
        {
            body.velocity = new Vector2(0, 0);
            return;
        }

        if(Input.GetAxis("Horizontal") >= .1f || Input.GetAxis("Horizontal") <= -.1f){
            body.velocity = new Vector2((Input.GetAxis("Horizontal") * Speed)*Time.deltaTime, 0);

            if(Input.GetAxis("Horizontal") <= .1f)
                sprite.gameObject.transform.rotation = new Quaternion(0,180,0,0);
            else if(Input.GetAxis("Horizontal") >= -.1f)
                sprite.gameObject.transform.rotation = new Quaternion(0,0,0,0);
        }else{
            body.velocity = new Vector2(0, 0);
        }

        if(Input.GetAxis("Vertical") >= .1f || Input.GetAxis("Vertical") <= -.1f){
            body.velocity = new Vector2(body.velocity.x, (Input.GetAxis("Vertical") * Speed)*Time.deltaTime);
        }
        else{
            body.velocity = new Vector2(body.velocity.x, 0);
        }

        body.velocity.Normalize();
    }

    void TurnBoat(){
        if(Input.GetKeyDown(KeyCode.Space))
            Speed *= 4;
        if(Input.GetKeyUp(KeyCode.Space))
            Speed /= 4;
    }
}
