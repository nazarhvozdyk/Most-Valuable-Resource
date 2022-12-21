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

    public void AnimateLevelEnding(ScreenFadeAnimations.AnimationEndedCallBack callBack)
    {
        _screenFadeAnimations.StartFadeOutAnimation(callBack);
    }

    public void OnLevelLoaded()
    {
        _screenFadeAnimations.StartFadeInAnimation();
    }
}
