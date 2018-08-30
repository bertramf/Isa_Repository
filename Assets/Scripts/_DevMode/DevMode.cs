using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevMode : MonoBehaviour {

	private PlayerStats playerStats;
    private CanvasGroup mainDebugGroup;
	private CanvasGroup playerStateGroup;
    private TeleportPointsBase teleportScr;
    private Camera mainCam;
	private bool devMode;
	private int number = 0;
    private float currentTimeScale = 1f;
	
	[Header("Can DevMode")]
	public bool canDevMode;

	[Header("Matrix DevMode Publics")]
	public bool showMotionParticles;
	public bool showPlayerState;
	public bool canTeleport;
    public bool canZoom;
    public bool enableTimeScale;

	[Header("Reference Publics")]
	public ParticleSystem psDebugJumpCurve;

    private void Start () {
		playerStats = GetComponent<PlayerStats> ();
        mainDebugGroup = GameObject.Find("MainDebugGroup").GetComponent<CanvasGroup>();
        playerStateGroup = GameObject.Find("PlayerStateGroup").GetComponent<CanvasGroup>();
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (enableTimeScale){
            Time.timeScale = 1f;
        }
		
		Invoke("OneSecondAfterStart", 1.0f);
    }

	private void OneSecondAfterStart(){
		teleportScr = GameObject.Find("Base_TeleportPoints").GetComponent<TeleportPointsBase>();
	}

	private void Update () {
		ActivateDevMode ();
		DeveloperMode();
	}

	private void ActivateDevMode(){
		if(canDevMode){
			if (Input.GetButtonDown ("DevMode")) {
				if (devMode == false) {
					devMode = true;
				} 
				else {
					devMode = false;
				}
			}
		}
		else{
			if(devMode == true){
				devMode = false;
			}
		}
	}

	private void DeveloperMode(){
        ShowDebugCanvasgroup();
		ShowParticleMotionInAir();
		VisualizePlayerState();
		TeleportToCheckpoint ();
        ActivateTimeScaler();
        ZoomCamera();
	}

    private void ShowDebugCanvasgroup(){
        if (devMode){
            mainDebugGroup.alpha = 1f;
        }
        else{
            mainDebugGroup.alpha = 0f;
        }
    }

    private void ShowParticleMotionInAir(){
		if(devMode && showMotionParticles){
			psDebugJumpCurve.transform.gameObject.SetActive (true);
			var main = psDebugJumpCurve.main;
			bool isOn = false;
			if(playerStats.grounded == true && isOn == false){
				isOn = true;
				psDebugJumpCurve.Play();
				main.loop = true;
			}
			else{
				isOn = false;
				main.loop = false;
			}
		}
		else{
			psDebugJumpCurve.transform.gameObject.SetActive (false);
		}
	}

	private void VisualizePlayerState(){
		if(devMode && showPlayerState){
			if(devMode && playerStateGroup.alpha == 0){
                playerStateGroup.alpha = 1;
			}
		}
		else{
            playerStateGroup.alpha = 0;
		}
	}

	private void TeleportToCheckpoint(){
		if(devMode && canTeleport){
            Vector2 currentCheckpoint;

			if(Input.GetKeyDown(KeyCode.Alpha0) && number < (teleportScr.checkpointsPosition.Length - 1)){
				number ++;
				currentCheckpoint = teleportScr.checkpointsPosition[number];
				transform.position = new Vector3 (currentCheckpoint.x, currentCheckpoint.y, transform.position.z);
			}
			else if(Input.GetKeyDown(KeyCode.Alpha9) && number > 0){
				number --;
                currentCheckpoint = teleportScr.checkpointsPosition[number];
                transform.position = new Vector3 (currentCheckpoint.x, currentCheckpoint.y, transform.position.z);
			}
		}
	}

    private void ActivateTimeScaler(){
        if (devMode && enableTimeScale){
            if (Input.GetKeyDown(KeyCode.LeftBracket)){
                if (Time.timeScale > 0.3f){
                    Time.timeScale -= 0.2f;
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket)){
                if (Time.timeScale < 1.9f){
                    Time.timeScale = Time.timeScale + 0.2f;
                }
            }
        }
        else{
            currentTimeScale = 1f;
        }

        currentTimeScale = Time.timeScale;
    }

    private void ZoomCamera(){
        if(devMode && canZoom){
            if (mainCam.orthographicSize > 0.5f){
                if (Input.GetKeyDown(KeyCode.Equals)){
                    mainCam.orthographicSize -= 0.5f;
                }
            }
            if (mainCam.orthographicSize < 5f){
                if (Input.GetKeyDown(KeyCode.Minus)){
                    mainCam.orthographicSize += 0.5f;
                }
            }
        }
    }

}
