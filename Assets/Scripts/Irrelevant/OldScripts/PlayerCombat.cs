using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Designed for all player combat: attack, get damage, health system, spells
public class PlayerCombat : MonoBehaviour {
	
	private Animator anim;
	private SpriteRenderer spriteRenderer;
	private PlayerStats playerStats;
    private PlayerCourage playerCourageScr;
    private EnemyController enemyControllerScr;
    private bool isFillingCourage;

    //Private Combat Booleans
    private bool goAttack;
    private bool canAttack;
	private bool canKneel;
	private bool isKneeling;

    [Header("Public Combat Variables")]
	public int swordDamage = 1;
    public float courageBoost = 4f;
    public float courageCost = 1f;
	public float attackCooldown = 0.4f;
	public float healTime = 1.2f;
	public float healCooldown = 0.8f;

    [Header("Publics")]
    public ParticleSystem ps_healCharge;
    public ParticleSystem ps_healFinished;
    public LayerMask enemyLayer;
    public Color colorStart;
    public Color colorEnd;
 
    [Header("Sword Sphere Variables")]
    public float swordCollRadius = 0.25f;
    public float yPosSword = -0.15f;
    public float xPosSword = 0.3f;

    private void Start () {
		anim = transform.Find ("PlayerVisuals").GetComponent<Animator> ();
		spriteRenderer = transform.Find ("PlayerVisuals").GetComponent<SpriteRenderer>();
		playerStats = GetComponent<PlayerStats> ();
        playerCourageScr = GetComponent<PlayerCourage>();

        canAttack = true;
		canKneel = true;
    }
	
	private void Update () {
		AttackInput ();
		CheckHitAnimation();

		KneelInput ();

        AssignAnimations();
    }

	private void AttackInput(){
		if (Input.GetButtonDown("X") && canAttack && !isKneeling){
			goAttack = true;
			StartCoroutine(AttackTimer());
		}
		else{
			goAttack = false;
		}
	}

	private void CheckHitAnimation(){
		Vector2 localCircleCenter = new Vector2(xPosSword + transform.position.x, yPosSword + transform.position.y);

		if (playerStats.swordActivated){
			bool trueHit = false;
			playerStats.swordActivated = false;
			Collider2D[] enemyColl = Physics2D.OverlapCircleAll(localCircleCenter, swordCollRadius, enemyLayer);
			for(int i = 0; i < enemyColl.Length; i++){
				enemyControllerScr = enemyColl[i].gameObject.GetComponent<EnemyController>();
				bool validHit = false;
				enemyControllerScr.GetHitted(out validHit);
				trueHit = validHit;
			}
			if (trueHit){
				ScreenShake.instance.ShakeFixed(0.05f, 0.05f);
				playerCourageScr.CourageDown(courageCost);
			}
		}
	}

	private void KneelInput(){
		if (playerStats.grounded) {
			if (Input.GetButton ("Y") && canKneel && !isKneeling) {
				StartCoroutine (KneelState ());
			} 
		}
	}

	private IEnumerator KneelState(){
        //Set start values
		isKneeling = true;
        playerStats.canWalk = false;

        yield return new WaitForSeconds(0.3f);

        ps_healCharge.Play();

        //Hier gaat de timer lopen
        float timer = Time.time + healTime;
        while (Input.GetButton("Y") && Time.time < timer){
            ScreenShake.instance.Shake(0.03f);
            yield return null;
            if(Time.time >= timer){
                //Enhance Courage
				//isKneeling = false;
                ScreenShake.instance.ShakeFixed(0.05f, 0.15f);
				StartCoroutine (KneelCooldown ());
                StartCoroutine(FlickerSprite());
                ps_healFinished.Play();
                playerCourageScr.CourageUp(courageBoost);
            }
        }

        //Set end values: hier komt hij alleen als hij klaar is met de timer OF als je stopt met y indrukken!
		isKneeling = false;
		playerStats.canWalk = true;
        ps_healCharge.Stop();
    }

	private IEnumerator KneelCooldown(){
		canKneel = false;
		yield return new WaitForSeconds (healCooldown);
		canKneel = true;
	}

    private IEnumerator FlickerSprite(){
        float lerpTime = 0.15f;
        float t = 0;
        while (t < 1){
            t += Time.deltaTime / lerpTime;
            spriteRenderer.color = Color.Lerp(colorStart, colorEnd, t);
            yield return null;
        }
        float t2 = 0;
        while (t2 < 1){
            t2 += Time.deltaTime / lerpTime / 2;
            spriteRenderer.color = Color.Lerp(colorEnd, colorStart, t2);
            yield return null;
        }
    }

    private void AssignAnimations(){
        anim.SetBool("goAttack", goAttack);
		anim.SetBool("isKneeling", isKneeling);
    }

    private IEnumerator AttackTimer(){
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

	public void GetHitted(){
		//Wat moet er met de speler gebeuren als je gehit wordt?
	}

    private void OnDrawGizmos(){
        Vector3 circleCenter3D = new Vector3(xPosSword + transform.position.x, yPosSword + transform.position.y, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(circleCenter3D, swordCollRadius);
    }

}
