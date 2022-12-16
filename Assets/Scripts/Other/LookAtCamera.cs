using UnityEngine;

[DefaultExecutionOrder(0)]
public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    private bool _invert = false;

    private void Start()
    {
        float xValue = 40;

        if (_invert)
            xValue = -40;

        transform.localEulerAngles = new Vector3(xValue, 0, 0);
    }
}
