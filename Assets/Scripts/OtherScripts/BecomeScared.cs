using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BecomeScared : MonoBehaviour {

	private PlayerState playerState;

	private void Start(){
		playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			playerState.BecomeScared();
		}
	}

}
