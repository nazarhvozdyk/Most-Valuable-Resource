using UnityEngine;

public class PointersCanvasReference : MonoBehaviour
{
    private static PointersCanvasReference _instance;
    public static PointersCanvasReference Instance
    {
        get => _instance;
    }

    // canvas that has all pointers
    [SerializeField]
    private Canvas _pointersCanvas;
    public Canvas Canvas
    {
        get => _pointersCanvas;
    }

    private void Awake() 
    { 
        _instance = this;
    }
}
