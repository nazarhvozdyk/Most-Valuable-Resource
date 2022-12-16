using System;
using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(1)]
public class BuildingStorage : FuelStorage
{
    [SerializeField]
    private float _priceTagsOffset = 2;

    [SerializeField]
    private ResourceSpritesData _resourceSpritesData;

    [SerializeField]
    private ResourceUI _resourceUIPrefab;

    [SerializeField]
    private BuildingPointer _buildingPointerPrefab;

    [SerializeField]
    private Transform _pointerPositionTransform;

    [SerializeField]
    private Transform _priceTagStartPositionTransform;

    [SerializeField]
    private Building _buildingToBuildPrefab;
    private Dictionary<Type, int> _buildingPrice;
    private Dictionary<Type, int> _eachTypeAmount = new Dictionary<Type, int>();
    private BuildingPointer _pointer;
    public Building BuildingToBuild
    {
        get => _buildingToBuildPrefab;
    }
    private Dictionary<Type, ResourceUI> _priceTags;

    protected override void Awake()
    {
        _buildingPrice = _buildingToBuildPrefab.Price;
        _capacity = 0;
        _priceTags = new Dictionary<Type, ResourceUI>(_buildingPrice.Count);

        foreach (var item in _buildingPrice)
        {
            _eachTypeAmount.Add(item.Key, 0);
            _capacity += item.Value;
        }

        base.Awake();
    }

    private void Start()
    {
        Canvas canvas = BuildingStoragesCanvasReference.Instance.Canvas;
        _pointer = Instantiate(_buildingPointerPrefab, canvas.transform);
        _pointer.SetUp(_pointerPositionTransform.position);
        CreatePriceTag();
    }

    public override Type[] GetTypesOfNeededResources()
    {
        List<Type> types = new List<Type>();

        foreach (var item in _buildingPrice)
        {
            int amountOfNeededResources = item.Value;
            int amountOfCurrentResources = _eachTypeAmount[item.Key];

            if (amountOfCurrentResources < amountOfNeededResources)
                types.Add(item.Key);
        }

        return types.ToArray();
    }

    public override bool TryToAddResource(Resource resource)
    {
        Type resourceType = resource.GetType();

        foreach (var item in _eachTypeAmount)
        {
            // return false if we have enough of this type of resources
            // or return we dont need this resource type

            if (resourceType == item.Key.GetType())
            {
                int amountOfNeededResources = _buildingPrice[item.Key];
                int amountOfCurrentResources = item.Value;

                if (amountOfNeededResources == amountOfCurrentResources)
                    return false;
            }
        }

        bool isResourceAdded = base.TryToAddResource(resource);

        if (isResourceAdded)
        {
            // increase current resources number
            _eachTypeAmount[resourceType]++;

            // decrease price tag cost
            _priceTags[resourceType].Add(-1);

            bool isEnoungh = CheckIfEnoughToBuild();

            if (isEnoungh)
                BuildBuilding();
        }

        return isResourceAdded;
    }

    private bool CheckIfEnoughToBuild()
    {
        foreach (var item in _buildingPrice)
            if (_eachTypeAmount[item.Key] != item.Value)
                return false;

        return true;
    }

    private void BuildBuilding()
    {
        for (int i = 0; i < _resources.Count; i++)
            Destroy(_resources[i].gameObject);

        Building newBuilding = Instantiate(_buildingToBuildPrefab);
        newBuilding.transform.position = transform.position;
        OnBuildBuilded();
    }

    private void OnBuildBuilded()
    {
        // make it imposible to interact with player by changing position
        transform.position = new Vector3(0, -100, 0);

        gameObject.SetActive(false);
        Destroy(_pointer.gameObject);

        foreach (var item in _priceTags.Values)
            Destroy(item.gameObject);

        _priceTags = null;
        _buildingPrice = null;
    }

    private void CreatePriceTag()
    {
        Canvas worldCanvas = WorldCanvasReference.Instance.Canvas;
        int priceTagsAmounts = 0;

        foreach (var item in _buildingPrice)
        {
            Type typeOfCurrentResource = item.Key;
            int amountOfResources = item.Value;

            Vector3 nextPosition =
                _priceTagStartPositionTransform.position
                + _priceTagStartPositionTransform.up * (_priceTagsOffset * priceTagsAmounts);

            ResourceUI newResourceUI = Instantiate(_resourceUIPrefab, worldCanvas.transform);
            newResourceUI.transform.position = nextPosition;
            newResourceUI.gameObject.AddComponent<LookAtCamera>();

            Sprite sprite = _resourceSpritesData.GetSpriteByResourceType(typeOfCurrentResource);

            newResourceUI.SetSprite(sprite);
            newResourceUI.Add(amountOfResources);
            _priceTags.Add(typeOfCurrentResource, newResourceUI);
            priceTagsAmounts++;
        }
    }
}
