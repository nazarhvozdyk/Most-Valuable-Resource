using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeAnimations : MonoBehaviour
{
    [SerializeField]
    private float _animationTime = 1;

    [SerializeField]
    private Image _animatedImage;

    public void StartFadeInAnimation()
    {
        StartCoroutine(FadeIn());
    }

    public void StartFadeOutAnimation()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        _animatedImage.enabled = true;

        for (float t = 0; t < _animationTime; t += Time.deltaTime)
        {
            float process = t / _animationTime;
            Color color = _animatedImage.color;
            color.a = process;
            _animatedImage.color = color;
            yield return null;
        }

        Color finalColor = _animatedImage.color;
        finalColor.a = 1f;
        _animatedImage.color = finalColor;
    }

    private IEnumerator FadeIn()
    {
        _animatedImage.enabled = true;

        for (float t = 0; t < _animationTime; t += Time.deltaTime)
        {
            float process = t / _animationTime;
            float alphaValue = 1 - process;
            Color color = _animatedImage.color;
            color.a = alphaValue;
            _animatedImage.color = color;
            yield return null;
        }

        Color finalColor = _animatedImage.color;
        finalColor.a = 0f;
        _animatedImage.color = finalColor;
        _animatedImage.enabled = false;
    }
}
