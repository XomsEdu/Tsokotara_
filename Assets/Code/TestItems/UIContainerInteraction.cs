using UnityEngine;

public class UIContainerInteraction : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
        {
            ContainerObject container = other.GetComponent<ContainerObject>();
            if (container != null) container.ContainerToCanva();
        }
    
    private void OnTriggerExit(Collider other)
        {
            ContainerObject container = other.GetComponent<ContainerObject>();
            if (container != null) container.CanvaToContainer();
        }
}
