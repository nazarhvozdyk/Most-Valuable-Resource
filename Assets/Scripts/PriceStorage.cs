using System;
using UnityEngine;
using System.Collections.Generic;

public class PriceStorage : FuelStorage
{
    [SerializeField]
    private ResourceUI _resourceUIPrefab;

    [SerializeField]
    private float _priceTagsOffset = 2;

    [SerializeField]
    private Transform _priceTagStartPositionTransform;

    private Dictionary<Type, int> _price;
    private Dictionary<Type, int> _eachTypeAmount = new Dictionary<Type, int>();

    private Dictionary<Type, ResourceUI> _priceTags;
    public delegate void OnStorageIsFullHandler();
    public event OnStorageIsFullHandler onStorageIsFull;

    protected override void Awake() { }

    public void SetUp(Dictionary<Type, int> price)
    {
        _price = price;
        _capacity = 0;
        _priceTags = new Dictionary<Type, ResourceUI>(_price.Count);

        foreach (var item in _price)
        {
            _eachTypeAmount.Add(item.Key, 0);
            _capacity += item.Value;
        }
        _resources = new List<Resource>(_capacity);
        CreatePriceTags();
    }

    public override Type[] GetTypesOfNeededResources()
    {
        List<Type> types = new List<Type>();

        foreach (var item in _price)
        {
            int amountOfNeededResources = item.Value;
            int amountOfCurrentResources = _eachTypeAmount[item.Key];

            if (amountOfCurrentResources < amountOfNeededResources)
                types.Add(item.Key);
        }

        return types.ToArray();
    }

    public override bool TryToAddResource(Resource resource)
    {
        Type resourceType = resource.GetType();

        foreach (var item in _eachTypeAmount)
        {
            // return false if we have enough of this type of resources
            // or return we dont need this resource type

            if (resourceType == item.Key.GetType())
            {
                int amountOfNeededResources = _price[item.Key];
                int amountOfCurrentResources = item.Value;

                if (amountOfNeededResources == amountOfCurrentResources)
                    return false;
            }
        }

        bool isResourceAdded = base.TryToAddResource(resource);

        if (isResourceAdded)
        {
            // increase current resources number
            _eachTypeAmount[resourceType]++;

            // decrease price tag cost
            _priceTags[resourceType].Add(-1);

            if (IsFull())
                OnStorageIsFull();
        }

        return isResourceAdded;
    }

    private void OnStorageIsFull()
    {
        foreach (var item in _priceTags.Values)
            Destroy(item.gameObject);

        _priceTags = null;
        _price = null;
        onStorageIsFull.Invoke();
    }

    private void CreatePriceTags()
    {
        Canvas worldCanvas = WorldCanvasReference.Instance.Canvas;
        int priceTagsAmounts = 0;

        foreach (var item in _price)
        {
            Type typeOfCurrentResource = item.Key;
            int amountOfResources = item.Value;

            Vector3 nextPosition =
                _priceTagStartPositionTransform.position
                + _priceTagStartPositionTransform.up * (_priceTagsOffset * priceTagsAmounts);

            ResourceUI newResourceUI = Instantiate(_resourceUIPrefab, worldCanvas.transform);
            newResourceUI.transform.position = nextPosition;

            ResourceSpritesData resourceSpritesData = UIManager.Instance.ResourceIconData;
            Sprite sprite = resourceSpritesData.GetSpriteByResourceType(typeOfCurrentResource);

            newResourceUI.SetSprite(sprite);
            newResourceUI.Add(amountOfResources);
            _priceTags.Add(typeOfCurrentResource, newResourceUI);
            priceTagsAmounts++;
        }
    }
}
