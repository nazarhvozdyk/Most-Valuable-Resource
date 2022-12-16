using UnityEngine;

public class BuildingResourcesTrackerCreator : MonoBehaviour
{
    private static BuildingResourcesTrackerCreator _instance;
    public static BuildingResourcesTrackerCreator Instance
    {
        get => _instance;
    }

    [SerializeField]
    private ResourceUI _resourceUIPrefab;

    [SerializeField]
    private Canvas _worldCanvas;

    public ResourceUI CreateResourceUI()
    {
        Transform parent = _worldCanvas.transform;
        ResourceUI newResourceUI = Instantiate(_resourceUIPrefab, parent);
        return null;
    }
}
