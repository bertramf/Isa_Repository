using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : MonoBehaviour {

    public bool enableTimeScale;
    public float timeScale;
	
	void Update () {
        if (enableTimeScale)
        {
            Time.timeScale = timeScale;
        }
	}
}
