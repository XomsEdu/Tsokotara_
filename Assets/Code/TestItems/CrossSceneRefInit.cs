using UnityEngine;

public class GlobalReferenceHolder : MonoBehaviour
{
    public static GlobalReferenceHolder instance {get; private set;}

    public static GameObject chestStackStatic;  public GameObject chestStack;
    public static GameObject smallFillerStatic; public GameObject smallFiller;
    public static GameObject playerBaseStatic;

    private void Awake()
        {
            instance = this;
            chestStackStatic = chestStack;
            smallFillerStatic = smallFiller;
        }
}


//Reference manager for cross scene requiered references. Rn it stores particular UI refs, to be modified later