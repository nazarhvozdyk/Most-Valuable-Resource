using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    private static SpaceShip _instance;
    public static SpaceShip Instance
    {
        get => _instance;
    }

    [SerializeField]
    private List<ResourceAmount> _neededResourcesToFlyInspector;

    [SerializeField]
    private PriceStorage _priceStorage;
    private Dictionary<Type, int> _neededResourcesToFly;

    [SerializeField]
    private ParticleSystem _flameEffect;

    private void Awake()
    {
        _instance = this;

        _neededResourcesToFly = new Dictionary<Type, int>(_neededResourcesToFlyInspector.Count);

        for (int i = 0; i < _neededResourcesToFlyInspector.Count; i++)
            _neededResourcesToFly.Add(
                _neededResourcesToFlyInspector[i].resource.GetType(),
                _neededResourcesToFlyInspector[i].Amount
            );
    }

    private void Start()
    {
        _flameEffect.Stop();
        _priceStorage.SetUp(_neededResourcesToFly);
        _priceStorage.onStorageIsFull += OnStorageIsFull;
    }

    private void OnStorageIsFull()
    {
        _flameEffect.Play();
        GameManager.Instance.OnLevelTaskComplited();
    }
}
