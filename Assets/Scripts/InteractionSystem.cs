using System;
using System.Collections;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    private static InteractionSystem _instance;
    public static InteractionSystem Instance
    {
        get => _instance;
    }

    [SerializeField]
    private float _interactionRadious = 2;
    public float InteractionDistance
    {
        get => _interactionRadious;
    }

    [SerializeField]
    private LayerMask _interactionLayer;

    // time between taking and giving resources
    [SerializeField]
    [Range(0.01f, 0.3f)]
    private float _interactRate = 0.2f;

    private enum InteractionState
    {
        GivingResources,
        TakingResources,
        None
    }

    private InteractionState _currentInteractionState = InteractionState.None;

    private Coroutine _takingResourcesCoroutine;
    private Coroutine _givingResourcesCoroutine;

    // storable object, we interact with at the moment
    private IStorable _currentStorableObject;

    private void Awake()
    {
        _instance = this;
    }

    private void Start() 
    { 
        Application.targetFrameRate = 15;
    }

    private void Update()
    {
        ResourcesStorage playerStorage = Player.Instance.ResourcesStorage;
        Vector3 playerPosition = Player.Instance.transform.position;

        if (_currentInteractionState == InteractionState.GivingResources)
        {
            // if distance beetwen player and storage more tran interaction radious
            // stop giving the resources

            if (!IsPointInRange(_currentStorableObject.GetStoragePosition()))
            {
                _currentInteractionState = InteractionState.None;
                StopCoroutine(_givingResourcesCoroutine);
            }

            return;
        }

        if (_currentInteractionState == InteractionState.TakingResources)
        {
            // if distance beetwen player and storage more tran interaction radious
            // stop taking the resources

            if (!IsPointInRange(_currentStorableObject.GetStoragePosition()))
            {
                _currentInteractionState = InteractionState.None;
                StopCoroutine(_takingResourcesCoroutine);
            }

            return;
        }

        // searching the storage
        int interactionsAtOnce = 1;
        Collider[] colliders = new Collider[interactionsAtOnce];
        int collidersCount = Physics.OverlapSphereNonAlloc(
            playerPosition,
            _interactionRadious,
            colliders,
            _interactionLayer
        );

        if (collidersCount == 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<IStorable>(out _currentStorableObject))
            {
                // if found storage is fuel storage player gives him the resources
                // otherwise player takes the resources
                if (_currentStorableObject.GetType().IsSubclassOf(typeof(FuelStorage)))
                {
                    if (_currentStorableObject.IsFull())
                        return;

                    _currentInteractionState = InteractionState.GivingResources;
                    _givingResourcesCoroutine = StartCoroutine(GiveResources());
                }
                else
                {
                    if (_currentStorableObject.IsEmpty())
                        return;

                    _currentInteractionState = InteractionState.TakingResources;
                    _takingResourcesCoroutine = StartCoroutine(TakeResources());
                }
            }
        }
    }

    // returns true if point in interaction range of player
    private bool IsPointInRange(Vector3 point)
    {
        Vector3 playerPosition = Player.Instance.transform.position;
        return Vector3.Distance(playerPosition, point) < _interactionRadious;
    }

    private IEnumerator TakeResources()
    {
        IStorable playerStorage = Player.Instance.ResourcesStorage;

        while (true)
        {
            Resource takenResource = null;

            if (playerStorage.IsFull())
                break;

            if (!_currentStorableObject.TryToGiveResource(out takenResource))
                break;

            playerStorage.TryToAddResource(takenResource);
            yield return new WaitForSeconds(_interactRate);
        }

        _currentInteractionState = InteractionState.None;
    }

    private IEnumerator GiveResources()
    {
        IStorable playerStorage = Player.Instance.ResourcesStorage;

        while (true)
        {
            if (playerStorage.IsEmpty())
                break;

            if (_currentStorableObject.IsFull())
                break;

            Type[] typesOfNeededResources = _currentStorableObject.GetTypesOfNeededResources();

            if (typesOfNeededResources == null)
                break;

            bool isResourceGiven = false;

            for (int i = 0; i < typesOfNeededResources.Length; i++)
            {
                Resource resourceTakenFromPlayer;

                bool isPlayerHasRequiredResource = playerStorage.TryToGiveResourceByType(
                    typesOfNeededResources[i],
                    out resourceTakenFromPlayer
                );

                if (!isPlayerHasRequiredResource)
                    continue;

                _currentStorableObject.TryToAddResource(resourceTakenFromPlayer);
                isResourceGiven = true;
                i = typesOfNeededResources.Length;
            }

            if (!isResourceGiven)
                break;

            yield return new WaitForSeconds(_interactRate);
        }

        _currentInteractionState = InteractionState.None;
    }
}
