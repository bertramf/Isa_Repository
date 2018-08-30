using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutOnStart : MonoBehaviour {

    public CanvasGroup blackScreenGroup;

	private void Start () {
        StartCoroutine(FadeOut());
	}

    private IEnumerator FadeOut() {
        float t = 1f;

        while (t > 0) {
            t -= Time.deltaTime / 2f;
            blackScreenGroup.alpha = t;
            yield return null;
        }
    }

}
