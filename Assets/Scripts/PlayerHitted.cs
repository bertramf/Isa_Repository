using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DeathCause { spikes, gordeldier, falling }

public class PlayerHitted : MonoBehaviour {

    public delegate void PlayerHittedDelegate();
    public static event PlayerHittedDelegate onPlayerHitted;

    public DeathCause deathCause;

    private PlayerBase playerBase;
    private PlayerStats playerStats;
	private FadeBlackScreen fadeBlackScreenScr;
    private Animator anim;
    [HideInInspector()]
    public bool isHitted;

    public ParticleSystem ps_hitted;

	private void Start(){
		playerBase = GetComponent<PlayerBase> ();
        playerStats = GetComponent<PlayerStats>();
        fadeBlackScreenScr = GameObject.Find("HUD_BlackScreen").GetComponent<FadeBlackScreen> ();
        anim = transform.Find("PlayerVisuals").GetComponent<Animator>();
    }

	public void GetHitted(DeathCause deathCause){
        if(onPlayerHitted != null) {
            onPlayerHitted();
        }

        if (deathCause == DeathCause.falling){
            isHitted = true;
            FallingVisuals();
            StartCoroutine(DieLogicFalling());
        }
        else if (deathCause == DeathCause.spikes) {
            isHitted = true;
            SpikeVisuals();
            playerBase.FreezePlayer();
            StartCoroutine(DieLogicStandard());
        }
        else if (deathCause == DeathCause.gordeldier) {
            isHitted = true;
            GordeldierVisuals();
            playerBase.FreezePlayer();
            StartCoroutine(DieLogicStandard());
        }
    }

    private void FallingVisuals(){
        //Wwise stukje - MOET NOG VERANDERD WORDEN!!!!!!!!!!!!!!!!!!!!!!!!!!
        AkSoundEngine.PostEvent("Dead_falling", gameObject);
    }

    private void SpikeVisuals(){
        //Wwise stukje - MOET NOG VERANDERD WORDEN!!!!!!!!!!!!!!!!!!!!!!!!!!
        AkSoundEngine.PostEvent("Dead_Spike", gameObject);

        anim.SetTrigger("isHitted");
        ps_hitted.Play();
        ScreenShake.instance.ShakeFixed(0.5f, 0.1f);
    }
    
    private void GordeldierVisuals(){
        //Wwise stukje
        AkSoundEngine.PostEvent("Dead_Gordeldier", gameObject);

        anim.SetBool("isHitted", true);
        ps_hitted.Play();
        ScreenShake.instance.ShakeFixed(0.5f, 0.1f);
    }

    private IEnumerator DieLogicFalling(){
        fadeBlackScreenScr.StartCoroutine(fadeBlackScreenScr.FadeIn(playerStats.blackScreenFadeInTime));
        yield return new WaitForSeconds(playerStats.blackScreenFadeInTime);
        SceneManager.LoadSceneAsync("SceneStart", LoadSceneMode.Additive);
    }

    private IEnumerator DieLogicStandard(){
        float restTime = playerStats.getHittedTime - playerStats.blackScreenFadeInTime;
        yield return new WaitForSeconds(restTime);
        fadeBlackScreenScr.StartCoroutine(fadeBlackScreenScr.FadeIn(playerStats.blackScreenFadeInTime));
        yield return new WaitForSeconds (playerStats.blackScreenFadeInTime);
		SceneManager.LoadSceneAsync("SceneStart", LoadSceneMode.Additive);
	}

}
