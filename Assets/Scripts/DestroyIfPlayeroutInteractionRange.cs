using UnityEngine;

public class DestroyIfPlayeroutInteractionRange : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 playerPosition = Player.Instance.transform.position;

        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        if (distanceToPlayer > InteractionSystem.Instance.InteractionDistance)
            Destroy(gameObject);
    }
}
