using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;

    public void SpawnNewItem(Item item, InventorySlot slot, int count)
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
            inventoryItem.InitialiseItem(item, count);
        }
}

//Inventory item storage and spawner