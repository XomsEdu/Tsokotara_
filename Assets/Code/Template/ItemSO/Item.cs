using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Base")]
public class Item : ScriptableObject
{
    
    [Header ("Item parameters")]
    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private GameObject itemGameObject;
    //GO with components for item (3D model for placable, GO with gun script for gun etc)

    //[SerializeField] private Rarity rarity;
    [field: SerializeField] public Sprite imageIcon { get; private set; }
    [field: SerializeField] public int stackSize { get; private set; } = 1;
    [SerializeField] private SlotType[] equipableSlots;

    /* private enum Rarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary,
            OneOfAKind
        } */
}

public enum ItemType 
    {
        MeleeWeapon,
        RangeWeapon,
        Tool,
        Consumable,
        Plecable,
        Wearable,
        Resource
    }

public enum SlotType
    {
        Head,
        Chest,
        Hand,
        Legs,
        Feet,
        Ring,
        Back,
        Belt
        //etc
    }
//Template for item data