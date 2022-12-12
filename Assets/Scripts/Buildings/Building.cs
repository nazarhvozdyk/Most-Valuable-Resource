using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour
{
    [SerializeField]
    private List<ResourceAmount> _price;
    public Dictionary<Type, int> Price
    {
        get
        {
            Dictionary<Type, int> price = new Dictionary<Type, int>(_price.Count);

            for (int i = 0; i < _price.Count; i++)
                price.Add(_price[i].resource.GetType(), _price[i].Amount);

            return price;
        }
    }
}
