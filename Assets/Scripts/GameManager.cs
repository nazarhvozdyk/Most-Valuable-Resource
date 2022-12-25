using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();

            return _instance;
        }
    }

    public void OnLevelTaskComplited()
    {
        Player.Instance.SitInSpaceShip();
    }

    public void FinishTheLevel()
    {
        ScreenFadeAnimations.AnimationEndedCallBack callBack = LoadNextLevel;
        UIManager.Instance.AnimateLevelEnding(callBack);
    }

    private void LoadNextLevel()
    {
        int indexOfCurrentScene = SceneManager.GetActiveScene().buildIndex;
        if (SceneManager.sceneCountInBuildSettings == indexOfCurrentScene)
            LoadGameOverScene();
        else
            SceneManager.LoadScene(++indexOfCurrentScene);
    }

    private void LoadGameOverScene()
    {
        int indexOfGameOverScene = 0;
        SceneManager.LoadScene(indexOfGameOverScene);
    }

    public void LoadFirstLevel()
    {
        int indexOfFirstLevel = 1;
        SceneManager.LoadScene(indexOfFirstLevel);
    }
}
