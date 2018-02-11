using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour{

    public static ScreenShake instance;
    private float shakeTimer;
    private float shakeAmount;
    private bool isShaking;

    private void Awake(){
        instance = this;
    }

    public void ShakeFixed(float shakeDuration, float shakePwr){
        shakeTimer = shakeDuration;
        shakeAmount = shakePwr;
    }

    public void Shake(float shakePwr){
        shakeAmount = shakePwr;
        isShaking = true;
    }

    private Vector2 GetShaker(){
        if (shakeTimer >= 0f){
            shakeTimer -= Time.deltaTime;

            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
            return shakePos;
        }
        else if (isShaking){
            isShaking = false;
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
            return shakePos;
        }

        return Vector2.zero;
    }

    private void LateUpdate(){
        Vector2 shakeStuff = GetShaker();
        transform.position = new Vector3(shakeStuff.x, shakeStuff.y + 2, -1);
    }

}
