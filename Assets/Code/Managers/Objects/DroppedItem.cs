using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item; // Reference to the ScriptableObject item
    public int stackSize = 1; // How many items are in this stack

    private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                {
                    InventoryManager inventory = other.GetComponent<InventoryManager>();

                    if (inventory != null)
                        {
                            int remainingStack = stackSize; // Track how many are left to pick up

                            // Try adding as many as possible
                            while (remainingStack > 0 && inventory.AddItem(item))
                                {
                                    remainingStack--;
                                }

                            if (remainingStack == 0)
                                {
                                    Destroy(gameObject); // Picked up everything
                                }
                            else
                                {
                                    stackSize = remainingStack;
                                }
                        }
            }
        }
}
