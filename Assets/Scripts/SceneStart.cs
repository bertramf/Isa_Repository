using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneStart : MonoBehaviour {
    
    public string sceneName;

	private const string GAMEPLAY_NAME = "_Gameplay";
    private const string TRIGGERS_NAME = "_Triggers";
    private const string SETDRESSING_NAME = "_Setdressing";

    private const string sceneCoreName = "SceneCore";

	private void Awake () {
		StartCoroutine(SceneOrderLogic());
    }

	private IEnumerator SceneOrderLogic(){
        string gameSceneName = sceneName + GAMEPLAY_NAME;
        string triggerSceneName = sceneName + TRIGGERS_NAME;
        string setdressingSceneName = sceneName + SETDRESSING_NAME;

        //Load de Persistent Manager als hij nog niet geladen is (vanaf menu bijvoorbeeld)
        if (!SceneManager.GetSceneByName("PersistentManager").isLoaded){
            SceneManager.LoadSceneAsync("PersistentManager", LoadSceneMode.Additive);
        }

        //Unload de oude gameplayScene
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded){
            SceneManager.UnloadSceneAsync(gameSceneName);
        }

        //Unload de oude triggerScene
        if (SceneManager.GetSceneByName(triggerSceneName).isLoaded){
            SceneManager.UnloadSceneAsync(triggerSceneName);
		}

        //Unload de oude setdressingScene
        if (SceneManager.GetSceneByName(setdressingSceneName).isLoaded){
            SceneManager.UnloadSceneAsync(setdressingSceneName);
        }

        //Wanneer gameplayScene niet loaded is gaat hij verder
        yield return new WaitUntil(() => !SceneManager.GetSceneByName(gameSceneName).isLoaded);

        //----------------------------LOADING---------------------------------------------------------------------------------------------

        //Load de nieuwe gameplayScene
        SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);

        //Wanneer gameplayScene loaded is gaat hij verder
        yield return new WaitUntil(() => SceneManager.GetSceneByName(gameSceneName).isLoaded);

        //Load de nieuwe triggerScene en de nieuwe setdressingscene
        SceneManager.LoadSceneAsync(triggerSceneName, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(setdressingSceneName, LoadSceneMode.Additive);

        //Wanneer gameplayScene loaded is gaat hij verder
        yield return new WaitUntil(() => SceneManager.GetSceneByName(gameSceneName).isLoaded);

        //Set gameplayScene active en remove SceneStart
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameSceneName));
        SceneManager.UnloadSceneAsync("SceneStart");
	}
}
