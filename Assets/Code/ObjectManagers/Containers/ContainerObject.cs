using UnityEngine;

public class ContainerObject : MonoBehaviour
{
    protected Transform UIDestination;
    protected GameObject containerUI;
    protected UIType typeUI;

    private void Awake()    {containerUI = transform.GetChild(0).gameObject;}

    protected virtual void Start()
        {
            switch (typeUI)
                {
                    case UIType.SmallChest:
                        if (GlobalReferenceHolder.smallFillerStatic != null)
                            UIDestination = GlobalReferenceHolder.smallFillerStatic.transform;
                        break;

                    case UIType.BigChest:
                        if (GlobalReferenceHolder.chestStackStatic != null)
                            UIDestination = GlobalReferenceHolder.chestStackStatic.transform;
                        break;

                    //case UIType.Dialouge:
                }
        }

    public void ContainerToCanva()
        {
            containerUI.transform.SetParent(UIDestination);
            containerUI.transform.localScale = Vector3.one;
            containerUI.SetActive(true);
        }

    public void CanvaToContainer()
        {
            containerUI.SetActive(false);
            containerUI.transform.SetParent(transform);
        }
}

//Class for UI holders to move UI inventories (not only inventories btw). Basically UI puller

public enum UIType
{
    SmallChest,
    BigChest,
    Dialouge
}