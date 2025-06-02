using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharachterManager : MonoBehaviour
{
    public Transform rightHand, leftHand, rightFoot, leftFoot, head, spawner;

    public InteractionItem interactionController;
    List<GameObject> itemPool;
    public Action itemSwapped;

    void Awake() //temporal function
        {
            interactionController = rightHand.GetComponentInChildren<InteractionItem>();
            interactionController.spawnerPosition = spawner;
            interactionController.owner = this.gameObject;
        }


    void SpawnItem(Item item)
        {
            foreach (GameObject obj in itemPool)
                {
                    obj.TryGetComponent<InteractionItem>(out InteractionItem interactionItem);
                        if(interactionItem != null && interactionItem.item == item)
                            {
                                obj.SetActive(true);
                                itemPool.Remove(obj);
                                return;
                            }
                }
            GameObject newItem = Instantiate(item.itemGameObject);
            itemSwapped?.Invoke();
            newItem.transform.SetParent(rightHand);
        }

    void DespawnItem()
        {
            GameObject despawnItem = rightHand.GetChild(0).gameObject;
            itemPool.Add(despawnItem);
            despawnItem.SetActive(false);
        }
}
