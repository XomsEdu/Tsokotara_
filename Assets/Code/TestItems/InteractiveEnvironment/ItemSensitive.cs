using System.Collections.Generic;
using UnityEngine;

public class ItemSensitive : MonoBehaviour
{
    [SerializeField] Item sensitiveItem;
    [SerializeField] List<GameObject> awakeObjects = new();

    void OnCollisionEnter(Collision collision)
        {
            DroppedItem droppedItem = collision.gameObject.GetComponentInChildren<DroppedItem>();
            if(droppedItem != null)
                {
                    if (sensitiveItem = droppedItem.item) this.gameObject.SetActive(false);
                }
        }

    void OnDisable()
        {
            foreach (GameObject obj in awakeObjects)
                {   if(obj != null) obj.SetActive(true);    }
        }

}
