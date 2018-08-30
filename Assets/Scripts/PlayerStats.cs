using UnityEngine;

//alle public stats vd speler
//Model view : wat voor input & controller : wat kun je doen
public class PlayerStats : MonoBehaviour {

	private PlayerState playerState;

    [HideInInspector()]
    public bool canControl = false;
    [HideInInspector()]
    public bool lockCam = false;

    [HideInInspector()]
    public bool colliderGrounded = false;
    [HideInInspector()]
    public bool grounded = false;
    [HideInInspector()]
    public bool beforeGround = false;
    [HideInInspector()]
    public bool beforeWall = false;

    //----------------------------------------------------------]
    public bool hitsCamBorder = false;
    public float yBorder;

    [HideInInspector()]
    public bool isDashing = false;
    [HideInInspector()]
    public bool inAttackAnim = false;
    [HideInInspector()]
    public bool swordActivated = false;

    [HideInInspector()]
    public bool canTurn = true;
    [HideInInspector()]
    public bool canWalk = true;

    [HideInInspector()]
    public int lookDirection = 1;
    [HideInInspector()]
    public float dashSpeed = 0f;
    [HideInInspector()]
    public float yVelocity;

	[Header("PlayerState dependent variables")]
	public float maxJumpVelocity;
	public float movementSpeed;
	public float maxMovementSpeed;

    [Header("Important Animation Timings")]
    public float getHittedTime = 1.5f;
    public float wakeUpTime = 3f;

    [Header("Important Gameplay Tweak Publics")]
    public float invulnerableTime = 1f;
	public float blackScreenBlackTime = 0.5f;
	public float blackScreenFadeInTime = 0.8f;
    public float blackScreenFadeOutTime = 0.8f;
    public float restRespawnTime = 2f;
	public float maxJumpVelocity_scared = 7f;
	public float maxJumpVelocity_courage = 10f;
	public float maxMovementSpeed_scared = 2.5f;
	public float maxMovementSpeed_courage = 3.5f;

	private void Start(){
		playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
	}

	private void Update(){
		if(playerState.state == CourageState.scared){
			maxJumpVelocity = maxJumpVelocity_scared;
			maxMovementSpeed = maxMovementSpeed_scared;
        }
		else if(playerState.state == CourageState.courage && canControl){
			maxJumpVelocity = maxJumpVelocity_courage;
			maxMovementSpeed = maxMovementSpeed_courage;
            
        }
	}

}