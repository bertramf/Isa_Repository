using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GordeldierController : MonoBehaviour {

	public enum CreatureState {idle, charging}
	private Rigidbody2D rb;
	private Animator anim;
	private Transform player;

	[Header("Private Values")]
	private Vector2 movementVector;
	private int direction = 0;
	private bool goCharge = true;
	private bool goIdleCooldown = true;

	[Header("Important & Debug Public Values")]
	public CreatureState moleState = CreatureState.idle;
	public ParticleSystem psCharge;
	public bool isInPlayerRange;
	public bool idleCooldownIsOver = false;
	public bool isCharging = false;
	public float chargeSpeed = 0f;
	public float maxChargeSpeed = 5f;
	public float chargeTime = 1.5f;
	public float idleCooldown = 1.5f;
	public float buildUpTime = 0.4f;

	private void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		player = GameObject.Find ("Player").transform;

		Idle();
	}

	private void OnTriggerStay2D (Collider2D other){
		if(other.gameObject.tag == "Player"){
			isInPlayerRange = true;
		}
	}

	private void OnTriggerExit2D (Collider2D other){
		if(other.gameObject.tag == "Player"){
			isInPlayerRange = false;
		}
	}
	
	private void Update () {
		CheckStateTransitions();
		Movement();
		AssignAnimations();
	}

	private void CheckStateTransitions(){
		if(moleState == CreatureState.idle){
			if(isInPlayerRange && idleCooldownIsOver){
				Charge();
			}
		}
		else if(moleState == CreatureState.charging){
			if(isCharging == false){
				Idle();
			}
		}
	}

	//Constant Frames running
	private void Idle(){
		moleState = CreatureState.idle;
		goCharge = true;
		
		if(goIdleCooldown){
			StartCoroutine(IdleCoolDown());
		}
	}

	//1 Frame running
	private void Charge(){
		moleState = CreatureState.charging;
		goIdleCooldown = true;

		if(goCharge){
			CheckDirection();
			StartCoroutine(ChargeCoroutine());
		}
	}

	private IEnumerator IdleCoolDown(){
		goIdleCooldown = false;
		idleCooldownIsOver = false;
        //Wwise stukje
        AkSoundEngine.PostEvent("Gordeldier_Idle", gameObject);
        yield return new WaitForSeconds(idleCooldown);
		idleCooldownIsOver = true;
	}

	private void CheckDirection(){
		float xDifference = transform.position.x - player.position.x;
		Vector3 lScale;
		if(xDifference >= 0){
			direction = -1;
			lScale = new Vector3(1, 1, 1);
		}
		else{
			direction = 1;
			lScale = new Vector3(-1, 1, 1);
		}
		transform.localScale = lScale;
	}

	private IEnumerator ChargeCoroutine(){
		goCharge = false;
		isCharging = true;

		chargeSpeed = 0f;
        //Wwise stukje
        AkSoundEngine.PostEvent("Gordeldier_BuildUp", gameObject);
        yield return new WaitForSeconds(buildUpTime);

		psCharge.gameObject.SetActive(true);
		float totalChargeTime = chargeTime * Random.Range(0.8f, 1.2f);
		float partChargeTime = totalChargeTime/4;
        AkSoundEngine.PostEvent("Gordeldier_Attack", gameObject);

        //Constant Max Speed
        chargeSpeed = maxChargeSpeed;
		yield return new WaitForSeconds(partChargeTime * 3);

		//Deccelerating
		float t = 1;
		while(t > 0){
			t -= Time.deltaTime / partChargeTime;
			chargeSpeed = maxChargeSpeed * t;
			yield return null;
		}

		psCharge.gameObject.SetActive(false);
		isCharging = false;
	}

	private void Movement(){
		if(moleState == CreatureState.idle){
			movementVector = new Vector2(0, rb.velocity.y);
		}
		if(moleState == CreatureState.charging){
			movementVector = new Vector2(direction * chargeSpeed, rb.velocity.y);
		}
		rb.velocity = movementVector;
	}

	private void AssignAnimations(){
		anim.SetBool("isCharging", isCharging);
	}
}
