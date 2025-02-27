using UnityEngine;

public class CollectorManager : MonoBehaviour
{
    public InventoryManager inventory;

    public void PickUpItem(DroppedItem droppedItem)
        {
            if (inventory == null || droppedItem == null) return;

            int maxStackSize = droppedItem.item.stackSize;

            foreach (InventorySlot slot in inventory.inventorySlots)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.item == droppedItem.item)
                {
                    int spaceLeft = itemInSlot.item.stackSize - itemInSlot.count;
                    int toAdd = Mathf.Min(spaceLeft, droppedItem.stackSize);
                    itemInSlot.count += toAdd;
                    itemInSlot.RefreshCount();
                    droppedItem.ReduceStackSize(toAdd);

                    if (droppedItem.stackSize <= 0) return;
                }
            }

            foreach (InventorySlot slot in inventory.inventorySlots)
            {
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot == null)
                {
                    int toAdd = Mathf.Min(maxStackSize, droppedItem.stackSize);
                    inventory.SpawnNewItem(droppedItem.item, slot, toAdd);
                    droppedItem.ReduceStackSize(toAdd);

                    if (droppedItem.stackSize <= 0) return;
                }
            }
        }
}

//Calculator for picking up items on collector object