using UnityEngine;

public class GridInfo : MonoBehaviour
{
    private static GridInfo _instance;
    public static GridInfo Instance
    {
        get => _instance;
    }

    private void Awake() => _instance = this;

    [SerializeField]
    private StoragesData _storagesData;

    public float GridOffset
    {
        get => _storagesData._gridOffset;
    }
}
