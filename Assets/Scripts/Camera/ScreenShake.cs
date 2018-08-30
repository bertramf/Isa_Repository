using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour{

    public static ScreenShake instance;
    private float shakeTimer;
    private float shakeAmount;
    private bool isShaking;
    private bool oneShake;

    private void Awake(){
        instance = this;
    }

    public void ShakeFixed(float shakeDuration, float shakePwr){
        if(isShaking == false){
            shakeTimer = shakeDuration;
            shakeAmount = shakePwr;
        }
    }

    public void Shake(float shakePwr){
        shakeAmount = shakePwr;
        oneShake = true;
    }

    private Vector2 GetShaker(){
        if (shakeTimer >= 0f){
            isShaking = true;
            shakeTimer -= Time.deltaTime;

            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
            return shakePos;
        }
        else if (oneShake){
            isShaking = false;
            oneShake = false;
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;
            return shakePos;
        }
        else{
            isShaking = false;
        }

        return Vector2.zero;
    }

    private void LateUpdate(){
        Vector2 shakeStuff = GetShaker();
        Vector3 newCamPosition = new Vector3(transform.position.x + shakeStuff.x, transform.position.y + shakeStuff.y, transform.position.z);
        transform.position = newCamPosition;
    }

}
