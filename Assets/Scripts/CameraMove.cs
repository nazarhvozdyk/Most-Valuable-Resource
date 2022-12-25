using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform _targetTransform;

    [SerializeField]
    float _lerpRate = 8f;

    private void OnEnable()
    {
        if (_targetTransform == null)
            _targetTransform = Player.Instance.transform;
    }

    private void Start()
    {
        transform.position = _targetTransform.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            _targetTransform.position,
            Time.deltaTime * _lerpRate
        );
    }
}
