using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMusicStates : MonoBehaviour {

    private MusicAudio musicAudioScr;

    [Header("StartEvent, Start, Middle, End")]
    public string musicState;

    private void Start() {
        musicAudioScr = transform.parent.GetComponent<MusicAudio>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            musicAudioScr.PlayMusicState(musicState);
            if(musicState == "StartEvent") {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnDrawGizmos() {
        float xScale = transform.localScale.x;
        float yScale = transform.localScale.y;
        Vector3 gizmoSize = new Vector3(xScale, yScale, 1);

        Gizmos.color = new Color(1, 1, 0, 0.35f);
        Gizmos.DrawWireCube(transform.position, gizmoSize);
    }

}
