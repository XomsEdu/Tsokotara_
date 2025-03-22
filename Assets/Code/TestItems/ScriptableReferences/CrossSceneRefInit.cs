using UnityEngine;

public class CrossSceneRefInit : MonoBehaviour
{
    public static GameObject chestStackStatic;  public GameObject chestStack;

    public static GameObject smallFillerStatic; public GameObject smallFiller;

    private void Awake()
        {
            chestStackStatic = chestStack;
            smallFillerStatic = smallFiller;
        }
}


//Reference manager for cross scene requiered references. Rn it stores particular UI refs, to be modified later