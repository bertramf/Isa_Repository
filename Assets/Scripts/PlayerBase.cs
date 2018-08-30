using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Designed for basic movement: idle & walking
public class PlayerBase : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator anim;
    private PlayerStats playerStats;
    private PlayerRaycasts playerRaycastsScr;
    private PlayerJump playerJumpScr;
    private PlayerDash playerDashScr;
    private CheckpointBase checkpointBaseScr;
    private PlayerState playerState;
    private FadeBlackScreen fadeBlackScreenScr;
    private CameraController2 cameraControllerScr;
    [HideInInspector()]
    public bool isInvulnerable;
    private bool isInGrass;
    private float inputHorizontal;

    public ParticleSystem psAwake;
    public ParticleSystem psCourageous;
    public ParticleSystem psGrassWalkingRight;
    public ParticleSystem psGrassWalkingLeft;
    public ParticleSystem psGrassDashingRight;
    public ParticleSystem psGrassDashingLeft;
    public float airDelayTime = 0.15f;
    public float joystickDeadzone = 0.8f;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        anim = transform.Find("PlayerVisuals").GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
        playerRaycastsScr = GetComponent<PlayerRaycasts>();
        playerJumpScr = GetComponent<PlayerJump>();
        playerDashScr = GetComponent<PlayerDash>();
        checkpointBaseScr = GameObject.Find("Manager_Checkpoints").GetComponent<CheckpointBase>();
        playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
        fadeBlackScreenScr = GameObject.Find("HUD_BlackScreen").GetComponent<FadeBlackScreen>();
        cameraControllerScr = GameObject.Find("Main Camera").GetComponent<CameraController2>();

        StartCoroutine(FadeBlackScreenOut());
        StartCoroutine(SetPlayerStartValues());
    }

    private IEnumerator FadeBlackScreenOut() {
        fadeBlackScreenScr.blackScreenGroup.alpha = 1;
        yield return new WaitForSeconds(playerStats.blackScreenBlackTime);
        fadeBlackScreenScr.StartCoroutine(fadeBlackScreenScr.FadeOut(playerStats.blackScreenFadeOutTime));
    }

    private IEnumerator SetPlayerStartValues() {
        anim.SetBool("isHitted", false);
        transform.position = new Vector3(checkpointBaseScr.respawnPoint.x, checkpointBaseScr.respawnPoint.y, transform.position.z);
        cameraControllerScr.SetCameraStartValues(transform.position.x, transform.position.y);

        if (checkpointBaseScr.courageCheckpoint) {
            playerState.BecomeCourageous();
        }
        else {
            playerState.BecomeScared();
        }
        psAwake.Play();

        yield return new WaitForSeconds(playerStats.wakeUpTime);

        UnFreezePlayer();
    }

    public void FreezePlayer() {
        playerStats.canControl = false;
        rb.isKinematic = true;
        playerStats.yVelocity = 0f;
        rb.velocity = new Vector2(0, 0);
    }

    public void UnFreezePlayer() {
        playerStats.canControl = true;
        rb.isKinematic = false;
    }

    public void SetInvulnerable() {
        StartCoroutine(Invulnerable());
    }

    private IEnumerator Invulnerable() {
        isInvulnerable = true;
        yield return new WaitForSeconds(playerStats.getHittedTime + playerStats.invulnerableTime);
        isInvulnerable = false;
    }

    private void Update() {
        if (playerStats.canControl) {
            playerRaycastsScr.Raycasts();

            playerJumpScr.JumpBehaviour();
            playerJumpScr.SetGravity();
            playerDashScr.Dash();

            HorizontalMovement();
            Movement();
        }

        playerJumpScr.JumpStates();
        PlayerStateVisuals();
        GrassParticles();
        AssignAnimations();
        AssignAudioEvents();
    }

    private void HorizontalMovement() {
        //Check for Input
        if (playerStats.canWalk) {
            inputHorizontal = Input.GetAxisRaw("Horizontal");
        }
        else {
            inputHorizontal = 0;
        }

        //Hij gaat gewoon naar rechts of naar links met playerStats.maxMovementSpeed
        if ((inputHorizontal > joystickDeadzone && playerStats.lookDirection == 1) || (inputHorizontal < -joystickDeadzone && playerStats.lookDirection == -1)) {
            playerStats.movementSpeed = playerStats.maxMovementSpeed;
        }
        //Turning to right
        else if (inputHorizontal > joystickDeadzone && playerStats.lookDirection == -1) {
            playerStats.movementSpeed = playerStats.maxMovementSpeed;
            TurnDirection(1);
        }
        //Turning to left
        else if (inputHorizontal < -joystickDeadzone && playerStats.lookDirection == 1) {
            playerStats.movementSpeed = playerStats.maxMovementSpeed;
            TurnDirection(-1);
        }
        //Geen input: hij valt dus stil of decelereert
        else {
            if (playerStats.grounded) {
                playerStats.movementSpeed = 0;
            }
            else {
                if (playerStats.movementSpeed == playerStats.maxMovementSpeed) {
                    StartCoroutine(DecelerateAirMovement());
                }
            }
        }
    }

    private void TurnDirection(int direction) {
        if (playerStats.canTurn) {
            playerStats.lookDirection = direction;
            Vector3 playerScale = new Vector3(playerStats.lookDirection, 1, 1);
            transform.localScale = playerScale;
        }
        //Als je niet kunt draaien maar dat wel probeert blijf je stilstaan
        else {
            playerStats.movementSpeed = 0;
        }
    }

    private IEnumerator DecelerateAirMovement() {
        float t = 1;
        while (t >= 0) {
            t -= Time.deltaTime / airDelayTime;
            playerStats.movementSpeed = playerStats.maxMovementSpeed * t;
            yield return null;
        }
    }

    private void Movement() {
        playerStats.yVelocity = playerJumpScr.GetYVel();
        Vector2 movementDirection;
        if (playerStats.beforeWall) {
            if (playerStats.isDashing) {
                movementDirection = new Vector2(0, 0);
            }
            else {
                movementDirection = new Vector2(0, playerStats.yVelocity);
            }
        }
        else {
            if (playerStats.isDashing) {
                movementDirection = new Vector2(playerStats.dashSpeed * playerStats.lookDirection, 0);
            }
            else {
                movementDirection = new Vector2(playerStats.movementSpeed * playerStats.lookDirection, playerStats.yVelocity);
            }
        }
        rb.velocity = movementDirection;
    }

    private void PlayerStateVisuals() {
        if (playerState.state == CourageState.scared) {
            if (psCourageous.isPlaying == true) {
                psCourageous.Stop();
            }
        }
        else if (playerState.state == CourageState.courage && playerStats.canControl) {
            if (psCourageous.isPlaying == false) {
                psCourageous.Play();
            }
        }
    }

    //GrassVisuals
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Grass"){
			isInGrass = true;
            //Wwise stukje
            AkSoundEngine.SetSwitch("Ground", "Grass", gameObject);
        }
        else if(other.gameObject.tag == "GrassHigh") {
            isInGrass = true;
            //Wwise stukje
            AkSoundEngine.SetSwitch("Ground", "Grass_High", gameObject);
        }
	}

    //GrassVisuals
    private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Grass" || other.gameObject.tag == "GrassHigh") {
			isInGrass = false;
            //Wwise stukje
            AkSoundEngine.SetSwitch("Ground", "Earth", gameObject);
        }
    }

    //GrassVisuals
    private void GrassParticles(){
		var emWalkingLeft = psGrassWalkingLeft.emission;
		var emWalkingRight = psGrassWalkingRight.emission;
		var emDashingLeft = psGrassDashingLeft.emission;
		var emDashingRight = psGrassDashingRight.emission;

		//GrassWalk particles
		if(isInGrass && (inputHorizontal > joystickDeadzone || inputHorizontal < -joystickDeadzone) && !playerStats.isDashing && playerStats.canControl){
			if(playerState.state == CourageState.courage){
				if(playerStats.lookDirection == 1){
					emWalkingRight.rateOverTime = 15f;
					emWalkingLeft.rateOverTime = 0f;
				}
				else if(playerStats.lookDirection == -1){
					emWalkingRight.rateOverTime = 0f;
					emWalkingLeft.rateOverTime = 15f;
				}
			}
			else if(playerState.state == CourageState.scared){
				if(playerStats.lookDirection == 1){
					emWalkingRight.rateOverTime = 8f;
					emWalkingLeft.rateOverTime = 0f;
				}
				else if(playerStats.lookDirection == -1){
					emWalkingRight.rateOverTime = 0f;
					emWalkingLeft.rateOverTime = 8f;
				}
			}
		}
		else{
			emWalkingRight.rateOverTime = 0f;
			emWalkingLeft.rateOverTime = 0f;
		}
		
		//GrassDash particles
		if(isInGrass && playerStats.isDashing && playerStats.canControl){
            //Wwise stukje
            AkSoundEngine.PostEvent("Dash_Particles", gameObject);
			if(playerStats.lookDirection == 1){
				emDashingRight.rateOverTime = 100f;
				emDashingLeft.rateOverTime = 0f;
			}
			else if(playerStats.lookDirection == -1){
				emDashingRight.rateOverTime = 0f;
				emDashingLeft.rateOverTime = 100f;
			}
		}
		else{
			emDashingRight.rateOverTime = 0f;
			emDashingLeft.rateOverTime = 0f;
		}
	}

    private void AssignAnimations(){
		bool isWalking;
        
        if(playerStats.movementSpeed > 0){
            isWalking = true;
        }
        else{
            isWalking = false;
        }
        
        anim.SetBool("isWalking", isWalking);
		anim.SetBool ("isDashing", playerStats.isDashing);
		anim.SetInteger ("jumpState", playerJumpScr.jumpState);
    }

    private void AssignAudioEvents(){
        if (playerStats.movementSpeed > 0 && playerStats.grounded && playerStats.canControl){
            //Wwise stukje
            AkSoundEngine.PostEvent("Walk", gameObject);
        }
    }

}
