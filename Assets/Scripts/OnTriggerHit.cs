using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerHit : MonoBehaviour {

    public DeathCause deathCause;
    private GameObject player;
	private PlayerBase playerBase;
	private PlayerHitted playerHittedScr;

	private void Start(){
		player = GameObject.Find("Player");
		playerBase = player.GetComponent<PlayerBase>();
		playerHittedScr = player.GetComponent<PlayerHitted>();
	}

	private void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			if (playerHittedScr.isHitted == false && playerBase.isInvulnerable == false) {
				playerHittedScr.GetHitted (deathCause);
			}
		}
	}

}
