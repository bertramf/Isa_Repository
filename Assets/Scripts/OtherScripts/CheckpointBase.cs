using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBase : MonoBehaviour {

	private PlayerState playerState;

	[Header("Important Start Value")]
	public Vector2 startPosition;

	[Header("Public Reference")]
	public Vector2 respawnPoint;
	[HideInInspector()]
	public bool courageCheckpoint;

	private void Awake(){
		playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
		respawnPoint = new Vector2(startPosition.x, startPosition.y);

		if(playerState.state == CourageState.scared){
			courageCheckpoint = false;
		}
		else if(playerState.state == CourageState.courage){
			courageCheckpoint = true;
		}
	}

	public void SetRespawnPoint(float x, float y){
		respawnPoint = new Vector2(x, y);
	}
}
