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

    public void RemoveOneResource()
    {
        int indexOfResource = _resources.Count - 1;

        Resource resourceToRemove = _resources[indexOfResource];
        _resources.RemoveAt(indexOfResource);

        Destroy(resourceToRemove.gameObject);
    }
}
