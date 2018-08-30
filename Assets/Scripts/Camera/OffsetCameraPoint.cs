using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetCameraPoint : MonoBehaviour {

	private Transform playerTransform;
	private PlayerStats playerStats;

	public bool startCoroutine = true;
	public bool canMoveDown = false;
	public float yVelocityDebug;
	public float ySpeed = 0f;
	public float ySpeedFactor = -5f;
	public float lerpTime = 0.5f;

	void Start () {
		playerTransform = GameObject.Find ("Player").transform;
		playerStats = playerTransform.GetComponent<PlayerStats> ();
	}

	void Update () {
		yVelocityDebug = playerStats.yVelocity;
		if (playerStats.yVelocity < 0f) {
			if (canMoveDown) {
				if (startCoroutine) {
					StartCoroutine (LerpSpeed ());
				}
				MoveDown ();
			} 
			else {
				transform.position = transform.position;
			}
		}
		else if(playerStats.yVelocity > 0f){
			startCoroutine = true;
			canMoveDown = true;
			transform.position = playerTransform.position;
		}
		else{
			startCoroutine = true;
			canMoveDown = true;
			transform.position = playerTransform.position;
		}

		if(playerStats.grounded){
			canMoveDown = false;
		}

	}

	private IEnumerator LerpSpeed(){
		startCoroutine = false;
		float t = 0f;
		while (t < 1) {
			t += Time.deltaTime / lerpTime;
			ySpeed = t;
			yield return null;
		}
		float t2 = 1f;
		while (t2 > 0) {
			t2 -= Time.deltaTime / lerpTime;
			ySpeed = t2;
			yield return null;
		}
	}

	private void MoveDown(){
		Vector3 movementDirection = new Vector3(0, ySpeed * ySpeedFactor * Time.deltaTime, 0);
		transform.Translate(movementDirection);
	}

}
