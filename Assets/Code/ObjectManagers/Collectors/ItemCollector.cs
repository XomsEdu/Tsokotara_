using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private InventorySlot[] pickUpSlots;
    [SerializeField] private GameObject inventoryItemPrefab;

    public void AddItem(IGiveItem itemObject)
        {
            if (itemObject == null) return;
            int localStack = itemObject.localCount;

            foreach (InventorySlot slot in pickUpSlots)
                {
                    if (slot == null) continue;

                    InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                    
                    if (itemInSlot != null && itemInSlot.item == itemObject.item && itemInSlot.count < itemObject.item.stackSize)
                        {
                            int spaceLeft = itemInSlot.item.stackSize - itemInSlot.count;
                            int toAdd = Mathf.Min(spaceLeft, localStack);
                            itemInSlot.count += toAdd;  itemInSlot.RefreshCount();
                            localStack -= toAdd;

                            if (localStack <= 0) break;
                        }
                    else if (itemInSlot == null)
                        {
                            int toAdd = Mathf.Min(itemObject.item.stackSize, localStack);
                            SpawnNewItem(itemObject.item, slot, toAdd);
                            localStack -= toAdd;

                            if (localStack <= 0) break;
                        }
                }

            itemObject.StackReturn(localStack);
        }
    
    public void SpawnNewItem(Item item, InventorySlot slot, int count) //UI logic
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item, count);
        } //Maybe need to refactor replacing instantiate with pull
}

//Calculator for picking up items on collector object

//Gonna need InventoryManager for SpawnNewItem() 
//Gonna need ItemCheck trigger to be used from new Quests on their addition