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
    private float _interactRate = 0.2f;

    private enum InteractionState
    {
        GivingResources,
        TakingResources,
        None
    }

    private InteractionState _currentInteractionState = InteractionState.None;

    // stotage we interact with at the moment
    private ResourcesStorage _currentResourcesStorage;

    private void Start()
    {
        Application.targetFrameRate = 25;
    }

    private void Update()
    {
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
                StopCoroutine(GiveResourcesTo(_currentResourcesStorage as FuelStorage));
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
                StopCoroutine(TakeResourcesFrom(_currentResourcesStorage));
                _currentResourcesStorage = null;
            }

            return;
        }

        // searching the storage

        Collider[] colliders = new Collider[1];
        int collidersCount = Physics.OverlapSphereNonAlloc(
            playerPosition,
            _interactionRadious,
            colliders,
            _interactionLayer
        );

        if (collidersCount == 0)
            return;

        ResourcesStorage resourcesStorage;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent<ResourcesStorage>(out resourcesStorage))
            {
                if (resourcesStorage is FuelStorage)
                {
                    if (resourcesStorage.IsFull)
                        return;

                    StartCoroutine(GiveResourcesTo(resourcesStorage as FuelStorage));
                    _currentInteractionState = InteractionState.GivingResources;
                }
                else
                {
                    if (resourcesStorage.IsEmpty)
                        return;

                    StartCoroutine(TakeResourcesFrom(resourcesStorage));
                    _currentInteractionState = InteractionState.TakingResources;
                }

                _currentResourcesStorage = resourcesStorage;
                return;
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
            if (!resourcesStorage.TryToGiveOneResource(out takenResource))
            {
                // if its not posible stop coroutine
                StopCoroutine(TakeResourcesFrom(resourcesStorage));
                _currentInteractionState = InteractionState.None;
                yield break;
            }
            // try to add taken resource to player's storage
            if (!playerStorage.TryToAddResource(takenResource))
            {
                // if its not posible stop coroutine
                StopCoroutine(TakeResourcesFrom(resourcesStorage));
                _currentInteractionState = InteractionState.None;

                // and return taken resource to resource storage
                resourcesStorage.TryToAddResource(takenResource);
                yield break;
            }

            yield return new WaitForSeconds(_interactRate);
        }
    }

    private IEnumerator GiveResourcesTo(FuelStorage fuelResourcesStorage)
    {
        ResourcesStorage playerStorage = Player.Instance.ResourcesStorage;

        while (true)
        {
            Resource resourceTakenFromPlayer = null;

            // try to add taken resource to player's storage
            for (int i = 0; i < fuelResourcesStorage.ResourcesToStore.Length; i++)
            {
                // get the resource's type that stores the warehouse
                Type typeOfNeededResource = fuelResourcesStorage.ResourcesToStore[i].GetType();

                // if fuel resource found, stop searching
                if (
                    playerStorage.TryToGiveResourceByType(
                        typeOfNeededResource,
                        out resourceTakenFromPlayer
                    )
                )
                {
                    break;
                }
            }

            // if resource that fuel storage needed not find stop coroutine
            if (resourceTakenFromPlayer == null)
            {
                StopCoroutine(GiveResourcesTo(fuelResourcesStorage));
                _currentInteractionState = InteractionState.None;
                yield break;
            }

            if (!fuelResourcesStorage.TryToAddResource(resourceTakenFromPlayer))
            {
                // if its not posible stop coroutine
                StopCoroutine(GiveResourcesTo(fuelResourcesStorage));
                _currentInteractionState = InteractionState.None;

                // and return taken resource to resource storage
                playerStorage.TryToAddResource(resourceTakenFromPlayer);
                yield break;
            }

            yield return new WaitForSeconds(_interactRate);
        }
    }
}
