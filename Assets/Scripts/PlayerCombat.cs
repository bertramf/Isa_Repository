using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Designed for all player combat: attack, get damage, health system, spells
public class PlayerCombat : MonoBehaviour {

    private PlayerMovement playerMovementScr;
    private PlayerCourage playerCourageScr;
    private EnemyController enemyControllerScr;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isFillingCourage;

    //Melee Booleans
    [HideInInspector()]
    public bool swordActivated;
    private bool goAttack;
    private bool canAttack;

	//Kneel Booleans
	public bool canKneel;
	private bool isInKneelState;
    private bool kneelingAnim;

    [Header("Courage Values")]
    public float courageBoost = 4f;
    public float courageCost = 1f;

    [Header("Publics")]
    public ParticleSystem ps_healCharge;
    public ParticleSystem ps_healFinished;
    public LayerMask enemyLayer;
    public Color colorStart;
    public Color colorEnd;
    public int swordDamage = 1;
    public float attackCooldown = 0.4f;
    public float healTime = 1.2f;

    [Header("Sword Sphere Variables")]
    public float swordCollRadius = 0.25f;
    public float yPosSword = -0.15f;
    public float xPosSword = 0.3f;

    private void Start () {
        playerMovementScr = GetComponent<PlayerMovement>();
        playerCourageScr = GetComponent<PlayerCourage>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        canAttack = true;
		canKneel = true;
    }
	
	private void Update () {
        //Attack
		if (Input.GetButtonDown("X") && canAttack && !isInKneelState){
            goAttack = true;
            StartCoroutine(AttackTimer());
        }
        else{
            goAttack = false;
        }

        //Ask for help
		if (Input.GetButtonDown("Y") && canKneel){
            StartCoroutine(AskForHelp());
        }

        CheckHit();
        AssignAnimations();
    }

    private IEnumerator AskForHelp(){
        //Set start values
		isInKneelState = true;
        kneelingAnim = true;
        playerMovementScr.canWalk = false;

        yield return new WaitForSeconds(0.3f);

        ps_healCharge.Play();

        //Hier gaat de timer lopen
        float timer = Time.time + healTime;
        while (Input.GetButton("Y") && Time.time < timer){
            ScreenShake.instance.Shake(0.03f);
            yield return null;
            if(Time.time >= timer){
                //Enhance Courage
                ScreenShake.instance.ShakeFixed(0.05f, 0.15f);
                StartCoroutine(FlickerSprite());
                ps_healFinished.Play();
                playerCourageScr.CourageUp(courageBoost);
            }
        }

        //Set end values: hier komt hij alleen als hij klaar is met de timer
        ps_healCharge.Stop();
        kneelingAnim = false;
        playerMovementScr.canWalk = true;
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
		isInKneelState = false;
    }

    private void CheckHit(){
        Vector2 localCircleCenter = new Vector2(xPosSword + transform.position.x, yPosSword + transform.position.y);

        if (swordActivated){
            bool trueHit = false;
            swordActivated = false;
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
		
    private void AssignAnimations(){
        anim.SetBool("goAttack", goAttack);
        anim.SetBool("isKneeling", kneelingAnim);
    }

    private IEnumerator AttackTimer(){
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

	public void Hit(){

	}

    private void OnDrawGizmos(){
        Vector3 circleCenter3D = new Vector3(xPosSword + transform.position.x, yPosSword + transform.position.y, 0);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(circleCenter3D, swordCollRadius);
    }

}
