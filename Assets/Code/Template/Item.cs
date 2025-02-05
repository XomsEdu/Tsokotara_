using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item")]
public class Item : ScriptableObject
{
    
    [Header ("Gameplay")]
    public ItemType type;
    public Rarity rarity;

    [Header ("UI")]
    public string title;
    public Sprite image;
    public int stackSize = 1;


    public enum ItemType {
        MeleeWeapon,
        RangeWeapon,
        Trap,
        Tool,
        Armor,
        Consumable,
        Ammunition,
        Building
    }

    public enum Rarity{
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        OneOfAKind
    }
}
