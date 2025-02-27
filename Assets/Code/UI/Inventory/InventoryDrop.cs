using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler
{
    private ItemSpawnManager itemSpawnManager;

    public Transform dropPlace;

    private void Awake()
        {
            itemSpawnManager = FindObjectOfType<ItemSpawnManager>();
            if (itemSpawnManager == null)
                Debug.LogError("ItemSpawnManager not found in the scene!");
        }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped != null)
        {
            InventoryItem dragableItem = dropped.GetComponent<InventoryItem>();
            if (dragableItem != null && itemSpawnManager != null)
            {
                itemSpawnManager.CreateDroppedItem(dragableItem.item, dragableItem.count, dropPlace.position);
                Destroy(dropped);
            }
        }
    }
}
//sa