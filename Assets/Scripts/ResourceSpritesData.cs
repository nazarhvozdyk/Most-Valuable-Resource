using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSpritesData", menuName = "Game/ResourceSpritesData")]
public class ResourceSpritesData : ScriptableObject
{
    [SerializeField]
    private Sprite _oilSprite;

    public Sprite GetSpriteByResourceType(Type type)
    {
        return _oilSprite;
    }
}
