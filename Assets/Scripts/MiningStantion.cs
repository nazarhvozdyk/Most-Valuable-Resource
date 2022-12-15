using System.Collections;
using UnityEngine;
using System;

public class MiningStantion : Building
{
    [SerializeField]
    private Resource _miningResourcePrefab;

    [SerializeField]
    private ResourcesStorage _minedResourcesStorage;

    [SerializeField]
    private int _resourcesInMinute = 60;
    private float _prodactionRate;

    private void OnEnable()
    {
        _prodactionRate = 60f / _resourcesInMinute;
    }

    private void Start()
    {
        StartCoroutine(MineResources());
    }

    private void OnResourceTakenFromFullStorage(Type typeOfTakenResource)
    {
        StartCoroutine(MineResources());
        _minedResourcesStorage.onResourceTaken -= OnResourceTakenFromFullStorage;
    }

    private IEnumerator MineResources()
    {
        while (!_minedResourcesStorage.IsFull())
        {
            Resource minedResource = Instantiate(_miningResourcePrefab);

            minedResource.transform.position = transform.position;

            if (!_minedResourcesStorage.TryToAddResource(minedResource))
                Destroy(minedResource);

            yield return new WaitForSeconds(_prodactionRate);
        }

        _minedResourcesStorage.onResourceTaken += OnResourceTakenFromFullStorage;
    }
}
