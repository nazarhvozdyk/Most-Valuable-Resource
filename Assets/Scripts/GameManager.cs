using UnityEngine;

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
        Debug.Log("level task complited");
        Player.Instance.SitInSpaceShip();
    }

    public void OnPlayerSitOnShip()
    {
        Debug.Log("level finished");
    }
}
