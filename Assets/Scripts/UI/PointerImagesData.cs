using UnityEngine;

[CreateAssetMenu(fileName = "PointerImagesData", menuName = "Game/PointerImagesData")]
public class PointerImagesData : ScriptableObject
{
    [SerializeField]
    private Sprite _noFuelSprite;

    [SerializeField]
    private Sprite _noSpaceSprite;
    public Sprite NoFuelSprite
    {
        get => _noFuelSprite;
    }
    public Sprite NoSpaceSprite
    {
        get => _noSpaceSprite;
    }
}
