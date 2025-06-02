using UnityEngine;

public class DroppedItem : MonoBehaviour, IGiveItem
{
    public Item localItem;  public Item item => localItem;
    public int localStack;  public int localCount => localStack;
    public int ID;

    private void Awake() => ID = GetInstanceID();

    private void OnTriggerEnter(Collider other)
        {
            ItemCollector collector = other.GetComponent<ItemCollector>();
            if (collector != null) collector.AddItem(this);


            DroppedItem mergeItem = other.GetComponent<DroppedItem>();
            if (mergeItem != null && mergeItem.item == this.item && mergeItem.ID <= this.ID)
                {
                    mergeItem.localStack += this.localStack;
                    Destroy(transform.parent.gameObject);
                } //Maybe refactor to pulling instead of destroying item
        }

    public void StackReturn(int returnAmount)
        {
            localStack = returnAmount;
            if (localStack <= 0) Destroy(transform.parent.gameObject);
        } //Maybe refactor to pulling instead of destroying item
}

//To add: need to parrent to the scene object is dropped in