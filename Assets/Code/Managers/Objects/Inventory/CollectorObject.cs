using UnityEngine;

public class CollectorObject : MonoBehaviour
{
    public InventorySlot[] pickUpSlots;
    public GameObject inventoryItemPrefab;

    public void PickUpItem(DroppedItem droppedItem)
        {
            if (droppedItem == null) return;

            int maxStackSize = droppedItem.item.stackSize;

            foreach (InventorySlot slot in pickUpSlots)
                {
                    if (slot == null) continue;

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

                    if (itemInSlot == null)
                        {
                            int toAdd = Mathf.Min(maxStackSize, droppedItem.stackSize);
                            SpawnNewItem(droppedItem.item, slot, toAdd);
                            droppedItem.ReduceStackSize(toAdd);

                            if (droppedItem.stackSize <= 0) return;
                        }
                }
        }
    
    public void SpawnNewItem(Item item, InventorySlot slot, int count) //UI logic
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item, count);
        } //Maybe need to refactor replacing instantiate with pull
}

//Calculator for picking up items on collector object