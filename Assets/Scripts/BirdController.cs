using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

	private Rigidbody2D rb;
	private Animator anim;
	private Transform player;
	private Vector2 flyDirection;
	private bool isFlying = false;
	private int idleState = 1;

    private float timer;
    private float idleTime;
    private float flySpeed;
    public float flyCooldownTime = 0.5f;
    
	public ParticleSystem ps_flying;
	public bool canPick = true;
    public bool isGroupBird = true;

	private void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		player = GameObject.Find("Player").transform;

        if (transform.parent != null){
            BirdGroupTrigger groupTrigger = transform.parent.GetComponent<BirdGroupTrigger>();
            if (groupTrigger != null) {
                groupTrigger.onPlayerClose += TriggerFly;
            }
        }

        timer = 0f;
        idleTime = Random.Range(2f, 2.5f);
        flySpeed = Random.Range(2.5f, 3.2f);
    }

    private void Update () {
        timer += Time.deltaTime;

        if(timer > idleTime && isFlying == false){
            timer = 0f;
            int randomRange = Random.Range(0, 20);
            if (randomRange == 0){
                FlipSprite();
            }
            else if (randomRange < 5 && canPick){
                idleState = 3;
                anim.SetTrigger("idle3");
            }
            else if (randomRange > 5 && randomRange < 13){
                idleState = 2;
            }
            else{
                idleState = 1;
            }
        }
        
		AssignAnimations();
	}

	private void FlipSprite(){
		Vector3 lScale = new Vector3(transform.localScale.x * -1, 1, 1);
		transform.localScale = lScale;
	}

	private void TriggerFly(){
		StartCoroutine(FlyBehaviour());
        //Wwise stukje
        AkSoundEngine.PostEvent("Bird_Group", gameObject);
    }

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			StartCoroutine(FlyBehaviour());
            //Wwise stukje
            AkSoundEngine.PostEvent("Bird_Single", gameObject);
		}
	}

	private IEnumerator FlyBehaviour(){
		isFlying = true;
		float cooldown = Random.Range(0, flyCooldownTime);

		yield return new WaitForSeconds(cooldown);

		float xDirection;
		float xDifference = player.position.x - transform.position.x;
		if(xDifference <= 0){
			xDirection = Random.Range(0.1f, 1f);
			Vector3 lScale;
			lScale = new Vector3(1, 1, 1);
			transform.localScale = lScale;
		}
		else{
			xDirection = Random.Range(-0.1f, -1f);
			Vector3 lScale;
			lScale = new Vector3(-1, 1, 1);
			transform.localScale = lScale;
		}
		
        Vector2 flyDirection = new Vector2(xDirection, 1);
        rb.velocity = flyDirection.normalized * flySpeed;

        idleState = 4;
		ps_flying.Play();

		StartCoroutine(DestroyObject());
	}

	private IEnumerator DestroyObject(){
		yield return new WaitForSeconds(10f);
		Destroy(this.gameObject);
	}

	private void AssignAnimations(){
		anim.SetInteger("idleState", idleState);
    }

}
