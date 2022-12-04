using System;
using System.Collections;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    [SerializeField]
    private float _interactionRadious = 2;

    [SerializeField]
    private LayerMask _interactionLayer;

    // time between taking and giving resources
    [SerializeField]
    [Range(0.1f, 0.5f)]
    private float _interactRate = 0.2f;

    private enum InteractionState
    {
        GivingResources,
        TakingResources,
        None
    }

    private InteractionState _currentInteractionState = InteractionState.None;

    // storage, we interact with at the moment
    private ResourcesStorage _currentResourcesStorage;

    private Coroutine _takingResourcesCoroutine;
    private Coroutine _givingResourcesCoroutine;

    private void Update()
    {
        ResourcesStorage playerStorage = Player.Instance.ResourcesStorage;
        Vector3 playerPosition = Player.Instance.transform.position;

        if (_currentInteractionState == InteractionState.GivingResources)
        {
            // if distance beetwen player and storage more tran interaction radious
            // stop giving the resources

            float distanceToStorage = Vector3.Distance(
                playerPosition,
                _currentResourcesStorage.transform.position
            );

            if (distanceToStorage > _interactionRadious)
            {
                _currentInteractionState = InteractionState.None;
                StopCoroutine(_givingResourcesCoroutine);
                _currentResourcesStorage = null;
            }

            return;
        }

        if (_currentInteractionState == InteractionState.TakingResources)
        {
            // if distance beetwen player and storage more tran interaction radious
            // stop taking the resources

            float distanceToStorage = Vector3.Distance(
                playerPosition,
                _currentResourcesStorage.transform.position
            );

            if (distanceToStorage > _interactionRadious)
            {
                _currentInteractionState = InteractionState.None;
                StopCoroutine(_takingResourcesCoroutine);
                _currentResourcesStorage = null;
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

        ResourcesStorage resourcesStorageToInteract;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<ResourcesStorage>(out resourcesStorageToInteract))
            {
                // if found storage is fuel storage player gives him the resources
                // otherwise player takes the resources

                if (resourcesStorageToInteract is FuelStorage)
                {
                    if (playerStorage.IsEmpty)
                        return;

                    if (resourcesStorageToInteract.IsFull)
                        return;

                    _currentInteractionState = InteractionState.GivingResources;
                    _givingResourcesCoroutine = StartCoroutine(
                        GiveResourcesTo(resourcesStorageToInteract as FuelStorage)
                    );
                }
                else
                {
                    if (playerStorage.IsFull)
                        return;

                    if (resourcesStorageToInteract.IsEmpty)
                        return;

                    _currentInteractionState = InteractionState.TakingResources;
                    _takingResourcesCoroutine = StartCoroutine(
                        TakeResourcesFrom(resourcesStorageToInteract)
                    );
                }

                _currentResourcesStorage = resourcesStorageToInteract;
            }
        }
    }

    private IEnumerator TakeResourcesFrom(ResourcesStorage resourcesStorage)
    {
        ResourcesStorage playerStorage = Player.Instance.ResourcesStorage;

        while (true)
        {
            Resource takenResource = null;

            // try take resource from storage
            if (!resourcesStorage.TryToGiveResource(out takenResource))
            {
                break;
            }
            // try to add taken resource to player's storage
            if (!playerStorage.TryToAddResource(takenResource))
            {
                // return taken resource to resource storage
                resourcesStorage.TryToAddResource(takenResource);
                break;
            }

            yield return new WaitForSeconds(_interactRate);
        }

        _currentInteractionState = InteractionState.None;
    }

    private IEnumerator GiveResourcesTo(FuelStorage fuelResourcesStorage)
    {
        ResourcesStorage playerStorage = Player.Instance.ResourcesStorage;

        while (true)
        {
            Type typeOfNeededResource = fuelResourcesStorage.GetTypeOfNeededResource();

            Resource resourceTakenFromPlayer;

            bool isPlayerHasRequiredResource = playerStorage.TryToGiveResourceByType(
                typeOfNeededResource,
                out resourceTakenFromPlayer
            );

            if (!isPlayerHasRequiredResource)
                break;

            fuelResourcesStorage.TryToAddResource(resourceTakenFromPlayer);

            yield return new WaitForSeconds(_interactRate);
        }

        _currentInteractionState = InteractionState.None;
    }
}
