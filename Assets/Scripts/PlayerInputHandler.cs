using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerMove _playerMove;

    [SerializeField]
    private float _joystickRadious = 100f;

    private Vector3 _startMousePositon;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _startMousePositon = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
            _playerMove.SetMoveDirection(Vector2.zero);

        if (!Input.GetMouseButton(0))
            return;

        // creating a move vector
        Vector3 delta = (Input.mousePosition - _startMousePositon) / _joystickRadious;
        float deltaMagnitude = delta.magnitude;

        // move vector magnitude cannot be more than 1 and less that 0 so we clamp it

        float clampedMagnitude = Mathf.Clamp(deltaMagnitude, 0f, 1f);
        Vector3 deltaNormalized = delta.normalized * clampedMagnitude;

        Vector2 newMoveDirection = new Vector2(deltaNormalized.x, deltaNormalized.y) * -1;
        _playerMove.SetMoveDirection(newMoveDirection);
    }
}
