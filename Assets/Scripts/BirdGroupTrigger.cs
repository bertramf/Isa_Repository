using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SimpleDelegate();

public class BirdGroupTrigger : MonoBehaviour {

    public event SimpleDelegate onPlayerClose;
    
	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
            if (onPlayerClose != null) {
                onPlayerClose();
                Destroy(this);
            }
		}
	}

	private void OnDrawGizmos(){
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
        Vector3 gizmoSize = new Vector3(xScale, yScale, 1);

        Gizmos.color = new Color(1f, 0, 1f, 0.5f);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }

}
