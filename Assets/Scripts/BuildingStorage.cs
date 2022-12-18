using UnityEngine;

[DefaultExecutionOrder(1)]
public class BuildingStorage : MonoBehaviour
{
    [SerializeField]
    private Transform _pointerPositionTransform;

    [SerializeField]
    private BuildingPointer _buildingPointerPrefab;

    [SerializeField]
    private PriceStorage _priceStorage;
    private BuildingPointer _pointer;

    [SerializeField]
    private Building _buildingToBuildPrefab;
    public Building BuildingToBuild
    {
        get => _buildingToBuildPrefab;
    }

    private void Start()
    {
        _priceStorage.SetUp(_buildingToBuildPrefab.Price);
        _priceStorage.onStorageIsFull += BuildBuilding;
        CreateBuildingPointer();
    }

    private void CreateBuildingPointer()
    {
        Canvas worldCanvas = WorldCanvasReference.Instance.Canvas;

        _pointer = Instantiate(_buildingPointerPrefab, worldCanvas.transform);
        _pointer.transform.position = _pointerPositionTransform.position;
    }

    private void BuildBuilding()
    {
        Building newBuilding = Instantiate(_buildingToBuildPrefab);
        newBuilding.transform.position = transform.position;
        OnBuildBuilded();
    }

    private void OnBuildBuilded()
    {
        gameObject.SetActive(false);
        transform.position = new Vector3(0, -100, 0);
        Destroy(_pointer.gameObject);
    }
}
