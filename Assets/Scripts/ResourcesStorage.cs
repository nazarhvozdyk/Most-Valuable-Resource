using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour, IStorable
{
    // the position of first resource
    [SerializeField]
    private Transform _resourceParent;

    [SerializeField]
    protected int _capacity = 20;

    [SerializeField]
    private int _rowsCount = 4;

    [SerializeField]
    private int _colsCount = 5;
    public int ResourcesCount
    {
        get => _resources.Count;
    }
    protected List<Resource> _resources;
    public delegate void ResourceAddedHandler(Type addedResourceType);
    public event ResourceAddedHandler onResourceAdded;
    public delegate void ResourceTakenHandler(Type takenResourceType);
    public event ResourceTakenHandler onResourceTaken;

    protected virtual void Awake()
    {
        _resources = new List<Resource>(_capacity);
    }

    public virtual bool TryToAddResource(Resource resource)
    {
        if (_resources.Count == _capacity)
            return false;

        resource.GoTo(CalculateNextLocalPosition(), _resourceParent);
        _resources.Add(resource);

        onResourceAdded?.Invoke(resource.GetType());

        return true;
    }

    protected void UpdateAllPositionsStartsFrom(int startIndex)
    {
        for (int i = startIndex; i < _resources.Count; i++)
            _resources[i].GoTo(CalculateLocalPositionFor(i), _resourceParent);
    }

    public virtual bool TryToGiveResourceByType(Type resourceType, out Resource resource)
    {
        resource = null;

        if (IsEmpty())
            return false;

        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            if (_resources[i].GetType() == resourceType)
            {
                resource = _resources[i];
                _resources.RemoveAt(i);
                // update the position to avoid epmty space
                UpdateAllPositionsStartsFrom(i);
                onResourceTaken?.Invoke(resource.GetType());
                return true;
            }
        }

        return false;
    }

    public bool TryToGiveResource(out Resource resource)
    {
        resource = null;

        if (IsEmpty())
            return false;

        resource = _resources[_resources.Count - 1];
        _resources.RemoveAt(_resources.Count - 1);

        onResourceTaken?.Invoke(resource.GetType());
        return true;
    }

    // calculates the position of the resource by the index and return it
    private Vector3 CalculateLocalPositionFor(int index)
    {
        int x = index % _rowsCount;
        int y = index / (_rowsCount * _colsCount);
        int z = index / _rowsCount % _colsCount;

        Vector3 position = new Vector3(x, y, z) * GridInfo.Instance.GridOffset;
        return position;
    }

    // calculates the next position of the resource and return it
    private Vector3 CalculateNextLocalPosition()
    {
        int x = _resources.Count % _rowsCount;
        int y = _resources.Count / (_rowsCount * _colsCount);
        int z = _resources.Count / _rowsCount % _colsCount;

        Vector3 position = new Vector3(x, y, z) * GridInfo.Instance.GridOffset;

        return position;
    }

    public Vector3 GetStoragePosition()
    {
        return transform.position;
    }

    public virtual Type[] GetTypesOfNeededResources()
    {
        Type[] types = new Type[1];
        types[0] = _resources[_resources.Count - 1].GetType();
        return types;
    }

    public bool IsFull()
    {
        return _resources.Count == _capacity;
    }

    public bool IsEmpty()
    {
        return _resources.Count == 0;
    }
}
