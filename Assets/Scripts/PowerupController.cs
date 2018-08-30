using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour {

    private Animator anim;

    public ParticleSystem vfx_Idle;
    public ParticleSystem vfx_Consume;

    private void Start(){
        anim = GetComponent<Animator>();
    }

    public void PowerupVisuals(float powerupTime){
        vfx_Idle.gameObject.SetActive(false);
        vfx_Consume.Play();
        anim.SetBool("goConsume", true);
        Invoke("DestroyPowerup", powerupTime);
        //Wwise stukje
        AkSoundEngine.PostEvent("Pickup", gameObject);
    }

    private void DestroyPowerup(){
        Destroy(this.gameObject);
    }

}
