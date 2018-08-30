using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimator : MonoBehaviour {

    private Animator anim;

	private void Start () {
        anim = GetComponent<Animator>();

        float randomTime = Random.Range(0, 1f);
        Invoke("StartAnimation", randomTime);
    }

    private void StartAnimation(){
        if (!anim.enabled){
            anim.enabled = true;
        }
    }
}
