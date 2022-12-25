using UnityEngine;

[DefaultExecutionOrder(0)]
public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private bool _invert = false;

    private void OnEnable()
    {
        float xValue = 40;

        if (_invert)
            xValue = -40;

        transform.eulerAngles = new Vector3(xValue, 0, 0);
    }
}
