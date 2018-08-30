using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour {

    public GameObject camera;
    
	private void Awake () {
        transform.position = camera.transform.position;
    }
	
    private void Update () {
        transform.position = camera.transform.position;
    }
}
