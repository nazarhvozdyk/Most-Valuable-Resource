using System;
using System.Collections.Generic;
using UnityEngine;

public class FuelStorage : ResourcesStorage
{
    [SerializeField]
    private Factory _factory;
    public Resource[] ResourcesToStore
    {
        get => _factory.FuelResources;
    }
    private Dictionary<Type, int> _eachTypeAmount = new Dictionary<Type, int>();
    private int _maxAmountOfEachResource;

    private void OnEnable()
    {
        _maxAmountOfEachResource = _capacity / ResourcesToStore.Length;
    }

    private void Start()
    {
        for (int i = 0; i < ResourcesToStore.Length; i++)
            _eachTypeAmount.Add(ResourcesToStore[i].GetType(), 0);
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
            bool isResourceFound = false;

            for (int j = 0; j < _resources.Count; j++)
            {
                // skip the iteration if the resource was found
                if (isResourceFound)
                    continue;

                if (resourceTypes[i] == _resources[j].GetType())
                {
                    resourcesToTake[i] = _resources[j];
                    resourcesToTakeCount++;
                    isResourceFound = true;
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

    public override Type GetTypeOfNeededResource()
    {
        int smallestValue = _maxAmountOfEachResource;
        Type smallestTypeAmount = null;

        foreach (var item in _eachTypeAmount.Keys)
        {
            if (_eachTypeAmount[item] < smallestValue)
            {
                smallestValue = _eachTypeAmount[item];
                smallestTypeAmount = item;
            }
        }

        return smallestTypeAmount;
    }
}
