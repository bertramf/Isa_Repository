using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerStateDown : MonoBehaviour {

    private PlayerStateDown playerStateDownScr;

	private void Start () {
        playerStateDownScr = GameObject.Find("Player").GetComponent<PlayerStateDown>();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (playerStateDownScr.stateDown == false) {
                playerStateDownScr.StateDown();
            }
        }
    }

}
