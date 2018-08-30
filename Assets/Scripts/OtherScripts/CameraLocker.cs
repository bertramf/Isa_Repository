using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocker : MonoBehaviour {

	private PlayerStats playerStats;
	private SetLockPosition setLockPositionScr;
	
	public bool destroyLockedCamAfterEnter;
	
	private void Start () {
		playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
		setLockPositionScr = transform.GetComponentInParent<SetLockPosition>();
	}
	
	private void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			playerStats.lockCam = true;
			setLockPositionScr.SetPosition(transform.position);
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			playerStats.lockCam = false;
		}
		if(destroyLockedCamAfterEnter){
			Destroy(this.gameObject);
		}
	}

    private void OnDrawGizmos(){
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
        Vector3 gizmoSizeOffset = new Vector3(xScale - 0.1f, yScale - 0.1f, 1);

        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawWireCube(transform.position, gizmoSizeOffset);
    }
}
