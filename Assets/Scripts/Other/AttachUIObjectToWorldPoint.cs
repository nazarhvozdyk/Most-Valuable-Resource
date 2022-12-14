using UnityEngine;

public class AttachUIObjectToWorldPoint : MonoBehaviour
{
    private Transform _pointTransform;
    private RectTransform _UIObjectRectTransform;
    private Camera _camera;

    private void Start()
    {
        _camera = MainCameraReference.Instance.Camera;
    }

    public void SetUp(RectTransform rectTransform, Transform positionTransform)
    {
        _UIObjectRectTransform = rectTransform;
        _pointTransform = positionTransform;
    }

    private void LateUpdate()
    {
        _UIObjectRectTransform.position = _camera.WorldToScreenPoint(_pointTransform.position);
    }
}
