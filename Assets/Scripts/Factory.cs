using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Factory : Building
{
    public enum StopReason
    {
        NoFuel,
        NoSpace
    }

    // resources that factory use as fuel
    [SerializeField]
    private Resource[] _fuelResources;

    [SerializeField]
    private Resource _producedResourcePrefab;

    [SerializeField]
    private FactoryFuelStorage _fuelStorage;

    [SerializeField]
    private ResourcesStorage _producedResourcesStorage;

    // the start position of spawned resource
    [SerializeField]
    private Transform _resourceStartPositionTransform;

    [SerializeField]
    private int _resourcesInMinute = 60;
    private float _prodactionRate;
    public Resource[] FuelResources
    {
        get => _fuelResources;
    }
    private bool _isProducing = false;

    private void OnEnable()
    {
        _prodactionRate = 60f / _resourcesInMinute;
    }

    private Type[] _fuelResourcesTypes;
    public delegate void FactoryStopHandler(StopReason stopReason);
    public event FactoryStopHandler onFactoryStopped;
    public delegate void FactoryStartHandler();
    public event FactoryStartHandler onFactoryStartProducing;

    private void Start()
    {
        _fuelResourcesTypes = new Type[_fuelResources.Length];

        for (int i = 0; i < _fuelResourcesTypes.Length; i++)
            _fuelResourcesTypes[i] = _fuelResources[i].GetType();

        if (_fuelStorage.ResourcesCount > 0)
            StartProduction();
        else
            _fuelStorage.onResourceAdded += OnFirstFuelResourceAdded;
    }

    private void OnFirstFuelResourceAdded(Type typeOfResource)
    {
        StartProduction();
        _fuelStorage.onResourceAdded -= OnFirstFuelResourceAdded;
    }

    private void OnResourceTakenFromFullStorage(Type resourceType)
    {
        StartProduction();
        _producedResourcesStorage.onResourceTaken -= OnResourceTakenFromFullStorage;
    }

    private void StartProduction()
    {
        StartCoroutine(TryToProduceResources());
    }

    private void StopProducing(StopReason stopReason)
    {
        _isProducing = false;
        StopCoroutine(TryToProduceResources());
        onFactoryStopped?.Invoke(stopReason);
    }

    private IEnumerator TryToProduceResources()
    {
        while (true)
        {
            if (_producedResourcesStorage.IsFull())
            {
                if (_isProducing)
                    StopProducing(StopReason.NoSpace);

                _isProducing = false;
                _producedResourcesStorage.onResourceTaken += OnResourceTakenFromFullStorage;
                yield break;
            }

            if (!_fuelStorage.TryToRemoveResourcesByTypes(_fuelResourcesTypes))
            {
                if (_isProducing)
                    StopProducing(StopReason.NoFuel);

                // subscribe on resource added event so production
                // continue when fuel storage get some fuel
                _fuelStorage.onResourceAdded += OnFirstFuelResourceAdded;
                _isProducing = false;
                yield break;
            }

            Resource producedResource = Instantiate(_producedResourcePrefab);
            producedResource.transform.position = _resourceStartPositionTransform.position;

            _producedResourcesStorage.TryToAddResource(producedResource);

            if (_isProducing == false)
            {
                onFactoryStartProducing?.Invoke();
                _isProducing = true;
            }

            yield return new WaitForSeconds(_prodactionRate);
        }
    }
}
