public class Chest : ContainerObject
{   

    protected override void Start()
        {
            int inventorySize = containerUI.transform.childCount;

            if (inventorySize <= 9)
                typeUI = UIType.SmallChest;
            else
                typeUI = UIType.BigChest;

            base.Start();
        }
}

//Inheritant of container object with it`s own references