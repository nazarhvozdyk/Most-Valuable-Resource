using UnityEngine;

public class WorldCanvasReference : MonoBehaviour
{
    public static WorldCanvasReference Instance
    {
        get => _instance;
    }
    private static WorldCanvasReference _instance;

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
