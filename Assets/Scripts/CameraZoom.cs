using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

	public Camera normalcamera;
	public Camera bgCamera1;
	public Camera bgCamera2;
	public Camera bgCamera3;
	public Camera bgCamera4;
	public Camera bgCamera5;

	private float normalSizeNormalCam;
	private float normalSizeCam1;
	private float normalSizeCam2;
	private float normalSizeCam3;
	private float normalSizeCam4;
	private float normalSizeCam5;

	private void Awake () {
		normalSizeNormalCam = normalcamera.orthographicSize;
		normalSizeCam1 = bgCamera1.orthographicSize;
		normalSizeCam2 = bgCamera2.orthographicSize;
		normalSizeCam3 = bgCamera3.orthographicSize;
		normalSizeCam4 = bgCamera4.orthographicSize;
		normalSizeCam5 = bgCamera5.orthographicSize;
	}

	public void StartZoomCameraIn(float startSize, float endSize, float zoomTime){
		StartCoroutine(ZoomCameraIn(startSize, endSize, zoomTime));
	}

	private IEnumerator ZoomCameraIn(float startSize, float endSize, float zoomTime){
		float difference = startSize - endSize;

		float t = 1f;
		while(t > 0f){
			t -= Time.deltaTime / zoomTime;
			normalcamera.orthographicSize = normalSizeNormalCam + (difference * t);
			bgCamera1.orthographicSize = normalSizeCam1 + (difference * t);
			bgCamera2.orthographicSize = normalSizeCam2 + (difference * t);
			bgCamera3.orthographicSize = normalSizeCam3 + (difference * t);
			bgCamera4.orthographicSize = normalSizeCam4 + (difference * t);
			bgCamera5.orthographicSize = normalSizeCam5 + (difference * t);
			yield return null;
		}

	}
}
