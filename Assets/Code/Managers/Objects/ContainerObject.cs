using UnityEngine;
using System.Collections;

public class ContainerObject : MonoBehaviour
{

    private GameObject containerInventory;
    public Transform UIContainer;
    public float UITransferDelay = 1f;

    private void Awake()
        {   containerInventory = transform.GetChild(0).gameObject;  }

    public void InventoryToCanva()
        {
            StopAllCoroutines();
            containerInventory.transform.SetParent(UIContainer);
            containerInventory.transform.localScale = Vector3.one;
            containerInventory.SetActive(true);
        }

    public void CanvaToInventory()
        {
            StopAllCoroutines();
            StartCoroutine(HideInventoryAfterTime(UITransferDelay));
        }

    private IEnumerator HideInventoryAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            containerInventory.SetActive(false);
            containerInventory.transform.SetParent(transform);
        }
}

//Class for UI holders to move UI inventories