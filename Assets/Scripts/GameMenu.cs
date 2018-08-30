using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour {

    public CanvasGroup blackScreenGroup;
    public CanvasGroup menuGroup;
    public bool isInMenu;
    public float currentAlpha;

    private bool isFadingIn = false;
    private bool isFadingOut = false;

    private void Update() {
        currentAlpha = menuGroup.alpha;

        if (Input.GetButtonDown("Back")) {
            if (!isInMenu) {
                isInMenu = true;
                StartCoroutine(FadeIn(currentAlpha));
            }
            else {
                isInMenu = false;
                StartCoroutine(FadeOut(currentAlpha));
            }
        }

        if (isInMenu) {
            if (Input.GetButtonDown("A")) {
                StartCoroutine(FadeOut(currentAlpha));
                StartCoroutine(FadeBlackScreenIn());
                StartCoroutine(GoToMenuScene());
            }
        }
    }

    private IEnumerator GoToMenuScene() {

        yield return new WaitForSeconds(2f);

        //Load de nieuwe menuScene
        SceneManager.LoadSceneAsync(("Menu"), LoadSceneMode.Additive);

        yield return new WaitUntil(() => SceneManager.GetSceneByName("Menu").isLoaded);

        //Unload de persistentScene
        if (SceneManager.GetSceneByName("PersistentManager").isLoaded) {
            SceneManager.UnloadSceneAsync("PersistentManager");
        }

        //Unload de gameplayScene
        if (SceneManager.GetSceneByName("T6_Gameplay").isLoaded) {
            SceneManager.UnloadSceneAsync("T6_Gameplay");
        }

        //Unload de triggerScene
        if (SceneManager.GetSceneByName("T6_Triggers").isLoaded) {
            SceneManager.UnloadSceneAsync("T6_Triggers");
        }

        //Unload de setdressingScene
        if (SceneManager.GetSceneByName("T6_Setdressing").isLoaded) {
            SceneManager.UnloadSceneAsync("T6_Setdressing");
        }
    }

    private IEnumerator FadeBlackScreenIn() {
        float t = 0f;

        while(t < 1) {
            t += Time.deltaTime / 2f;
            blackScreenGroup.alpha = t;
            yield return null;
        }
    }

    private IEnumerator FadeIn(float currentAlpha) {

        isFadingIn = true;
        isFadingOut = false;
        float t = currentAlpha;

        while (t < 1 && !isFadingOut && isFadingIn) {
            t += Time.deltaTime / 0.5f;
            menuGroup.alpha = t;
            yield return null;
        }
        isFadingIn = false;
    }

    private IEnumerator FadeOut(float currentAlpha) {

        isFadingOut = true;
        isFadingIn = false;
        float t = currentAlpha;

        while (t > 0 && isFadingOut && !isFadingIn) {
            t -= Time.deltaTime / 0.5f;
            menuGroup.alpha = t;
            yield return null;
        }
        isFadingOut = false;
    }



}
