using System.Collections.Generic;
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
    [SerializeField] private SlotType[] usableSlots;


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

[CreateAssetMenu (menuName = "ScriptableObject/Item/Animated")]
public partial class ItemAnimated : Item
{
    [SerializeField] private AnimatorOverrideController animationSet;
    //We need some kind of audioClips list here, not decided yet on how to use it in animation clip system
}

[CreateAssetMenu (menuName = "ScriptableObject/Item/Usable")]
public partial class ItemUsable : ItemAnimated
{
    [SerializeField] private float strength; //abstract for damage or effect strength
    [SerializeField] private float tempo; //abstract for firing speed or potion effect time, or cooldown etc.
    [SerializeField] private float speed; //abstract for projectile speed or other parameters to use
    [SerializeField] private bool singleActionUse; //abstract bool for single action guns or for consumables
    //[SerializeField] private List<StatusEffect> statusEffects;
}

[CreateAssetMenu (menuName = "ScriptableObject/Item/Consuming")]
public partial class ItemResourceFed : ItemUsable
{
    [SerializeField] private List<Item> fedResource; //Ammo type etc.
    [SerializeField] private int resourceStore; //Rounds in chamber etc.
}

[CreateAssetMenu (menuName = "ScriptableObject/Item/Wearable")]
public partial class ItemWearable : Item
{
    [SerializeField] private float strength; //abstract for defence units or passive effect strength etc
    //[SerializeField] private List<StatusEffect> passiveEffects; 
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