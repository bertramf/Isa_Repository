using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialBehaviour : MonoBehaviour {

	public enum SocialState{idle, looking, scare, laughing}
	public enum LookState{leftFar, leftNear, middle, rightNear, rightFar}
    private FadeBlackScreen fadeBlackScreenScr;
    private Animator anim;
	private Transform player;
    private bool canStartCoroutine = true;

    [Header("Debug")]
	public SocialState socialState = SocialState.idle;
	public LookState lookState = LookState.middle;
    public float xDistance;

    [Header("Important Values")]
    public GameObject triggerObj;
    public ParticleSystem ps_scare1;
    public ParticleSystem ps_scare2;
    public ParticleSystem ps_laugh;
    public bool canScare = false;
    public float scareAnimationTime = 0.5f;
    public float scareCooldown = 2f;
    public bool canLaugh = true;
    public float beforeLaughTime = 0.45f;
    public float laughTime = 4f;

    private void Start () {
        fadeBlackScreenScr = GameObject.Find("HUD_BlackScreen").GetComponent<FadeBlackScreen>();
        anim = GetComponent<Animator>();
		player = GameObject.Find("Player").transform;
    }

    private void OnEnable() {
        PlayerStateDown.onStateDown += Laugh;
        PlayerHitted.onPlayerHitted += Laugh;
    }

    private void OnDisable() {
        PlayerStateDown.onStateDown -= Laugh;
        PlayerHitted.onPlayerHitted -= Laugh;
    }

    private void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			if(socialState == SocialState.idle){
				socialState = SocialState.looking;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			socialState = SocialState.idle;
		}
	}
	
	private void Update () {
        CheckScareState();
		CheckLookState();
		AssignAnimations();
	}

    private void CheckScareState() {
        float xPos = transform.position.x;
        float xPosPlayer = player.position.x;
        xDistance = xPosPlayer - xPos;

        if (canScare) {
            if (socialState != SocialState.laughing) {
                if (xDistance >= -0.7f && xDistance <= 0.7f) {
                    if (canStartCoroutine) {
                        StartCoroutine(ScareLogic());
                    }
                }
            }
        }
    }

    private void Laugh() {
        if (canLaugh) {
            StartCoroutine(LaughCoroutine());
        }
    }

    private IEnumerator LaughCoroutine() {
        float a = beforeLaughTime * 0.5f;
        float b = beforeLaughTime * 3f;
        float beforeLaughTimeFinal = Random.Range(a, b);

        yield return new WaitForSeconds(beforeLaughTimeFinal);

        socialState = SocialState.laughing;
        
        float a2 = laughTime * 0.75f;
        float b2 = laughTime * 1.25f;
        float laughTimeFinal = Random.Range(a2, b2);
        var main = ps_laugh.main;
        main.duration = laughTimeFinal - 0.5f;
        ps_laugh.Play();
        //Wwise stukje
        AkSoundEngine.PostEvent("Social_Laughing", gameObject);

        yield return new WaitForSeconds(laughTimeFinal);
        
        socialState = SocialState.idle;
    }

    private IEnumerator ScareLogic() {
        canStartCoroutine = false;
        socialState = SocialState.scare;
        yield return new WaitForSeconds(0.1f);
        Scare();
        yield return new WaitForSeconds(0.1f);
        fadeBlackScreenScr.StartCoroutine(fadeBlackScreenScr.FadeOut(0.1f));
        yield return new WaitForSeconds(scareAnimationTime - 0.1f - 0.1f);
        socialState = SocialState.idle;
        yield return new WaitForSeconds(scareCooldown);
        canStartCoroutine = true;
    }

    private void Scare() {
        ScreenShake.instance.ShakeFixed(0.2f, 0.1f);
        fadeBlackScreenScr.StartCoroutine(fadeBlackScreenScr.FadeIn(0.1f));
        ps_scare1.Play();
        ps_scare2.Play();
        triggerObj.SetActive(true);
        //Wwise stukje
        AkSoundEngine.PostEvent("Social_Scream", gameObject);

        StartCoroutine(TriggerCooldown());
    }

    private IEnumerator TriggerCooldown() {
        yield return new WaitForSeconds(0.5f);
        triggerObj.SetActive(false);
    }

	private void CheckLookState(){
		float xPos = transform.position.x;
		float xPosPlayer = player.position.x;
		xDistance = xPosPlayer - xPos;

		if(socialState == SocialState.looking){
			if(xDistance < -2f){
				lookState = LookState.leftFar;
			}
			else if(xDistance < -0.7f && xDistance >= -2f){
				lookState = LookState.leftNear;
			}
			else if(xDistance >= -0.7f && xDistance <= 0.7f){
				lookState = LookState.middle;
			}
			else if(xDistance > 0.7f && xDistance <= 2f){
				lookState = LookState.rightNear;
			}
			else if(xDistance > 2f){
				lookState = LookState.rightFar;
			}
		}
	}

	private void AssignAnimations(){
		if(socialState == SocialState.idle){
			anim.SetBool("idle", true);
		}
		else{
			anim.SetBool("idle", false);
		}

        if (socialState == SocialState.scare) {
            anim.SetBool("isScare", true);
        }
        else{
            anim.SetBool("isScare", false);
        }

        if(socialState == SocialState.laughing) {
            anim.SetBool("isLaughing", true);
        }
        else {
            anim.SetBool("isLaughing", false);
        }

        if (socialState == SocialState.looking) {
            if (lookState == LookState.leftFar) {
                anim.SetInteger("lookDirection", -2);
            }
            else if (lookState == LookState.leftNear) {
                anim.SetInteger("lookDirection", -1);
            }
            else if (lookState == LookState.middle) {
                anim.SetInteger("lookDirection", 0);
            }
            else if (lookState == LookState.rightNear) {
                anim.SetInteger("lookDirection", 1);
            }
            else if (lookState == LookState.rightFar) {
                anim.SetInteger("lookDirection", 2);
            }
        }
	}
}
