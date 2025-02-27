using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;
    public int stackSize;
    private bool isBeingPickedUp = false;

    private void OnTriggerEnter(Collider other)
        {
            if (isBeingPickedUp) return;
            
            CollectorManager collector = other.GetComponent<CollectorManager>();
            if (collector != null) collector.PickUpItem(this); isBeingPickedUp = true;
        }
    
    public void ReduceStackSize(int amount)
        {
            stackSize -= amount;
            if (stackSize <= 0) Destroy(gameObject);
        }
        
    private void OnTriggerExit(Collider other)
        {
            isBeingPickedUp = false;
        }
}

//Loot on ground code