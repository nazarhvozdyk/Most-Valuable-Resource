using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get => _instance;
    }

    [SerializeField]
    private ScreenFadeAnimations _screenFadeAnimations;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        OnLevelLoaded();
    }

    public void OnLevelEnded()
    {
        _screenFadeAnimations.StartFadeOutAnimation();
    }

    public void OnLevelLoaded()
    {
        _screenFadeAnimations.StartFadeInAnimation();
    }
}
