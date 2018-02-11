using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Designed for basic movement: idle & walking
public class PlayerMovement : MonoBehaviour {

    private Rigidbody2D rb;
    private PlayerCombat playerCombatScr;
    private Animator anim;

    private bool isWalking;
    private float inputHorizontal;
    private float lookDirection = 1;
    private float movementSpeed;

    [HideInInspector()]
    public bool canWalk = true;
    [HideInInspector()]
    public bool canTurn = true;
    
    [Header("Publics")]
    public ParticleSystem psJumpLeft;
    public ParticleSystem psJumpRight;
    public ParticleSystem psJumpCurve;
    public int maxSpeed = 3;
    public float joystickDeadzone = 0.8f;

    [Header("Public Jump Values")]
	public bool delayedJump;
    public int jumpState = 0;
    public float upVelocity = 0f;
    public float jumpStartVelocity = 10;
    public float gravityMultiplierMax = 1.5f;
    public float gravityMultiplier = 1f;
    public float gravityMultiplierTime = 0.5f;
    public float playerStandardGravity = 1f;
    public float playerFallGravity = 1.2f;

    [Header("Raycast Values")]
    public LayerMask groundLayer;

    private Vector2 topLeft1;
    private Vector2 bottomRight1;
    public bool groundBefore = false;
    public float horLength1 = 0.1f;
    public float verLength1 = 0.5f;
    public float horOffset1 = -0.1f;
    public float verOffset1 = -0.5f;

    private Vector2 topLeft2;
    private Vector2 bottomRight2;
    public bool grounded = false;
    public float horLength2 = 0.35f;
    public float verLength2 = 0.1f;
    public float verOffset2 = -0.5f;

    private void Start () {
        rb = GetComponent<Rigidbody2D>();
        playerCombatScr = GetComponent<PlayerCombat>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        rb.gravityScale = playerStandardGravity;
    }
	
	private void Update () {
        Raycasts();

        if (canWalk){
            inputHorizontal = Input.GetAxisRaw("Horizontal");
        }
        else{
            inputHorizontal = 0;
        }
			
		JumpBehaviour ();
		JumpVisuals ();
        Movement();
        CheckForTurning();
        AssignAnimations();
    }

	private void JumpBehaviour(){
		if (Input.GetButtonDown("A")){
			if (grounded) {
				jumpState = 1;
				upVelocity = jumpStartVelocity;
			} 
			else {
				StartCoroutine (DelayedJump ());
			}
		}
		else if (Input.GetButtonUp("A")){
			if (rb.velocity.y > 0){
				StartCoroutine(GravityMultiplier());
				upVelocity *= 0.4f;
			}
		}
		else{
			upVelocity = rb.velocity.y;
		}
			
		if (grounded) {
			playerCombatScr.canKneel = true;
			gravityMultiplier = 1;
		} 
		else {
			playerCombatScr.canKneel = false;
		}

		if (rb.velocity.y >= 0){
			rb.gravityScale = playerStandardGravity * gravityMultiplier;
		}
		else if (rb.velocity.y < 0){
			rb.gravityScale = playerFallGravity * gravityMultiplier;
		}

		if(delayedJump){
			delayedJump = false;
			if (Input.GetButton ("A")) {
				jumpState = 1;
				upVelocity = jumpStartVelocity;
			} 
			else {
				jumpState = 1;
				upVelocity = jumpStartVelocity * 0.55f;
			}
		}
	}
		
	private IEnumerator DelayedJump(){
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime / 0.15f;
			if (grounded) {
				delayedJump = true;
				t = 1;
			}
			yield return null;
		}
	}

    private IEnumerator GravityMultiplier(){
        gravityMultiplier = gravityMultiplierMax;

        float difference = gravityMultiplierMax - 1f;
        float t = 1;

        while (t > 0){
            t -= Time.deltaTime / gravityMultiplierTime;
            gravityMultiplier = 1 + (t * difference);
            yield return null;
        } 
    }

	private void JumpVisuals(){
		//Animation and Particle Logic
		if (jumpState == 1 && rb.velocity.y < 3){
			jumpState = 2;
		}
		else if(jumpState == 0 && rb.velocity.y < -2){
			jumpState = 2;
		}
		else if (jumpState == 2 && grounded){
			psJumpLeft.Play();
			psJumpRight.Play();
			jumpState = 0;
		}
	}

    private void Raycasts(){
        topLeft1 = new Vector2(transform.position.x + ((horLength1 - horOffset1) * lookDirection), transform.position.y + verLength1 / 2 + verOffset1);
        bottomRight1 = new Vector2(transform.position.x - (horOffset1 * lookDirection), transform.position.y - verLength1 / 2 + verOffset1);
        groundBefore = Physics2D.OverlapArea(topLeft1, bottomRight1, groundLayer);

        topLeft2 = new Vector2(transform.position.x - horLength2 / 2, transform.position.y + verLength2/ 2 + verOffset2);
        bottomRight2 = new Vector2(transform.position.x + horLength2 / 2, transform.position.y - verLength2 / 2 + verOffset2);
        grounded = Physics2D.OverlapArea(topLeft2, bottomRight2, groundLayer); 
    }

    private void Movement(){
        Vector2 movementDirection;
        if (!groundBefore){
            movementDirection = new Vector2(movementSpeed * lookDirection, upVelocity);
        }
        else{
            movementDirection = new Vector2(0, upVelocity);
        }
        rb.velocity = movementDirection;
    }

    private void CheckForTurning(){
        //positive > lookDirection = 1: turning right
        if (inputHorizontal > joystickDeadzone){
            if (lookDirection == -1){
                if (canTurn){
                    movementSpeed = maxSpeed;
                    lookDirection = 1;
                    Turning(lookDirection);
                }
                else{
                    movementSpeed = 0;
                }
            }
            else{
                movementSpeed = maxSpeed;
            }
        }
        //positive < lookDirection = -1: turning left
        else if (inputHorizontal < -joystickDeadzone){
            if (lookDirection == 1){
                if (canTurn){
                    movementSpeed = maxSpeed;
                    lookDirection = -1;
                    Turning(lookDirection);
                }
                else{
                    movementSpeed = 0;
                }
            }
            else{
                movementSpeed = maxSpeed;
            }
        }
        //movementSpeed must be 0 when no input given
        else{
            movementSpeed = 0;
        }
    }

    private void Turning(float direction){
        //Sword Coll Sphere offset swap
        playerCombatScr.xPosSword = playerCombatScr.xPosSword * -1;
        //Flip Whole Player Object
        Vector3 playerScale = new Vector3(direction, 1, 1);
        transform.localScale = playerScale;
    }

    private void AssignAnimations(){
        if(movementSpeed > 0){
            isWalking = true;
        }
        else{
            isWalking = false;
        }
        
        anim.SetBool("isWalking", isWalking);
        anim.SetInteger("jumpState", jumpState);
    }

    private void OnDrawGizmosSelected(){
        Vector3 gizmoCenter1 = new Vector3(transform.position.x + ((horLength1 / 2 - horOffset1) * lookDirection), transform.position.y + verOffset1, 0);
        Vector3 gizmoSize1 = new Vector3(horLength1, verLength1, 0);

        Vector3 gizmoCenter2 = new Vector3(transform.position.x, transform.position.y + verOffset2, 0);
        Vector3 gizmoSize2 = new Vector3(horLength2, verLength2, 0);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(gizmoCenter1, gizmoSize1);
        Gizmos.DrawCube(gizmoCenter2, gizmoSize2);
    }
}
