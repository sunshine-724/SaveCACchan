using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Title,
    TutorialStage,
    MainStage,
    GameClear_Tutorial,
    GameClear_MainStage,
    GameOver_TutorialStage,
    GameOver_MainStage,
}

public class ChangeScene : MonoBehaviour
{
    [SerializeField] Scenes thisScene;

    public void MoveTitle()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.Title));
    }

    public void MoveTutorialStage()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.TutorialStage));
    }

    public void MoveMainStage()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.MainStage));
    }

    public void MoveGameClear_Tutorial()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.GameClear_Tutorial));
    }

    public void MoveGameClear_MainStage()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.GameClear_MainStage));
    }

    public void MoveGameOver_Tutorial()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.GameOver_TutorialStage));
    }

    public void MoveGameOver_MainStage()
    {
        StartCoroutine(LoadNextSceneAsync(thisScene, Scenes.GameOver_MainStage));
    }

    public Scenes ReadCurrntScene()
    {
        return this.thisScene;
    }

    private IEnumerator LoadNextSceneAsync(Scenes currentScene, Scenes nextScene)
    {
        string currentSceneStr = currentScene.ToString();
        string nextSceneStr = nextScene.ToString();
        // 前のロードシーンをアンロード
        SceneManager.UnloadSceneAsync(currentSceneStr);

        // 次のシーンを非同期で読み込み
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneStr);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
