using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Item itemSO;
    public Item itm => itemSO;
}
