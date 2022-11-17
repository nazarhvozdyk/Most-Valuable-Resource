using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player Instance
    {
        get => _instance;
    }

    [SerializeField]
    private ResourcesStorage _resourceStorage;

    public ResourcesStorage ResourcesStorage
    {
        get => _resourceStorage;
    }

    private void Awake()
    {
        _instance = this;
    }
}
