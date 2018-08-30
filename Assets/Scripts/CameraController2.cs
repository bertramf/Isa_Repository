using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour {

    private PlayerStats playerStats;
    private Transform cameraFollowTransform;
    private Camera cameraComponent;
    private float xMovement;
    private float yMovement;
    private float yCamHalfSize;

    public bool canCoroutine = true;
    public float yCamBorder;
    public float ySmoothing;

    [Header("Free Camera Movement Values")]
    public float xOffset = 0.2f;
    public float xSmoothing = 7f;
    public float ySmoothingMax = 7f;
    public float ySmoothing2 = 2.5f;
    
    private void Start () {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        cameraFollowTransform = GameObject.Find("Player").transform;
        cameraComponent = GetComponent<Camera>();
    }

    public void SetCameraStartValues(float x, float y) {
        transform.position = new Vector3(x, y, transform.position.z);
    }

    private void FixedUpdate () {
        yCamHalfSize = cameraComponent.orthographicSize;
        yCamBorder = playerStats.yBorder + yCamHalfSize;

        Movement();
	}

    private void Movement() {
        float offsetWalk = playerStats.lookDirection * xOffset;

        float xA = transform.position.x;
        float yA = transform.position.y;
        float xB = cameraFollowTransform.position.x + offsetWalk;
        float yB = cameraFollowTransform.position.y;

        xMovement = Mathf.Lerp(xA, xB, xSmoothing * Time.deltaTime);

        if (playerStats.hitsCamBorder) {
            canCoroutine = true;
            if (yB > yCamBorder) {
                yMovement = Mathf.Lerp(yA, yB, ySmoothing2 * Time.deltaTime);
            }
            else {
                yMovement = Mathf.Lerp(yA, yCamBorder, ySmoothing2 * Time.deltaTime);
            }
        }
        else {
            if (canCoroutine) {
                StartCoroutine(LerpYSmoothing());
            }
            yMovement = Mathf.Lerp(yA, yB, ySmoothing * Time.deltaTime);
        }
        
        transform.position = new Vector3(xMovement, yMovement, transform.position.z);
    }

    IEnumerator LerpYSmoothing() {
        canCoroutine = false;
        float t = 0f;
        while (t < 1) {
            t += Time.deltaTime / 1f;
            ySmoothing = Mathf.Lerp(ySmoothing2, ySmoothingMax, t);
            yield return null;
        }
    }
}
