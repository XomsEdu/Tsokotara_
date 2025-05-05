using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Wearable")]
public partial class ItemWearable : Item
{
    [SerializeField] private float strength; //abstract for defence units or passive effect strength etc
    //[SerializeField] private List<StatusEffect> passiveEffects; 
}