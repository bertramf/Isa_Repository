using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBehaviour : MonoBehaviour {

	private Animator anim;
	private bool canAwake = true;
	private float delayTime = 0.2f;

	public ParticleSystem vfx_awake;
	public bool closeBloom;

	void Start () {
		anim = GetComponent<Animator>();
		if(closeBloom){
			anim.SetBool("closeFlower", true);
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			if(canAwake){
				anim.SetBool("closeFlower", false);
				StartCoroutine(AwakeCoroutine());
			}
		}
	}

	private IEnumerator AwakeCoroutine(){
		canAwake = false;
		float randomValue = Random.Range(-0.1f, 0.1f);

		yield return new WaitForSeconds(delayTime + randomValue);

		anim.SetBool("goAwake", true);
		vfx_awake.Play();

        if (closeBloom) {
            //Wwise Stukje
            AkSoundEngine.PostEvent("Flower_Close", gameObject);
        }
        else {
            //Wwise Stukje
            AkSoundEngine.PostEvent("Flower_Open", gameObject);
        }

		yield return null;

		anim.SetBool("goAwake", false);
	}
}
