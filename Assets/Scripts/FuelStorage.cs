using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FuelStorage : ResourcesStorage
{
    [SerializeField]
    private Factory _factory;
    public Resource[] ResourcesToStore
    {
        get => _factory.FuelResources;
    }

    public override bool TryToAddResource(Resource resource)
    {
        for (int i = 0; i < _factory.FuelResources.Length; i++)
            if (_factory.FuelResources[i].GetType() == resource.GetType())
                return base.TryToAddResource(resource);

        return false;
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
                if(isResourceFound) 
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
            _resources.Remove(resourcesToTake[i]);
            Destroy(resourcesToTake[i].gameObject);
        }

        return true;
    }

    public void RemoveResource()
    {
        int indexOfResource = _resources.Count - 1;

        Resource resourceToRemove = _resources[indexOfResource];
        _resources.RemoveAt(indexOfResource);

        Destroy(resourceToRemove.gameObject);
    }
}
