using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    public GameObject droppedItemPrefab;

    public void CreateDroppedItem(Item item, int stackSize, Vector3 position)
        {
            GameObject droppedItemGO = Instantiate(droppedItemPrefab, position, Quaternion.identity);
            DroppedItem droppedItemScript = droppedItemGO.GetComponentInChildren<DroppedItem>();
            droppedItemScript.item = item;
            droppedItemScript.stackSize = stackSize;
        }
}
