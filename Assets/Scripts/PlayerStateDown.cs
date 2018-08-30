using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDown : MonoBehaviour {

    public delegate void StateDownDelegate();
    public static event StateDownDelegate onStateDown;

    private PlayerBase playerBase;
    private PlayerState playerState;
    private Animator anim;

    public ParticleSystem ps_stateDown;
    public bool stateDown;
    public float stateDownAnimTime = 1f;

    private void Start () {
        playerBase = GetComponent<PlayerBase>();
        playerState = GameObject.Find("Manager_PlayerState").GetComponent<PlayerState>();
        anim = transform.Find("PlayerVisuals").GetComponent<Animator>();
    }

    public void StateDown() {
        if (playerState.state == CourageState.courage) {
            stateDown = true;
            StartCoroutine(StateDownLogic());
            if(onStateDown != null) {
                onStateDown();
            }
        }
    }

    private IEnumerator StateDownLogic() {
        ScreenShake.instance.ShakeFixed(stateDownAnimTime, 0.05f);
        playerBase.FreezePlayer();
        ps_stateDown.Play();
        anim.SetBool("stateDown", true);
        yield return new WaitForSeconds(stateDownAnimTime);
        anim.SetBool("stateDown", false);
        stateDown = false;
        playerState.BecomeScared();
        playerBase.UnFreezePlayer();
    }

}
