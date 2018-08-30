using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Social2Behaviour : MonoBehaviour {

	public enum SocialState{idle, angry}
	private Animator anim;

	public SocialState socialState = SocialState.idle;
	
	private void Start () {
		anim = GetComponent<Animator>();
    }
	
	private void Update () {
		AssignAnimations();
	}

	private void OnTriggerStay2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			if(socialState == SocialState.idle){
				socialState = SocialState.angry;
                //Wwise stukje
                AkSoundEngine.PostEvent("Social_Gordeldier", gameObject);
            }
		}
	}

	private void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
            if (socialState == SocialState.angry) {
                socialState = SocialState.idle;
                //Wwise stukje
                AkSoundEngine.PostEvent("Stop_Social_Gordeldier", gameObject);
            }
        }
	}

	private void AssignAnimations(){
		if(socialState == SocialState.idle){
			anim.SetBool("angry", false);
           
        }
		else if(socialState == SocialState.angry){
			anim.SetBool("angry", true);
        }
	}
}
