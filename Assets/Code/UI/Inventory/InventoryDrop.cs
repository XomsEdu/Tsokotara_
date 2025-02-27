using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler
{
    public Transform dropPlace;
    public GameObject droppedItemPrefab;

    public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            if (dropped != null)
            {
                InventoryItem dragableItem = dropped.GetComponent<InventoryItem>();
                if (dragableItem != null)
                {
                    CreateDroppedItem(dragableItem.item, dragableItem.count, dropPlace.position);
                    Destroy(dropped);
                }
            }
        }

    private void CreateDroppedItem(Item item, int stackSize, Vector3 position)
        {
            GameObject droppedItemGO = Instantiate(droppedItemPrefab, position, Quaternion.identity);
            DroppedItem droppedItemScript = droppedItemGO.GetComponentInChildren<DroppedItem>();

            droppedItemScript.item = item;
            droppedItemScript.stackSize = stackSize;
        }
}
