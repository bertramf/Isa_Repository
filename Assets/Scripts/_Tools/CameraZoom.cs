using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    private Camera mainCam;

	void Start () {
        mainCam = GetComponent<Camera>();
	}

	void Update () {
		if (mainCam.orthographicSize > 0.5f) {
			if (Input.GetKeyDown (KeyCode.Equals)) {
				mainCam.orthographicSize -= 0.5f;
			}
		}
		if(mainCam.orthographicSize < 5f){
	        if (Input.GetKeyDown(KeyCode.Minus)){
	            mainCam.orthographicSize += 0.5f;
	        }
		}
    }
}
