using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;
    public int stackSize;
    public int ID;

    private void Awake()
        {   ID = GetInstanceID();   }

    private void OnTriggerEnter(Collider other)
        {
            CollectorObject collector = other.GetComponent<CollectorObject>();
            if (collector != null) collector.PickUpItem(this);


            DroppedItem mergeItem = other.GetComponent<DroppedItem>();
            if (mergeItem != null && mergeItem.item == this.item && mergeItem.ID <= this.ID)
                {
                    mergeItem.stackSize += this.stackSize; 
                    Destroy(transform.parent.gameObject);
                } //Maybe refactor to pulling instead of destroying item
        }

    public void ReduceStackSize(int amount)
        {
            stackSize -= amount;
            if (stackSize <= 0) Destroy(transform.parent.gameObject);
        } //Maybe refactor to pulling instead of destroying item
}

//To add: need to parrent to the scene object is dropped in