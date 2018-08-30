using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerOld_v1 : MonoBehaviour {

	private Camera cameraComponent;
	private Vector3 bottomLeftBound;
	private Vector3 topRightBound;

	public Transform playerTransform;
	public BoxCollider2D colliderBounds;

	public Vector2 margin;
	public Vector2 smoothing;
	public bool isFollowingPlayer;

	void Start () {
		cameraComponent = GetComponent<Camera>();
		playerTransform = GameObject.Find("Player").transform;

		isFollowingPlayer = true;
		bottomLeftBound = colliderBounds.bounds.min;
		topRightBound = colliderBounds.bounds.max;
	}
	
	void FixedUpdate () {
		float x = transform.position.x;
		float y = transform.position.y;

		if(isFollowingPlayer){
			if(Mathf.Abs(x - playerTransform.position.x) > margin.x){
				x = Mathf.Lerp(x, playerTransform.position.x, smoothing.x * Time.deltaTime);
			}
			if(Mathf.Abs(y - playerTransform.position.y) > margin.y){
				x = Mathf.Lerp(y, playerTransform.position.y, smoothing.y * Time.deltaTime);
			}
		}

		float cameraHalfWidth = cameraComponent.orthographicSize * ((float)Screen.width / Screen.height);
		float cameraHalfLength = cameraComponent.orthographicSize;

		x = Mathf.Clamp(x, bottomLeftBound.x + cameraHalfWidth, topRightBound.x + cameraHalfWidth);
		y = Mathf.Clamp(y, bottomLeftBound.y + cameraHalfLength, topRightBound.y + cameraHalfLength);

		transform.position = new Vector3(x, y, transform.position.z);
	}

	
	private void OnDrawGizmosSelected(){
		//Camera Margin Box
		float playerX = playerTransform.position.x;
		float playerY = playerTransform.position.y;
		Vector3 gizmoCenter = new Vector3(playerX, playerY, 0);
        Vector3 gizmoSize = new Vector3(margin.x, margin.y, 0);

        Gizmos.color = new Color(1, 0, 1, 0.2f);
        Gizmos.DrawCube(gizmoCenter, gizmoSize);
    }
}
