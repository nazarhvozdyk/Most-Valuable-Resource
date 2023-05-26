using UnityEngine;
using UnityEngine.UI;

public class PointerController : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    public enum Rotation
    {
        Up,
        Down,
        Left,
        Right,
        Center
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    public void SetRotation(Rotation rotation)
    {
        Vector2 pivot = Vector2.zero;

        if (rotation == Rotation.Up)
            pivot = new Vector2(0.5f, 1);
        else if (rotation == Rotation.Down)
            pivot = new Vector2(0.5f, 0);
        else if (rotation == Rotation.Right)
            pivot = new Vector2(1, 0.5f);
        else if (rotation == Rotation.Left)
            pivot = new Vector2(0, 0.5f);
        else if (rotation == Rotation.Center)
            pivot = new Vector2(0.5f, 0.5f);

        RectTransform rectTransform = transform as RectTransform;
        rectTransform.pivot = pivot;
    }

    public void SetSprite(Sprite sprite) => _image.sprite = sprite;
}
