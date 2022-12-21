using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAgainButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.LoadFirstLevel();
    }
}
