using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitController : MonoBehaviour {

	public enum RabbitState {idle, running, dead}
    private PlayerStateDown playerStateDownScr;
	private Rigidbody2D rb;
	private Animator anim;
	private Transform player;
	private Vector2 movementVector;

	[Header("Debug Values")]
	public RabbitState rabbitState = RabbitState.idle;
	public bool isInPlayerRange = false;
	public bool isRunning = false;
	public bool goRun = true;
	public bool goIdle2 = false;
	public bool isInIdleCoroutine = false;
	public bool beforeWall = false;
	public bool alive = true;
    public bool turnedDirection = false;
	public float runSpeed;
	public float randomIdle1Speed;
	public int direction;

	[Header("Gameplay Values")]
	public ParticleSystem psRun;
	public ParticleSystem psDead;
	public float maxRunSpeed = 5f;
	public float runTimeMin = 0.5f;
	public float runTimeMax = 0.7f;

	[Header("Raycast Values BeforeGround")]
	public LayerMask groundLayer;
	public float horLength = 0.35f;
	public float horOffset = 0.2f;
	public float verLength = 0.7f;
	public float verOffset = -0.8f;

	private void Start () {
        playerStateDownScr = GameObject.Find("Player").GetComponent<PlayerStateDown>();
        rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		player = GameObject.Find ("Player").transform;

		randomIdle1Speed = Random.Range(1f, 1.5f);
		direction = Mathf.RoundToInt(transform.localScale.x);

		Idle();
	}

	private void OnTriggerEnter2D (Collider2D other){
		if(other.gameObject.tag == "Spike"){
			rabbitState = RabbitState.dead;
			StopCoroutine("RunCoroutine");
			alive = false;
            anim.SetTrigger("die");
			ScreenShake.instance.ShakeFixed (0.3f, 0.05f);
			psDead.gameObject.SetActive(true);
            //Wwise Stukje
            AkSoundEngine.PostEvent("Rabbit_Dead", gameObject);

			StartCoroutine(DieCoroutine());
		}
	}

	private IEnumerator DieCoroutine(){
		Time.timeScale = 0.2f;
        playerStateDownScr.StateDown();

        float t = 0f;
		while(t < 1){
			t += Time.deltaTime / 0.25f;
			Time.timeScale = 0.2f + (t * 0.8f);
			yield return null;
		}

        Time.timeScale = 1f;
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
		Raycasts();
		CheckStateTransitions();
		if(rabbitState == RabbitState.idle){
			Idle();
		}
		Movement();
		RandomIdle1Speed();
		AssignAnimations();
        if (isInPlayerRange && turnedDirection == false && alive) {
            CheckDirection();
        }
	}

	private void Raycasts(){
		Vector2 topLeft = new Vector2(transform.position.x + ((horLength - horOffset) * direction), transform.position.y + verOffset + (verLength / 2));
		Vector2 bottomRight = new Vector2(transform.position.x - (horOffset * direction), transform.position.y + verOffset - (verLength / 2));
		beforeWall = Physics2D.OverlapArea(topLeft, bottomRight, groundLayer);
	}

	private void OnDrawGizmosSelected(){
		Vector3 gizmoCenter = new Vector3(transform.position.x + ((horLength / 2 - horOffset) * direction), transform.position.y + verOffset, 0);
		Vector3 gizmoSize = new Vector3(horLength, verLength, 0);

		//BeforeWall = Orange
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawCube(gizmoCenter, gizmoSize);
	}

	//Runs each frame
	private void CheckStateTransitions(){
		if(rabbitState == RabbitState.idle){
			if(isInPlayerRange){
				Run();
			}
		}
		else if(rabbitState == RabbitState.running){
			if(isRunning == false){
				rabbitState = RabbitState.idle;
				goRun = true;
			}
		}
	}

	//Runs each frame if RabbitState == idle
	private void Idle(){
		if(isInIdleCoroutine == false){
			StartCoroutine(Idle2Animation());
		}
	}

	//Loopt de hele tijd, speelt zichzelf weer af als hij klaar is
	private IEnumerator Idle2Animation(){
		isInIdleCoroutine = true;
		float waitBeforeIdle2 = Random.Range(2f, 3f);

		yield return new WaitForSeconds(waitBeforeIdle2);

		int doIdle2 = Random.Range(0, 2);
		if(doIdle2 == 1){
			goIdle2 = true;
			yield return null;
			goIdle2 = false;
		}
		else{
			goIdle2 = false;
		}

		isInIdleCoroutine = false;
	}

	//Runs each frame if inPlayerRange
	private void CheckDirection(){
		float xDifference = transform.position.x - player.position.x;
		Vector3 lScale;
		if(xDifference >= 0){
			if(beforeWall){
				direction = -1;
                StartCoroutine(OneSecondThisDirection());

            }
			else{
				direction = 1;
			}
			lScale = new Vector3(direction, 1, 1);
		}
		else{
			if(beforeWall){
				direction = 1;
                StartCoroutine(OneSecondThisDirection());
            }
			else{
				direction = -1;
			}
			lScale = new Vector3(direction, 1, 1);
		}
		transform.localScale = lScale;
	}

    private IEnumerator OneSecondThisDirection() {
        turnedDirection = true;
        yield return new WaitForSeconds(1f);
        turnedDirection = false;
    }

	//Runs each frame if inPlayerRange
	private void Run(){
		rabbitState = RabbitState.running;
		
		if(goRun){
			StartCoroutine(RunCoroutine());
		}
	}

	//Runs 1 time!
	private IEnumerator RunCoroutine(){
		goRun = false;
		isRunning = true;
		float runTime = Random.Range(runTimeMin, runTimeMax);
		float partRunTime = runTime/4;

		psRun.gameObject.SetActive(true);
        //Wwise stukje
        AkSoundEngine.PostEvent("Rabbit_Run", gameObject);

		//Constant Max Speed
		runSpeed = maxRunSpeed;
		yield return new WaitForSeconds(partRunTime * 3);

		//Decelerating
		float t = 1;
		while(t > 0){
			t -= Time.deltaTime / partRunTime;
			runSpeed = maxRunSpeed * t;
			yield return null;
		}

		psRun.gameObject.SetActive(false);
		isRunning = false;
        //Wwise stukje
        AkSoundEngine.PostEvent("Rabbit_Stop", gameObject);
    }

	//Runs each frame
	private void Movement(){
		if(rabbitState == RabbitState.idle){
			movementVector = new Vector2(0, rb.velocity.y);
		}
		else if(rabbitState == RabbitState.running){
			movementVector = new Vector2(runSpeed * direction, rb.velocity.y);
		}
		if(alive){
			rb.velocity = movementVector;
		}
		else{
			rb.velocity = new Vector2(0, 0);
		}
	}

	private void RandomIdle1Speed(){
		if( anim.GetCurrentAnimatorStateInfo(0).IsName("rabbit_idle1")){
       		anim.speed = randomIdle1Speed;
		}
		else{
			anim.speed = 1f;
		}
	}

	//Runs each frame
	private void AssignAnimations(){
		anim.SetBool("isRunning", isRunning);
		anim.SetBool("goIdle2", goIdle2);
	}
}
