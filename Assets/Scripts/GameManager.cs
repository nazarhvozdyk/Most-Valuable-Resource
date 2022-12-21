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
    private int _currentLevel = 1;

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
        if (SceneManager.sceneCount == _currentLevel)
            LoadGameOverScene();
        else
            SceneManager.LoadScene(++_currentLevel);
    }

    private void LoadGameOverScene()
    {
        _currentLevel = 0;
        int indexOfGameOverScene = 0;
        SceneManager.LoadScene(indexOfGameOverScene);
    }

    public void LoadFirstLevel()
    {
        _currentLevel = 1;
        SceneManager.LoadScene(_currentLevel);
    }
}
