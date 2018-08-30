using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	private PlayerStats playerStats;
	private SetLockPosition setLockPositionScr;
	private Transform cameraFollowTransform;
    private Camera cameraComponent;
	private bool canLerpBack;
	private bool followPlayer;
	private float xMovement;
	private float yMovement;
    private float yMovement2;
    private float camHalfHeight;

	[Header("Free Camera Movement Values")]
	public float xOffset = 0.4f;
	public float xSmoothing = 5f;
	public float ySmoothing = 6f;
    public float ySmoothing2 = 2f;

	[Header("Locked Camera Values")]
	public float lerpTime = 1f;
	public float xSpeed = 1.5f;
	public float ySpeed = 1.5f;

    [Header("BoxValues")]
    public LayerMask boxCamLayer;
    public Vector2 boxSize;

    [Header("BoxValues Debug")]
    public Collider2D highestHorBorder;
    public bool findsBoxes;
    public float yCam;
    public float yBorder = -1000f;

    private void Start () {
		playerStats = GameObject.Find("Player").GetComponent<PlayerStats> ();
        cameraComponent = GetComponent<Camera>();

        if (GameObject.Find("Triggers_Narratives") != null){
			setLockPositionScr = GameObject.Find("Triggers_Narratives").GetComponent<SetLockPosition>();
		}
		cameraFollowTransform = GameObject.Find("OffsetCameraPoint").transform;

		canLerpBack = true;
		followPlayer = true;
	}

	public void SetCameraStartValues(float x, float y){
		transform.position = new Vector3(x, y, transform.position.z);
	}

	private void Update(){
        CameraBoxValues();
        CameraBoxCast();

        if (GameObject.Find("Triggers_Narratives") != null && setLockPositionScr == null){
			setLockPositionScr = GameObject.Find("Triggers_Narratives").GetComponent<SetLockPosition>();
		}
	}

    private void CameraBoxValues() {
        camHalfHeight = cameraComponent.orthographicSize;
        yCam = transform.position.y - camHalfHeight;
    }

    private void CameraBoxCast() {
        RaycastHit2D[] boxCast = Physics2D.BoxCastAll(transform.position, boxSize, 0f, Vector2.zero, 1000f, boxCamLayer);
        
        findsBoxes = false;

        for (int i = 0; i < boxCast.Length; i++) {
            Debug.Log(boxCast[i].collider.name);
            if (boxCast[i].collider.tag == "Horizontal_Border") {
                if (i == 0) {
                    findsBoxes = true;
                    highestHorBorder = boxCast[i].collider;
                    yBorder = highestHorBorder.bounds.max.y;
                }
                else {
                    if (boxCast[i].transform.position.y > highestHorBorder.transform.position.y) {
                        highestHorBorder = boxCast[i].collider;
                        yBorder = highestHorBorder.bounds.max.y;
                    }
                }
            }
        }
    }

	private void FixedUpdate(){
		if (playerStats.lockCam) {
			LerpToLockedPosition ();
		} 
		else {
			if (canLerpBack == true) {
				StartCoroutine (LerpBackToPlayer ());
			}
			FreeMovement ();
		}
		Movement ();
	}

    //Wordt meerdere frames aangeroepen zolang boolean playerStats.lockCam true is
	private void LerpToLockedPosition(){
		canLerpBack = true;
		float xA = transform.position.x;
		float yA = transform.position.y;
		float xB = setLockPositionScr.posCameraLock.x;
		float yB = setLockPositionScr.posCameraLock.y;

		xMovement = Mathf.Lerp (xA, xB, xSpeed * Time.deltaTime);
		yMovement = Mathf.Lerp (yA, yB, ySpeed * Time.deltaTime);
	}

	//Wordt 1 keer aangeroepen; hier pas ik alleen xSmoothing en ySmoothing aan die weer wordt gebruikt in FreeMovement
	private IEnumerator LerpBackToPlayer(){
		canLerpBack = false;

		float t = 0f;
		float xDifference = xSmoothing - xSpeed;
		float yDifference = ySmoothing - ySpeed;

		while (t < 1){
			t += Time.deltaTime / lerpTime;
			xSmoothing = xSpeed + (t * xDifference);
			ySmoothing = ySpeed + (t * yDifference);
			yield return null;
		}
	}

    //Wordt meerdere frames aangeroepen zolang boolean playerStats.lockCam false is
    private void FreeMovement() {

        if (!followPlayer) {
            xMovement = transform.position.x;
            yMovement = transform.position.y;
        }
        else {
            float offsetWalk = playerStats.lookDirection * ((1 / xSmoothing) * xOffset);

            float xA = transform.position.x;
            float yA = transform.position.y;
            float xB = cameraFollowTransform.position.x + offsetWalk;
            float yB = cameraFollowTransform.position.y;

            xMovement = Mathf.Lerp(xA, xB, xSmoothing * Time.deltaTime);
            yMovement = Mathf.Lerp(yA, yB, ySmoothing * Time.deltaTime);
        }
    }

    private void Movement(){
        float yA = transform.position.y;
        float yB = yBorder + camHalfHeight;

        yMovement2 = Mathf.Lerp(yA, yB, ySmoothing2 * Time.fixedDeltaTime);

        if (cameraFollowTransform.position.y - 3f > yBorder || findsBoxes == false) {
            transform.position = new Vector3(xMovement, yMovement, transform.position.z);
        }
        else {
            transform.position = new Vector3(xMovement, yMovement2, transform.position.z);
        }
	}

	private void OnDrawGizmos(){
		float xMarginMidGizmo = transform.position.x;
		float yHorizontalMid = transform.position.y;
		float yPlayer;
		if(cameraFollowTransform != null){
			yPlayer = cameraFollowTransform.position.y;
		}
		else{
			yPlayer  = 0.95f;
		}

        Vector3 boxSize3D = new Vector3(boxSize.x, boxSize.y, 1);

		Vector3 line1PointDown = new Vector3(xMarginMidGizmo, transform.position.y - 1.2f, 0);
		Vector3 line1PointUp = new Vector3(xMarginMidGizmo, transform.position.y + 1.2f, 0);
		Vector3 line2PointLeft = new Vector3(transform.position.x - 1.5f, yHorizontalMid, 0);
		Vector3 line2PointRight = new Vector3(transform.position.x + 1.5f, yHorizontalMid, 0);
		Vector3 line3PointLeft = new Vector3(transform.position.x - 0.5f, yPlayer, 0);
		Vector3 line3PointRight = new Vector3(transform.position.x + 0.5f, yPlayer, 0);

		Gizmos.color = new Color(1, 1, 1, 0.35f);
		Gizmos.DrawLine(line1PointDown, line1PointUp);
		Gizmos.DrawLine(line2PointLeft, line2PointRight);

		Gizmos.color = new Color(1, 1, 0, 0.35f);
		Gizmos.DrawLine(line3PointLeft, line3PointRight);

        Gizmos.color = new Color(1, 0, 1, 0.5f);
        Gizmos.DrawWireCube(transform.position, boxSize3D);
    }
}