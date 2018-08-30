using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantShaking : MonoBehaviour {

	public Vector2 randomVector;
	public bool goCoroutine = true;
	private float xMovement;
	private float yMovement;
	private float yValue = 2.45f;

	public float intervalTime = 0.1f;
	public float randomPointAmount = 0.1f;
	public float xSpeed;
	public float ySpeed;

	private void Awake () {
		xMovement = transform.position.x;
		yMovement = transform.position.y;
	}
	
	private void Update () {
		if(goCoroutine){
			StartCoroutine(NewRandomPoint());
		}
		LerpToPoint();
		Movement();
	}

	private IEnumerator NewRandomPoint(){
		goCoroutine = false;
		randomVector = Random.insideUnitCircle * randomPointAmount;

		float randomTime = Random.Range(intervalTime, intervalTime * 1.5f);
		yield return new WaitForSeconds(randomTime);

		goCoroutine = true;
	}

	private void LerpToPoint(){
		float xA = transform.position.x;
		float xB = randomVector.x;
		float yA = transform.position.y;
		float yB = randomVector.y;

		xMovement = Mathf.Lerp (xA, xB, xSpeed * Time.deltaTime);
		yMovement = Mathf.Lerp (xA, xB, xSpeed * Time.deltaTime);
	}

	private void Movement(){
		transform.position = new Vector3(xMovement, yMovement + yValue, transform.position.z);
	}
}
