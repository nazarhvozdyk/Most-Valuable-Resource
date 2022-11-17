using System.Collections;
using UnityEngine;

public abstract class Resource : MonoBehaviour
{
    private Coroutine _movingProgressCoroutine;

    public void GoTo(Vector3 position, Transform parent)
    {
        if (_movingProgressCoroutine != null)
            StopCoroutine(_movingProgressCoroutine);

        _movingProgressCoroutine = StartCoroutine(GoingProcess(position, parent));
    }

    //  second argument is the parent that we set at the end ot the path
    private IEnumerator GoingProcess(Vector3 endLocalPosition, Transform nextParent)
    {
        float movingTime = 1f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 endPosition = nextParent.TransformPoint(endLocalPosition);

        for (float t = 0; t < movingTime; t += Time.deltaTime)
        {
            float process = t / movingTime;
            endPosition = nextParent.TransformPoint(endLocalPosition);

            transform.rotation = Quaternion.Lerp(startRotation, nextParent.rotation, process);
            transform.position = Vector3.Lerp(startPosition, endPosition, process);

            yield return null;
        }

        transform.SetParent(nextParent);
        transform.localPosition = endLocalPosition;
        transform.rotation = nextParent.rotation;
    }
}
