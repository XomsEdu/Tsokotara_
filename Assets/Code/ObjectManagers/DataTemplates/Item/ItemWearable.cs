using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Wearable")]
public partial class ItemWearable : Item
{
    [field: SerializeField] public Dictionary<PassiveEffect, float> passiveEffects {get; private set;}
}


public enum PassiveEffect
{
    protection,
    passiveHeal,
    strengthBuff,
    speedBuff,
    jumpBuff,
    jumpAddition
    //etc
}