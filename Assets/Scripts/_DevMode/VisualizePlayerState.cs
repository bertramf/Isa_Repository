using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizePlayerState : MonoBehaviour {

	private PlayerState playerState;

	public Text playerStateText;

	private void Start () {
		playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
	}
	
	private void Update () {
		VisualizeState();
	}

	private void VisualizeState(){
		if(playerState.state == CourageState.scared){
			playerStateText.text = "PlayerState = Scared";
		}
		else if(playerState.state == CourageState.courage){
			playerStateText.text = "PlayerState = Courage";
		}
	}
}
