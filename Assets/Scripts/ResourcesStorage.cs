using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesStorage : MonoBehaviour
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
    protected List<Resource> _resources = new List<Resource>();
    public delegate void ResourceAddedHandler();
    public event ResourceAddedHandler onResourceAdded;
    public delegate void ResourceTakenHandler();
    public event ResourceTakenHandler onResourceTaken;

    public bool IsFull
    {
        get => _resources.Count == _capacity;
    }

    public bool IsEmpty
    {
        get => _resources.Count == 0;
    }

    public virtual bool TryToAddResource(Resource resource)
    {
        if (_resources.Count == _capacity)
            return false;

        resource.GoTo(CalculateNextLocalPosition(), _resourceParent);
        _resources.Add(resource);

        onResourceAdded?.Invoke();

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

        if (IsEmpty)
            return false;

        for (int i = _resources.Count - 1; i >= 0; i--)
        {
            if (_resources[i].GetType() == resourceType)
            {
                resource = _resources[i];
                _resources.RemoveAt(i);
                // update the position to avoid epmty space
                UpdateAllPositionsStartsFrom(i);
                return true;
            }
        }

        return false;
    }

    public bool TryToGiveResource(out Resource resource)
    {
        resource = null;

        if (IsEmpty)
            return false;

        resource = _resources[_resources.Count - 1];
        _resources.RemoveAt(_resources.Count - 1);

        onResourceTaken?.Invoke();
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
}
