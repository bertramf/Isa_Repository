using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLogic : MonoBehaviour {

    public CanvasGroup blackScreenGroup;

    private void Update() {
        if (Input.GetButtonDown("A")) {
            StartGame();
        }

        if (Input.GetButtonDown("B")) {
            QuitGame();
        }
    }

    private void StartGame() {
        StartCoroutine(FadeBlackScreenIn());
        StartCoroutine(StartGameLogic());
    }

    private void QuitGame() {
        Application.Quit();    
    }

    private IEnumerator StartGameLogic() {
        yield return new WaitForSeconds(2f);

        //Load de nieuwe introScene
        SceneManager.LoadSceneAsync(("IntroScene"), LoadSceneMode.Additive);

        yield return new WaitUntil(() => SceneManager.GetSceneByName("IntroScene").isLoaded);

        //Unload de menuScene
        if (SceneManager.GetSceneByName("Menu").isLoaded) {
            SceneManager.UnloadSceneAsync("Menu");
        }
    }

    private IEnumerator FadeBlackScreenIn() {
        float t = 0f;

        while (t < 1) {
            t += Time.deltaTime / 2f;
            blackScreenGroup.alpha = t;
            yield return null;
        }
    }

}
