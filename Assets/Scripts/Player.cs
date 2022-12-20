using System.Collections;
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

    [SerializeField]
    private PlayerInputHandler _playerInputHandler;

    [SerializeField]
    private PlayerMove _playerMove;

    [SerializeField]
    private Rigidbody _rigidbody;

    public ResourcesStorage ResourcesStorage
    {
        get => _resourceStorage;
    }

    private void Awake()
    {
        _instance = this;
    }

    public void SitInSpaceShip()
    {
        Vector3 toShip = SpaceShip.Instance.transform.position - transform.position;
        float breakingForce = 2.5f;
        _playerMove.SetMoveDirection(toShip.normalized / breakingForce);
        StartCoroutine(SitInShip());
        BlockMovement();
    }

    private void BlockMovement()
    {
        _playerInputHandler.enabled = false;
    }

    private IEnumerator SitInShip()
    {
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
        GameManager.Instance.FinishTheLevel();
    }
}
