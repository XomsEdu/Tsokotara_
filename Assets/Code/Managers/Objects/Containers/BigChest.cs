using UnityEngine;

public class BigChest : ContainerObject
{
    private void Awake()
        {
            if (CrossSceneRefInit.chestStackStatic != null)
                UIContainer = CrossSceneRefInit.chestStackStatic.transform;

            containerInventory = transform.GetChild(0).gameObject;
        }
}

//Inheritant of container object with it`s own references