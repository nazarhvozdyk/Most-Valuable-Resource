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

    private void OnEnable()
    {
        _prodactionRate = 60f / _resourcesInMinute;
    }

    private void Start()
    {
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
        while (_fuelStorage.ResourcesCount != 0)
        {
            if (_producedResourcesStorage.IsFull)
            {
                _producedResourcesStorage.onResourceTaken += OnResourceTakenFromFullStorage;
                StopProducing();
                MessageSystem.Instance.OnFactoryStopped(_factoryName, _thereIsNoSpaceLeft);
                yield break;
            }

            _fuelStorage.RemoveOneResource();

            Resource producedResource = Instantiate(_producedResourcePrefab);
            producedResource.transform.position = _resourceStartPositionTransform.position;

            _producedResourcesStorage.TryToAddResource(producedResource);
            yield return new WaitForSeconds(_prodactionRate);
        }

        MessageSystem.Instance.OnFactoryStopped(_factoryName, _thereIsNoFuelString);
        // subscribe on resource added event so production
        // continue when fuel storage get some fuel
        _fuelStorage.onResourceAdded += OnFirstFuelResourceAdded;
    }
}
