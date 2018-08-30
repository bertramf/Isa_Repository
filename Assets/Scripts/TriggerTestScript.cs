using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTestScript : MonoBehaviour {

    private Animator anim;
    public GameObject[] arrayTest;
    
	private void Start () {
        anim = GetComponent<Animator>();
	}
	
	private void Update () {
        if (Input.GetKeyDown(KeyCode.Space)){
            anim.SetTrigger("goChange");
        }
	}
}
