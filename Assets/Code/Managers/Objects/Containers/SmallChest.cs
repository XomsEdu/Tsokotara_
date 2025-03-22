using UnityEngine;

public class SmallChest : ContainerObject
{
    private void Awake()
        {
            if (CrossSceneRefInit.smallFillerStatic != null)
                UIContainer = CrossSceneRefInit.smallFillerStatic.transform;

            containerInventory = transform.GetChild(0).gameObject;
        }
}

//Inheritant of container object with it`s own references