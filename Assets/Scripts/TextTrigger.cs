using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextTrigger : MonoBehaviour {

    private PlayerStats playerStats;
    private PlayerState playerState;
    private FadeBlackScreen fadeBlackScreenScr;
    private Animator anim;
    private bool hasParticle;
	private bool isInTrigger;
    private bool isFadingIn;
    private bool isFadingOut;
    private float currentAlpha;

	[Header("Important Publics")]
	public ParticleSystem p_UI_circle;
	public CanvasGroup textCanvasGroup;
	public bool destroyTextAfterSeen;
    public bool scaredStateOnlyText;
    public bool endTrigger;

	[Header("Timing Values")]
	public float fadeInTime = 1f;
	public float fadeOutTime = 1.5f;

	private void Start () {
        if (scaredStateOnlyText) {
            playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
        }

        if (endTrigger) {
            playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
            fadeBlackScreenScr = GameObject.Find("HUD_BlackScreen").GetComponent<FadeBlackScreen>();
            anim = GameObject.Find("PlayerVisuals").GetComponent<Animator>();
        }

        if (p_UI_circle == null){
            hasParticle = false;
        }
        else{
            hasParticle = true;
        }
	}

	private void Update(){
        currentAlpha = textCanvasGroup.alpha;
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
            if (scaredStateOnlyText) {
                if (playerState.state == CourageState.scared) {
                    if (currentAlpha < 1){
                        if (hasParticle){
                            p_UI_circle.Play();
                        }
                        StartCoroutine(FadeIn(currentAlpha));
                    }
                }
            }
            else if (endTrigger) {
                playerStats.movementSpeed = 0f;
                playerStats.canControl = false;
                if (currentAlpha < 1) {
                    StartCoroutine(FadeIn(currentAlpha));
                }
                StartCoroutine(EndCoroutine());
                anim.SetBool("endIdle", true);
            }
            else{
                if (currentAlpha < 1){
                    if (hasParticle){
                        p_UI_circle.Play();
                    }
                    StartCoroutine(FadeIn(currentAlpha));
                }
            }
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			if(currentAlpha > 0){
				if(hasParticle){
					p_UI_circle.Stop();
				}
				StartCoroutine(FadeOut(currentAlpha));
			}
		}
	}

    private IEnumerator EndCoroutine() {
        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeOut(currentAlpha));
        yield return new WaitForSeconds(4f);
        fadeBlackScreenScr.StartCoroutine(fadeBlackScreenScr.FadeIn(playerStats.blackScreenFadeInTime));
        StartCoroutine(GoToMenuScene());
    }

    private IEnumerator GoToMenuScene() {

        yield return new WaitForSeconds(2f);

        //Load de nieuwe menuScene
        SceneManager.LoadSceneAsync(("Menu"), LoadSceneMode.Additive);

        yield return new WaitUntil(() => SceneManager.GetSceneByName("Menu").isLoaded);

        //Unload de persistentScene
        if (SceneManager.GetSceneByName("PersistentManager").isLoaded) {
            SceneManager.UnloadSceneAsync("PersistentManager");
        }

        //Unload de gameplayScene
        if (SceneManager.GetSceneByName("T6_Gameplay").isLoaded) {
            SceneManager.UnloadSceneAsync("T6_Gameplay");
        }

        //Unload de triggerScene
        if (SceneManager.GetSceneByName("T6_Triggers").isLoaded) {
            SceneManager.UnloadSceneAsync("T6_Triggers");
        }

        //Unload de setdressingScene
        if (SceneManager.GetSceneByName("T6_Setdressing").isLoaded) {
            SceneManager.UnloadSceneAsync("T6_Setdressing");
        }
    }

    private IEnumerator FadeIn(float currentAlpha){

        isFadingIn = true;
        isFadingOut = false;
        float fadeTime = fadeInTime * (1 - currentAlpha);
		float t = currentAlpha;

		while(t < 1 && !isFadingOut && isFadingIn){
            t += Time.deltaTime / fadeTime;
			textCanvasGroup.alpha = t;
			yield return null;
		}
        isFadingIn = false;
    }

	private IEnumerator FadeOut(float currentAlpha){

        isFadingOut = true;
        isFadingIn = false;
        float fadeTime = fadeOutTime * currentAlpha;
		float t = currentAlpha;
        

        while (t > 0 && !isFadingIn && isFadingOut){
            t -= Time.deltaTime / fadeTime;
			textCanvasGroup.alpha = t;
			yield return null;
		}
        isFadingOut = false;

        if (destroyTextAfterSeen){
			Destroy(this.gameObject);
		}
	}

    private void OnDrawGizmos(){
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
        Vector3 gizmoSize = new Vector3(xScale, yScale, 1);

        Gizmos.color = new Color(0, 0, 1, 0.35f);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }

}
