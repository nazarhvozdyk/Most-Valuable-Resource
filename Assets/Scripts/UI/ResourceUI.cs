using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour
{
    [SerializeField]
    private Image _resourceImage;

    [SerializeField]
    private Text _amountText;
    private int _currentResourceAmount = 0;
    public int CurrentResourceAmount
    {
        get => _currentResourceAmount;
    }

    public void SetSprite(Sprite sprite)
    {
        _resourceImage.sprite = sprite;
    }

    public void Add(int amount)
    {
        _currentResourceAmount += amount;
        _amountText.text = _currentResourceAmount.ToString();
    }
}
