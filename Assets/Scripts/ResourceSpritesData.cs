using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSpritesData", menuName = "Game/ResourceSpritesData")]
public class ResourceSpritesData : ScriptableObject
{
    [SerializeField]
    private ResourceIconData[] resourceIconData;

    public Sprite GetSpriteByResourceType(Type type)
    {
        for (int i = 0; i < resourceIconData.Length; i++)
        {
            if (resourceIconData[i].resource.GetType() == type)
            {
                return resourceIconData[i].sprite;
            }
        }

        return null;
    }
}
