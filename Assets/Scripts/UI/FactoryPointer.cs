using System.Collections;
using UnityEngine;

public class FactoryPointer : MonoBehaviour
{
    [SerializeField]
    private PointerImagesData _pointerImagesData;

    [SerializeField]
    private Factory _factory;

    [SerializeField]
    private PointerController _pointerControllerPrefab;
    private PointerController _pointerController;

    [SerializeField]
    private float _animationTime = 1f;
    private bool _isActive;

    private void Start()
    {
        // create pointer
        Transform pointerParent = PointersCanvasReference.Instance.Canvas.transform;
        _pointerController = Instantiate(_pointerControllerPrefab, pointerParent);

        _factory.onFactoryStartProducing += OnFactoryStarted;
        _factory.onFactoryStopped += OnFactoryStopped;
    }

    private void OnFactoryStopped(Factory.StopReason stopReason)
    {
        StopAllCoroutines();

        if (stopReason == Factory.StopReason.NoFuel)
        {
            _pointerController.SetSprite(_pointerImagesData.NoFuelSprite);
            StartCoroutine(Show());
        }
        else if (stopReason == Factory.StopReason.NoSpace)
        {
            _pointerController.SetSprite(_pointerImagesData.NoSpaceSprite);
            StartCoroutine(Show());
        }

        _isActive = true;
    }

    private void OnFactoryStarted()
    {
        if (_isActive)
        {
            _isActive = false;
            StopAllCoroutines();
            StartCoroutine(Hide());
        }
    }

    private void LateUpdate()
    {
        Camera camera = MainCameraReference.Instance.Camera;
        Vector3 playerPosition = Player.Instance.transform.position;

        // Ordering: [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

        int planesAmount = 4;

        Vector3 playerToPointerDirection = transform.position - playerPosition;
        Ray ray = new Ray(playerPosition, playerToPointerDirection);

        float minDistance = Mathf.Infinity;
        int closestPlaneIndex = 0;

        for (int i = 0; i < planesAmount; i++)
        {
            if (planes[i].Raycast(ray, out float distance))
            {
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPlaneIndex = i;
                }
            }
        }

        if (minDistance < playerToPointerDirection.magnitude)
            _pointerController.SetRotation(GetPointerRotation(closestPlaneIndex));
        else
            _pointerController.SetRotation(PointerController.Rotation.Center);

        minDistance = Mathf.Clamp(minDistance, 0, playerToPointerDirection.magnitude);

        Vector3 worldPosition = ray.GetPoint(minDistance);
        Vector3 positionInUI = camera.WorldToScreenPoint(worldPosition);
        _pointerController.transform.position = positionInUI;
    }

    // Ordering: [0] = Left, [1] = Right, [2] = Down, [3] = Up, [4] = Near, [5] = Far
    private PointerController.Rotation GetPointerRotation(int planeIndex)
    {
        if (planeIndex == 0)
            return PointerController.Rotation.Left;
        else if (planeIndex == 1)
            return PointerController.Rotation.Right;
        else if (planeIndex == 2)
            return PointerController.Rotation.Down;
        else if (planeIndex == 3)
            return PointerController.Rotation.Up;

        return PointerController.Rotation.Center;
    }

    private void OnDestroy()
    {
        _factory.onFactoryStartProducing -= OnFactoryStarted;
        _factory.onFactoryStopped -= OnFactoryStopped;
    }

    private IEnumerator Show()
    {
        _pointerController.transform.localScale = Vector3.zero;

        for (float t = 0; t < _animationTime; t += Time.deltaTime)
        {
            float process = t / _animationTime;
            _pointerController.transform.localScale = Vector3.one * process;
            yield return null;
        }

        _pointerController.transform.localScale = Vector3.one;
    }

    private IEnumerator Hide()
    {
        float animationTime = 1f;
        _pointerController.transform.localScale = Vector3.one;

        for (float t = 0; t < animationTime; t += Time.deltaTime)
        {
            float process = t / animationTime;
            _pointerController.transform.localScale = Vector3.one - Vector3.one * process;
            yield return null;
        }

        _pointerController.transform.localScale = Vector3.zero;
    }
}
