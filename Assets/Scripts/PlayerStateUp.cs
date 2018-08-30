using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUp : MonoBehaviour {

	private PlayerState playerState;
	private PlayerBase playerBase;
	private PowerupController powerupControllerScr;
    private Animator anim;

	public ParticleSystem psStateChange;
	public float powerupTime = 0.333f;

	private void Start(){
		playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
		playerBase = GetComponent<PlayerBase>();
        anim = transform.Find("PlayerVisuals").GetComponent<Animator>();
    }

	private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Powerup"){
			powerupControllerScr = other.gameObject.GetComponent<PowerupController>();
			powerupControllerScr.PowerupVisuals(powerupTime);
			PowerupCheck();
        }
    }

	private void PowerupCheck(){
		if(playerState.state == CourageState.scared){
			StartCoroutine(StateChangeCooldown());
        }
	}

	private IEnumerator StateChangeCooldown(){
        ScreenShake.instance.ShakeFixed (powerupTime, 0.05f);
        playerBase.FreezePlayer();
        anim.SetTrigger("stateUp");
        yield return new WaitForSeconds(powerupTime);
		playerState.BecomeCourageous();
        playerBase.UnFreezePlayer();
		psStateChange.Play();
		ScreenShake.instance.ShakeFixed (1.75f, 0.025f);
    }

}
