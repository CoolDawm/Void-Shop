using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingList : MonoBehaviour
{
    [SerializeField]
    private Item[] shoppingItems=new Item[5];
    public bool CheckInventoryWithList(Item[] items)
    {
       int trueCount = 0;
       for(int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < shoppingItems.Length; j++)
            {
                if (items[i] == shoppingItems[j]) {
                    trueCount++;
                    break;
                }
            }
        }
        return trueCount is 5;
    }
}
