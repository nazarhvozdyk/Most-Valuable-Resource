using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1)]
public class Factory : MonoBehaviour
{
    private static readonly string _thereIsNoFuelString = "there is no fuel anymore";
    private static readonly string _thereIsNoSpaceLeft = "there is no space left for new resources";

    [SerializeField]
    private string _factoryName = "Factory";

    // resources that factory use as fuel
    [SerializeField]
    private Resource[] _fuelResources;

    [SerializeField]
    private Resource _producedResourcePrefab;

    [SerializeField]
    private FuelStorage _fuelStorage;

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

    private void OnFirstFuelResourceAdded()
    {
        StartProduction();
        _fuelStorage.onResourceAdded -= OnFirstFuelResourceAdded;
    }

    private void OnResourceTakenFromFullStorage()
    {
        StartProduction();
        _producedResourcesStorage.onResourceTaken -= OnResourceTakenFromFullStorage;
    }

    private void StartProduction() => StartCoroutine(ProduceResources());

    private void StopProducing() => StopCoroutine(ProduceResources());

    private IEnumerator ProduceResources()
    {
        while (true)
        {
            if (_fuelStorage.ResourcesCount < _fuelResources.Length)
            {
                if (_isProducing)
                    MessageSystem.Instance.OnFactoryStopped(_factoryName, _thereIsNoFuelString);

                _isProducing = false;
                // subscribe on resource added event so production
                // continue when fuel storage get some fuel
                _fuelStorage.onResourceAdded += OnFirstFuelResourceAdded;
                StopProducing();
                yield break;
            }

            if (_producedResourcesStorage.IsFull)
            {
                if (_isProducing)
                    MessageSystem.Instance.OnFactoryStopped(_factoryName, _thereIsNoSpaceLeft);

                _isProducing = false;
                _producedResourcesStorage.onResourceTaken += OnResourceTakenFromFullStorage;
                StopProducing();
                yield break;
            }

            if (!_fuelStorage.TryToRemoveResourcesByTypes(_fuelResourcesTypes))
            {
                MessageSystem.Instance.OnFactoryStopped(_factoryName, _thereIsNoFuelString);
                _fuelStorage.onResourceAdded += OnFirstFuelResourceAdded;
                _isProducing = false;
                StopProducing();
                yield break;
            }

            Resource producedResource = Instantiate(_producedResourcePrefab);
            producedResource.transform.position = _resourceStartPositionTransform.position;

            _producedResourcesStorage.TryToAddResource(producedResource);
            _isProducing = true;
            yield return new WaitForSeconds(_prodactionRate);
        }
    }
}
