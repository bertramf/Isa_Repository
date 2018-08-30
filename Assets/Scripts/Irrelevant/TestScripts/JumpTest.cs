using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour {

    private Rigidbody2D rb;

    public bool method1 = true;
    public float jumpStartVelocity = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float showVelocityY;

    private void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update () {
        if (Input.GetButtonDown("A")){
            rb.velocity = Vector2.up * jumpStartVelocity;
        }
        if (method1){ 
            if (Input.GetButtonUp("A")){
                if (rb.velocity.y > 0){ 
                    float yVel = rb.velocity.y * 0.35f;
                    rb.velocity = new Vector2(0, yVel);
                }
            }
        }


	}

    private void FixedUpdate(){
        if (method1 == false)
        {
            //fall behaviour
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            //up behaviour als je hem wel indrukt

            //up behaviour && je hem niet indrukt
            else if (rb.velocity.y > 0 && !Input.GetButton("A"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
            showVelocityY = rb.velocity.y;
        
    }
}
