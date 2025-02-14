using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;

    public GameObject inventoryItemPrefab;
    
    public bool AddItem(Item item)
        {
            for (int i = 0; i < inventorySlots.Length; i++) //for stacks
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < itemInSlot.maxStackSize)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return true;
                }
            }

            for (int i = 0; i < inventorySlots.Length; i++) //for new items
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }

            return false;
        }


    void SpawnNewItem(Item item, InventorySlot slot)
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item);
        }

}
