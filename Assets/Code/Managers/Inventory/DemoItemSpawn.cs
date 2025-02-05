using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoItemSpawn : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public Item[] itemsToSpawn;

    public void PickedUpItem(int id)
    {
        bool result = inventoryManager.AddItem(itemsToSpawn[id]);

        if(result == false){
            Debug.Log("No space in backpack");
        }
    }
}
