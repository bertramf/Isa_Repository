using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {

	public CanvasGroup blackScreen;
	public CanvasGroup textGroup1;
	public CanvasGroup textGroup2;
	public CanvasGroup textGroup3;
	public CanvasGroup textGroup4;

	public float fadeTime = 0.3f;
	public float idleTime = 1.5f;
    public float readTime = 3f;

	private IEnumerator Start () {
        float textTime = fadeTime * 2 + idleTime + readTime;

		yield return new WaitForSeconds(2f);
		StartCoroutine(FadeTextInAndOut(textGroup1, readTime));
		yield return new WaitForSeconds(textTime);
		StartCoroutine(FadeTextInAndOut(textGroup2, readTime));
		yield return new WaitForSeconds(textTime);
		StartCoroutine(FadeTextInAndOut(textGroup3, readTime));
		yield return new WaitForSeconds(textTime);
		StartCoroutine(FadeTextInAndOut(textGroup4, readTime));
		yield return new WaitForSeconds(textTime);

		SceneManager.LoadSceneAsync("SceneStart", LoadSceneMode.Additive);
        yield return new WaitUntil(() => SceneManager.GetSceneByName("SceneStart").isLoaded);
		SceneManager.UnloadSceneAsync("IntroScene");
	}

	private IEnumerator FadeTextInAndOut(CanvasGroup textGroup, float readTime){
		float t = 0f;
		while(t < 1){
			t += Time.deltaTime / fadeTime;
			textGroup.alpha = t;
			yield return null;
		}

		yield return new WaitForSeconds(readTime);

		while(t > 0){
			t -= Time.deltaTime / fadeTime;
			textGroup.alpha = t;
			yield return null;
		}
	}

	private IEnumerator FadeBlackScreenIn(float fadeTime){
		float t = 0f;
		while(t < 1f){
			t += Time.deltaTime / fadeTime;
			blackScreen.alpha = t;
			yield return null;
		}
	}

	private IEnumerator FadeBlackScreenOut(float fadeTime){
		float t = 1f;
		while(t > 0){
			t -= Time.deltaTime / fadeTime;
			blackScreen.alpha = t;
			yield return null;
		}
	}
}
