using UnityEngine;

public class MainCameraReference : MonoBehaviour
{
    public static MainCameraReference Instance
    {
        get => _instance;
    }
    private static MainCameraReference _instance;

    [SerializeField]
    private Camera _mainCamera;
    public Camera Camera
    {
        get => _mainCamera;
    }

    private void Awake() 
    { 
        _instance = this;
    }
}
