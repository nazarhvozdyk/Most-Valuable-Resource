using System;
using UnityEngine;
using System.Collections.Generic;

public class BuildingStorage : FuelStorage
{
    [SerializeField]
    private CreateBuildingPointer _buildingPointerPrefab;

    [SerializeField]
    private Transform _pointerPositionTransform;

    [SerializeField]
    private Building _buildingToBuildPrefab;
    private Dictionary<Type, int> _buildingPrice;
    private Dictionary<Type, int> _eachTypeAmount = new Dictionary<Type, int>();
    private CreateBuildingPointer _pointer;

    protected override void Awake()
    {
        _buildingPrice = _buildingToBuildPrefab.Price;
        _capacity = 0;

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
            _eachTypeAmount[resourceType]++;

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
        gameObject.SetActive(false);
        Destroy(_pointer.gameObject);
    }
}
