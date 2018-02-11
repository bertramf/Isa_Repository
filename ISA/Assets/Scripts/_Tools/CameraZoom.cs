using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    private Camera mainCam;

	void Start () {
        mainCam = GetComponent<Camera>();
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            mainCam.orthographicSize -= 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            mainCam.orthographicSize += 0.5f;
        }
    }
}
