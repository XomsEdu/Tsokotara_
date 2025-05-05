using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public TMP_Text countText;
    public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem (Item newItem, int startingCount = 1)
        {
            item = newItem;
            image.sprite = newItem.imageIcon;
            count = startingCount;
            RefreshCount();
        }

    public void RefreshCount()
        {
            countText.text = count.ToString();
            countText.gameObject.SetActive(count > 1);
        }

    public void OnBeginDrag(PointerEventData eventData)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            image.raycastTarget = false;
        }

    public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

    public void OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(parentAfterDrag);
            image.raycastTarget = true;
        }
}

//UI managment of item in inventory