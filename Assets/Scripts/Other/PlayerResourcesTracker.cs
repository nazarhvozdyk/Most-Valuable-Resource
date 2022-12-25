using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourcesTracker : MonoBehaviour
{
    [SerializeField]
    private ResourceUI _resourceUIPrefab;

    [SerializeField]
    private RectTransform _newResourceUIParent;

    private Dictionary<Type, ResourceUI> _resourcesInUI = new Dictionary<Type, ResourceUI>();

    private void Start()
    {
        ResourcesStorage playerResourcesStorage = Player.Instance.ResourcesStorage;
        playerResourcesStorage.onResourceAdded += OnResourceAdded;
        playerResourcesStorage.onResourceTaken += OnResourceTaken;
    }

    private void OnResourceAdded(Type addedResourceType)
    {
        ResourceUI resourceUI;

        bool isPlayerAlreadyHasThisResource = _resourcesInUI.TryGetValue(
            addedResourceType,
            out resourceUI
        );
        if (!isPlayerAlreadyHasThisResource)
        {
            resourceUI = CreateIconForNewResource(addedResourceType);
            _resourcesInUI.Add(addedResourceType, resourceUI);
        }

        resourceUI.Add(1);
    }

    private ResourceUI CreateIconForNewResource(Type newResoureType)
    {
        ResourceSpritesData resourceSpritesData = UIManager.Instance.ResourceIconData;
        Sprite resourceSprite = resourceSpritesData.GetSpriteByResourceType(newResoureType);

        ResourceUI newResourceUI = Instantiate(_resourceUIPrefab, _newResourceUIParent);
        newResourceUI.SetSprite(resourceSprite);

        return newResourceUI;
    }

    private void OnResourceTaken(Type takenResourceType)
    {
        ResourceUI resourceUI = _resourcesInUI[takenResourceType];

        if (resourceUI)
            resourceUI.Add(-1);

        if (resourceUI.CurrentResourceAmount == 0)
        {
            _resourcesInUI.Remove(takenResourceType);
            Destroy(resourceUI.gameObject);
        }
    }
}
