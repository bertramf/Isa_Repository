using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CourageState{
	scared,
	courage
}

public class PlayerState : MonoBehaviour {

	private Animator anim;

	[Header("Important Start Value")]
	public CourageState state = CourageState.scared;

	public void BecomeScared(){
        state = CourageState.scared;
        if (anim == null) {
            anim = GameObject.Find("PlayerVisuals").GetComponent<Animator>();
        }
        anim.SetBool("courageous", false);
        //Wwise stukje
        AkSoundEngine.SetState("PlayerState", "Scared");
        AkSoundEngine.SetSwitch("PlayerState", "Scared", gameObject);
    }

    public void BecomeCourageous() {
        state = CourageState.courage;
        if (anim == null) {
            anim = GameObject.Find("PlayerVisuals").GetComponent<Animator>();
        }
        anim.SetBool("courageous", true);
        //Wwise stukje
        AkSoundEngine.SetState("PlayerState", "Courage");
        AkSoundEngine.SetSwitch("PlayerState", "Courage", gameObject);
    }

}


