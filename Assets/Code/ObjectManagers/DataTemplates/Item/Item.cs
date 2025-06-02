using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Base")]
public class Item : ScriptableObject
    {
        [field: SerializeField] public string itemName { get; private set; }
        [field: SerializeField] public ItemType itemType { get; private set; }
        [field: SerializeField] public GameObject itemGameObject { get; private set; } //Prefub with model and needed scripts
        [field: SerializeField] public Vector3 gripOffset {get; private set;}
        [field: SerializeField] public Sprite imageIcon { get; private set; }
        [field: SerializeField] public int stackSize { get; private set; } = 1;
        [field: SerializeField] public SlotType[] equipableSlots { get; private set; }
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