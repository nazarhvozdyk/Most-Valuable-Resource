using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    public void ShowMessage(string message)
    {
        _text.text = message;
        StartCoroutine(ShowAnimation());
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private IEnumerator ShowAnimation()
    {
        float animationTime = 0.5f;

        for (float t = 0; t < animationTime; t += Time.deltaTime)
        {
            // number from 0 to 1
            float process = t / animationTime;

            Vector3 newScale = Vector3.Lerp(Vector3.zero, Vector3.one, process);
            transform.localScale = newScale;

            yield return null;
        }

        transform.localScale = Vector3.one;
        Invoke(nameof(Destroy), 3f);
    }
}
