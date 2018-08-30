using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWindRtcp : MonoBehaviour {

    private WindAudio windAudioScr;

    public float windRTCP;
    public float windKracht;

    private void Start() {
        windAudioScr = transform.parent.GetComponent<WindAudio>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player") {
            windAudioScr.ChangeRTCP(windRTCP, windKracht);
        }
    }

    private void OnDrawGizmos() {
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
        Vector3 gizmoSize = new Vector3(xScale, yScale, 1);

        Gizmos.color = new Color(1, 0, 0, 0.35f);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }

}
