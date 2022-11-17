using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Resource _testPrefab;

    [SerializeField]
    private ResourcesStorage _resourcesStorage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _resourcesStorage.TryToAddResource(Instantiate(_testPrefab));
        }
    }
}
