using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResourcesToAddTracker : MonoBehaviour
{
    [SerializeField]
    private Transform _UIPositionTransform;

    [SerializeField]
    private BuildingStorage _buildingStorage;
    private Dictionary<Type, ResourceUI> _resourcesInUI = new Dictionary<Type, ResourceUI>();

    private void Start()
    {
        _buildingStorage.onResourceAdded += OnResourceAdded;
        _buildingStorage.onResourceTaken += OnResourceTaken;
    }

    private void OnResourceAdded(Type addedResourceType) { }

    private void OnResourceTaken(Type takenResourceType)
    {
        // update UI
    }

    private void CreateUI()
    {
        Dictionary<Type, int> price = _buildingStorage.BuildingToBuild.Price;

        foreach (var item in price)
        {
            ResourceUI resourceUI = BuildingResourcesTrackerCreator.Instance.CreateResourceUI();
        }
    }

    private Vector3 CalculateNextLocalPosition(float heightOfResourceUI)
    {
        Vector3 position =
            _UIPositionTransform.position
            + new Vector3(0, heightOfResourceUI + _resourcesInUI.Count, 0);

        return position;
    }
}
