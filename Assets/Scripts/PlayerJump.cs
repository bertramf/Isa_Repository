using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handles yvel, jump imput & jump visuals
public class PlayerJump : MonoBehaviour {

	private Rigidbody2D rb;
	private PlayerStats playerStats;
	private bool earlyRelease = false;

    private float releaseFromPlatformTime;
    private float jumpTime;
	private float gravityMultiplier = 1f;

    [HideInInspector()]
    public float upVelocity = 0f;
    [HideInInspector()]
    public int jumpState = 0;

    public ParticleSystem psJumpLeft1;
    public ParticleSystem psJumpLeft2;
    public ParticleSystem psJumpRight1;
    public ParticleSystem psJumpRight2;
    public float maxFakeGroundedTime = 0.08f;
	public float minimumJumpInputTime = 0.08f;
    public float maxJumpVelocity = 10f;
	public float minJumpVelocity = -7f;
    public float gravityMultiplierMax = 1.2f;
    public float playerStandardGravity = 1f;
    public float playerFallGravity = 1.2f;

	private void Start(){
		rb = GetComponent<Rigidbody2D>();
		playerStats = GetComponent<PlayerStats> ();
		rb.gravityScale = playerStandardGravity;

        releaseFromPlatformTime = maxFakeGroundedTime;
    }

    private void Update() {
        if (playerStats.colliderGrounded) {
            playerStats.grounded = true;
            releaseFromPlatformTime = maxFakeGroundedTime;
        }
        else if (!playerStats.colliderGrounded && upVelocity < 0.1f && releaseFromPlatformTime > 0) {
            releaseFromPlatformTime -= Time.deltaTime;
            playerStats.grounded = true;
        }
        else {
            releaseFromPlatformTime = -1f;
            playerStats.grounded = false;
        }
    }

    private void FixedUpdate() {
        //Dit staat in fixedUpdate zodat de tijd niet afhankelijk is van de performance van een device
        if (jumpState > 0) {
            jumpTime += Time.deltaTime;
        }
    }

    public void JumpBehaviour(){
        //Ga omhoog als je input A geeft
		if (Input.GetButtonDown ("A") && playerStats.grounded) {
			jumpState = 1;
			jumpTime = 0f;
			upVelocity = playerStats.maxJumpVelocity;
            AudioJump();
        } 
		else if (Input.GetButtonUp ("A") && jumpTime < minimumJumpInputTime) {
			earlyRelease = true;
		}
		//Dit gebeurd er nadat je na een earlyRelease bij de minimumJumpInputTime komt
		else if (earlyRelease && jumpTime >= minimumJumpInputTime) {
			if (rb.velocity.y > 0) {
				earlyRelease = false;
				gravityMultiplier = gravityMultiplierMax;
				upVelocity *= 0.4f;
			}
		}
		//Dit gebeurd er als je gewoon loslaat nadat de tijd verstreken is
		else if (Input.GetButtonUp ("A") && jumpTime >= minimumJumpInputTime) {
			if (rb.velocity.y > 0) {
				gravityMultiplier = gravityMultiplierMax;
				upVelocity *= 0.4f;
			}
		} 
		else {
			upVelocity = Mathf.Clamp (rb.velocity.y, minJumpVelocity, playerStats.maxJumpVelocity);
		}

	}

	public void SetGravity(){
		if (playerStats.grounded) {
			gravityMultiplier = 1;
		}

		if (rb.velocity.y >= 0) {
			rb.gravityScale = playerStandardGravity * gravityMultiplier;
		} 
		else if (rb.velocity.y < 0) {
			rb.gravityScale = playerFallGravity * gravityMultiplier;
		}
	}

    public float GetYVel() {
        return upVelocity;
    }

    public void JumpStates() {
        //Van jumpstate 1 naar jumpstate 2 (normale jump)
        if (jumpState == 1 && rb.velocity.y < 2) {
            jumpState = 2;
        }

        //Van lopen naar vallen
        if (jumpState == 0 && rb.velocity.y < -2) {
            jumpState = 2;
        }

        //Wanneer grounded
        if (jumpState == 2 && playerStats.colliderGrounded) {
            upVelocity = 0f;
            jumpState = 0;
            JumpParticles();
            AudioLand();
        }
    }

    private void JumpParticles() {
        //Set Right xScale for each particleGameObject
        Vector3 vfxScale = new Vector3(playerStats.lookDirection, 1, 1);
        psJumpLeft1.transform.localScale = vfxScale;
        psJumpLeft2.transform.localScale = vfxScale;
        psJumpRight1.transform.localScale = vfxScale;
        psJumpRight2.transform.localScale = vfxScale;
        //Play each particle
        psJumpLeft1.Play();
        psJumpLeft2.Play();
        psJumpRight1.Play();
        psJumpRight2.Play();
    }

    private void AudioJump() {
        //Wwise stukje
        AkSoundEngine.PostEvent("Jump", gameObject);
    }

    private void AudioLand() {
        //Wwise stukje
        AkSoundEngine.PostEvent("Land", gameObject);
    }

}
