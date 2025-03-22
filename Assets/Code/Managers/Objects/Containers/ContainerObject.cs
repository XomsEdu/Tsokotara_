using UnityEngine;

public class ContainerObject : MonoBehaviour
{
    protected Transform UIContainer;
    protected GameObject containerInventory;

    public void InventoryToCanva()
        {
            containerInventory.transform.SetParent(UIContainer);
            containerInventory.transform.localScale = Vector3.one;
            containerInventory.SetActive(true);
        }

    public void CanvaToInventory()
        {
            containerInventory.SetActive(false);
            containerInventory.transform.SetParent(transform);
        }
}

//Class for UI holders to move UI inventories (not only inventories btw). Basically UI puller