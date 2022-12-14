using UnityEngine;

public class BuildingStoragesCanvasReference : MonoBehaviour
{
    public static BuildingStoragesCanvasReference Instance
    {
        get => _instance;
    }
    private static BuildingStoragesCanvasReference _instance;

    [SerializeField]
    private Canvas _canvas;
    public Canvas Canvas
    {
        get => _canvas;
    }

    private void Awake()
    {
        _instance = this;
    }
}
