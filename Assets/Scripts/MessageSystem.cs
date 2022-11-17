using UnityEngine;

public class MessageSystem : MonoBehaviour
{
    private static readonly string messageMainPart = " is stopped because ";

    private static MessageSystem _instance;
    public static MessageSystem Instance
    {
        get => _instance;
    }

    [SerializeField]
    private Message _messagePrefab;

    [SerializeField]
    private RectTransform _messagesParent;

    private void Awake()
    {
        _instance = this;
    }

    public void OnFactoryStopped(string factoryName, string whyStopped)
    {
        string finalMessage = factoryName + messageMainPart + whyStopped;

        Message message = Instantiate(_messagePrefab, _messagesParent);
        message.ShowMessage(finalMessage);
    }
}
