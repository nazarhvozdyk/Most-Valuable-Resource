using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private static readonly int RunKey = Animator.StringToHash("Run");

    [SerializeField]
    private Rigidbody _rigidbody;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _speed = 7;

    [SerializeField]
    private float _rotationSpeed = 4;

    private Vector2 _moveDirection;

    public void SetMoveDirection(Vector2 direction)
    {
        _moveDirection = direction;
        _animator.SetBool(RunKey, direction.magnitude > 0.01f);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_moveDirection.x, 0f, _moveDirection.y) * _speed;

        // rotation
        if (_rigidbody.velocity.magnitude < 0.1f)
            return;

        Vector3 rotationVector = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);

        Quaternion rotation = Quaternion.LookRotation(rotationVector);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            rotation,
            Time.deltaTime * _rotationSpeed
        );
    }
}
