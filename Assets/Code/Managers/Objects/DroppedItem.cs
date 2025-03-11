using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;
    public int stackSize;

    private void OnTriggerEnter(Collider other)
        {
            CollectorManager collector = other.GetComponent<CollectorManager>();
            if (collector != null) collector.PickUpItem(this);
        }

    public void ReduceStackSize(int amount)
        {
            stackSize -= amount;
            if (stackSize <= 0) Destroy(transform.parent.gameObject);
        }
}
