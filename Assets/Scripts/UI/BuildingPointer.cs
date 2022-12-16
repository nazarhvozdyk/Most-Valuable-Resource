using UnityEngine;

public class BuildingPointer : MonoBehaviour
{
    public void SetUp(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}
