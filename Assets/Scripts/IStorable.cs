using System;
using UnityEngine;

public interface IStorable
{
    public Vector3 GetStoragePosition();
    public Type[] GetTypesOfNeededResources();
    public bool TryToGiveResource(out Resource resource);
    public bool TryToGiveResourceByType(Type resourceType, out Resource resource);
    public bool TryToAddResource(Resource resource);
    public bool IsFull();
    public bool IsEmpty();
}
