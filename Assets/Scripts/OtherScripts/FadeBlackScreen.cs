using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBlackScreen : MonoBehaviour {

	[HideInInspector()]
	public CanvasGroup blackScreenGroup;

	private void Awake(){
		blackScreenGroup = transform.Find ("BlackScreenGroup").GetComponent<CanvasGroup> ();
	}

	public IEnumerator FadeIn(float fadeTime){
		float t = 0f;
		while(t < 1){
			t += Time.deltaTime / fadeTime;
			blackScreenGroup.alpha = t;
			yield return null;
		}
	}

	public IEnumerator FadeOut(float fadeTime){
		float t = 1f;
		while(t > 0){
			t -= Time.deltaTime / fadeTime;
			blackScreenGroup.alpha = t;
			yield return null;
		}
	}
}
