using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	private CheckpointBase checkpointBaseScr;

	public Transform checkpointTransform;
	public bool courageCheckpoint;

	private void Start(){
		checkpointBaseScr = GameObject.Find("Manager_Checkpoints").GetComponent<CheckpointBase>();
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			checkpointBaseScr.SetRespawnPoint(checkpointTransform.position.x, checkpointTransform.position.y);
			checkpointBaseScr.courageCheckpoint = courageCheckpoint;
		}
	}

	private void OnDrawGizmos(){
		float xPos = transform.parent.transform.position.x;
		float yPos = transform.parent.transform.position.y + 1;
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
		Vector3 gizmoPosition = new Vector3(xPos, yPos, 1);
        Vector3 gizmoSize = new Vector3(xScale, yScale, 1);

        Gizmos.color = new Color(0, 1, 0, 0.35f);
        Gizmos.DrawWireCube(gizmoPosition, gizmoSize);
    }
}
