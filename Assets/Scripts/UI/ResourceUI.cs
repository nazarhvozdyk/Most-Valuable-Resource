using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
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

    public void ClearText()
    {
        _amountText.text = string.Empty;
    }

    public void Add(int amount)
    {
        _currentResourceAmount += amount;
        _amountText.text = _currentResourceAmount.ToString();
    }
}
