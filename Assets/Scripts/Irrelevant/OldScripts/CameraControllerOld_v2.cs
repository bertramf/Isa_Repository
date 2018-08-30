using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerOld_v2 : MonoBehaviour {

	private PlayerStats playerStats;
	private bool followPlayer;
	private float xMovement;
	private float yMovement;
	private float offsetIdle;

	public Transform cameraFollowTransform;

	[Header("X Values")]
	public float xOffsetIdle = 0.6f;
    public float xSmoothingIdle = 3f;
	public float xSmoothingWalk = 3.5f;

	[Header("Y Values")]
	public float yDistanceThreshold = 0.5f;
	public float ySmoothingFast = 4f;
	public float ySmoothingSlow = 8f;

	private void Start () {
		playerStats = GameObject.Find("Player").GetComponent<PlayerStats> ();

		followPlayer = true;
		transform.position = new Vector3(cameraFollowTransform.position.x + playerStats.lookDirection * xOffsetIdle, cameraFollowTransform.position.y, transform.position.z);
	}

	private void FixedUpdate(){
		HorizontalMovement();
		VerticalMovement ();
		Movement ();
	}

	private void HorizontalMovement(){
		float x = transform.position.x;

		if(followPlayer){
			//Walking
			if(playerStats.movementSpeed > 0){
				if(Mathf.Abs(x - cameraFollowTransform.position.x) > 0){
					//Offset voor lopen is nodig om hem ongeveer in het midden te krijgen met een lerp (hoe groter de smoothing, hoe minder offset je ndoig hebt)
					//Deze formule werkt bij elke xSmoothingWalk om hem in het midden te krijgen: ((1/x)*4)
					float offsetWalk = playerStats.lookDirection * ((1/xSmoothingWalk)*4f);
					float b = cameraFollowTransform.position.x + offsetWalk;
					x = Mathf.Lerp(x, b, xSmoothingWalk * Time.deltaTime);
				}
			}
			//Idle
            else{
				offsetIdle = playerStats.lookDirection * xOffsetIdle;
				float b = cameraFollowTransform.position.x + offsetIdle;
				x = Mathf.Lerp (x, b, xSmoothingIdle * Time.deltaTime);
            }
		}

		xMovement = x;
	}

	private void VerticalMovement(){
		float y = transform.position.y;

		if (followPlayer) {

			float distance = Mathf.Abs(y - cameraFollowTransform.position.y);
			float b = cameraFollowTransform.position.y;

			if (distance > yDistanceThreshold) {
				y = Mathf.Lerp (y, b, ySmoothingFast * Time.deltaTime);
			}
			else if (distance > 0.01f && distance <= yDistanceThreshold) {
				y = Mathf.Lerp (y, b, ySmoothingSlow * Time.deltaTime);
			} 
			else {
				y = b;
			}
		}

		yMovement = y;
	}

	private void Movement(){
		transform.position = new Vector3(xMovement, yMovement, transform.position.z);
	}

	private void OnDrawGizmos(){
		float xMarginLeftGizmo = transform.position.x + xOffsetIdle * -1f;
		float xMarginMidGizmo = transform.position.x;
		float xMarginRightGizmo = transform.position.x + xOffsetIdle;

		float yHorizontalMid = transform.position.y;
		float yPlayer = cameraFollowTransform.position.y;

		Vector3 line1PointDown = new Vector3(xMarginLeftGizmo, transform.position.y - 1f, 0);
        Vector3 line1PointUp = new Vector3(xMarginLeftGizmo, transform.position.y + 1f, 0);
		Vector3 line2PointDown = new Vector3(xMarginMidGizmo, transform.position.y - 1.2f, 0);
        Vector3 line2PointUp = new Vector3(xMarginMidGizmo, transform.position.y + 1.2f, 0);
		Vector3 line3PointDown = new Vector3(xMarginRightGizmo, transform.position.y - 1f, 0);
        Vector3 line3PointUp = new Vector3(xMarginRightGizmo, transform.position.y + 1f, 0);
		Vector3 line4PointLeft = new Vector3(transform.position.x - 1.5f, yHorizontalMid, 0);
		Vector3 linePointRight = new Vector3(transform.position.x + 1.5f, yHorizontalMid, 0);
		Vector3 line5PointLeft = new Vector3(cameraFollowTransform.position.x - 0.5f, yPlayer, 0);
		Vector3 line5PointRight = new Vector3(cameraFollowTransform.position.x + 0.5f, yPlayer, 0);

        Gizmos.color = new Color(1, 1, 1, 0.2f);
		Gizmos.DrawLine(line1PointDown, line1PointUp);
		Gizmos.DrawLine(line3PointDown, line3PointUp);

		Gizmos.color = new Color(1, 1, 1, 0.35f);
		Gizmos.DrawLine(line2PointDown, line2PointUp);
		Gizmos.DrawLine(line4PointLeft, linePointRight);

		Gizmos.color = new Color(1, 1, 0, 0.35f);
		Gizmos.DrawLine(line5PointLeft, line5PointRight);
    }
}
