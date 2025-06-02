using UnityEngine;

public class ContainerInteraction : MonoBehaviour
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
