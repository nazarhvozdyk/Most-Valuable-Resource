using System;
using UnityEngine;

public class ResourceHint : MonoBehaviour
{
    [SerializeField]
    private Transform _resourceUIStartTransform;

    [SerializeField]
    private ResourceUI _resourceUIPrefab;
    private ResourceUI _resourceUI;
    private ResourceUI[] _resourcesInUI;

    public void SetUp(ResourceAmount[] resourcesAmount)
    {
        Canvas worldCanvas = WorldCanvasReference.Instance.Canvas;
        _resourcesInUI = new ResourceUI[resourcesAmount.Length];

        for (int i = 0; i < resourcesAmount.Length; i++)
        {
            Type resourceType = resourcesAmount[i].resource.GetType();
            ResourceSpritesData resourceSpritesData = UIManager.Instance.ResourceIconData;
            Sprite resourceSprite = resourceSpritesData.GetSpriteByResourceType(resourceType);

            _resourceUI = Instantiate(_resourceUIPrefab, worldCanvas.transform);
            _resourceUI.ClearText();
            _resourceUI.SetSprite(resourceSprite);
            Vector3 nextPosition =
                _resourceUIStartTransform.position + _resourceUIStartTransform.up * (2 * i);
            _resourceUI.transform.position = nextPosition;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
        for (int i = 0; i < _resourcesInUI.Length; i++)
            Destroy(_resourcesInUI[i].gameObject);
    }
}
