using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryDrop : MonoBehaviour, IDropHandler
{
    public Transform dropPlace; //to setup from player obj
    public GameObject droppedItemPrefab;

    public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            InventoryItem dragableItem = dropped.GetComponent<InventoryItem>();
            if (dragableItem != null)
                {
                    Destroy(dropped); //Pooling needed
                    Vector3 dropPoint = new Vector3(dropPlace.position.x ,dropPlace.position.y + 5f, dropPlace.position.z);
                    CreateDroppedItem(dragableItem.item, dragableItem.count, dropPoint);
                }
        }

    private void CreateDroppedItem(Item item, int stackSize, Vector3 position) //Pooling needed
        {
            GameObject droppedItemGO = Instantiate(droppedItemPrefab, position, Quaternion.identity);
            DroppedItem droppedItemScript = droppedItemGO.GetComponentInChildren<DroppedItem>();
            droppedItemScript.localItem = item;
            droppedItemScript.localStack = stackSize;

            Rigidbody rigidbody = droppedItemGO.GetComponent<Rigidbody>();

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    Vector3 direction = (hit.point - position).normalized;

                    rigidbody.velocity = direction * 10;
                }
        } //Needs refactoring to pull stuff and not instantiate
}

//Drop place would need additional raycast check to drop object in correct scene