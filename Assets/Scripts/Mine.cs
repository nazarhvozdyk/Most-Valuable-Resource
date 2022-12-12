using System.Collections;
using UnityEngine;

// we can use factory with 0 fuel resources instead of mine
// but i think it would be better to split those
public class Mine : Building
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

    private void OnResourceTakenFromFullStorage()
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
