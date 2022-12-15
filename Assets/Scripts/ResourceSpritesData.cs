using UnityEngine;

[CreateAssetMenu(fileName = "ResourceSpritesData", menuName = "Game/ResourceSpritesData")]
public class ResourceSpritesData : ScriptableObject
{
    [SerializeField]
    private Sprite _oilSprite;

    public Sprite Oil
    {
        get => _oilSprite;
    }
}
