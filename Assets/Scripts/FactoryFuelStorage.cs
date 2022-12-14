using System;
using System.Collections.Generic;
using UnityEngine;

public class FactoryFuelStorage : FuelStorage
{
    [SerializeField]
    private Factory _factory;
    public Resource[] ResourcesToStore
    {
        get => _factory.FuelResources;
    }
    protected Dictionary<Type, int> _eachTypeAmount = new Dictionary<Type, int>();
    private int _maxAmountOfEachResource;

    private void OnEnable()
    {
        _maxAmountOfEachResource = _capacity / ResourcesToStore.Length;
    }

    private void Start()
    {
        for (int i = 0; i < ResourcesToStore.Length; i++)
            _eachTypeAmount.Add(ResourcesToStore[i].GetType(), 0);

        ResourceAmount[] resourcesAmount = new ResourceAmount[ResourcesToStore.Length];
        for (int i = 0; i < resourcesAmount.Length; i++)
        {
            ResourceAmount resourceAmount = new ResourceAmount()
            {
                Amount = 1,
                resource = ResourcesToStore[i]
            };

            resourcesAmount[i] = resourceAmount;
        }
        GetComponent<ResourceHint>().SetUp(resourcesAmount);
    }

    public override bool TryToAddResource(Resource resource)
    {
        bool isResourceRequire = false;

        Type resourceType = resource.GetType();

        for (int i = 0; i < _factory.FuelResources.Length; i++)
            if (_factory.FuelResources[i].GetType() == resourceType)
                isResourceRequire = true;

        if (!isResourceRequire)
            return false;

        if (_eachTypeAmount[resourceType] == _maxAmountOfEachResource)
            return false;

        bool isResourceAdded = base.TryToAddResource(resource);

        if (isResourceAdded)
            _eachTypeAmount[resourceType]++;

        return isResourceAdded;
    }

    public override bool TryToGiveResourceByType(Type resourceType, out Resource resource)
    {
        bool isResourceGiven = base.TryToGiveResourceByType(resourceType, out resource);

        if (isResourceGiven)
            _eachTypeAmount[resourceType]--;

        return isResourceGiven;
    }

    // returns true if resources were removed
    public bool TryToRemoveResourcesByTypes(Type[] resourceTypes)
    {
        Resource[] resourcesToTake = new Resource[resourceTypes.Length];
        int resourcesToTakeCount = 0;

        // searching the resource by type
        for (int i = 0; i < resourceTypes.Length; i++)
        {
            for (int j = 0; j < _resources.Count; j++)
            {
                if (resourceTypes[i] == _resources[j].GetType())
                {
                    resourcesToTake[i] = _resources[j];
                    resourcesToTakeCount++;
                    // end lower loop to avoid useless iterations
                    j = _resources.Count;
                }
            }
        }

        // if found resources is less then needed return false
        if (resourcesToTakeCount != resourceTypes.Length)
            return false;

        // destroy all found resources
        // we can destroy resources if only we find all of them
        for (int i = 0; i < resourcesToTake.Length; i++)
        {
            Type typeOfTakenResource = resourcesToTake[i].GetType();
            _eachTypeAmount[typeOfTakenResource]--;
            _resources.Remove(resourcesToTake[i]);
            Destroy(resourcesToTake[i].gameObject);
        }

        return true;
    }

    public override Type[] GetTypesOfNeededResources()
    {
        List<Type> types = new List<Type>();

        foreach (var item in _eachTypeAmount.Keys)
            if (_eachTypeAmount[item] < _maxAmountOfEachResource)
                types.Add(item);

        return types.ToArray();
    }
}
