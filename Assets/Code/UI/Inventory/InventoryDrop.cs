using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler
{
    public Transform dropPlace;
    public GameObject droppedItemPrefab;

    public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem dragableItem = dropped.GetComponent<InventoryItem>();
            if (dragableItem != null)
                {
                    Destroy(dropped);
                    CreateDroppedItem(dragableItem.item, dragableItem.count, dropPlace.position);
                }
        }

    private void CreateDroppedItem(Item item, int stackSize, Vector3 position)
        {
            GameObject droppedItemGO = Instantiate(droppedItemPrefab, position, Quaternion.identity);
            DroppedItem droppedItemScript = droppedItemGO.GetComponentInChildren<DroppedItem>();
            droppedItemScript.item = item;
            droppedItemScript.localStack = stackSize;
        } //Needs refactoring to pull stuff and not instantiate
}

//Drop place would need additional raycast check to drop object in correct scene