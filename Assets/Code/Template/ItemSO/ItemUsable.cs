using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Usable")]
public partial class ItemUsable : ItemAnimated
{
    [SerializeField] private float strength; //abstract for damage or effect strength
    [SerializeField] private float tempo; //abstract for firing speed or potion effect time, or cooldown etc.
    [SerializeField] private float speed; //abstract for projectile speed or other parameters to use
    [SerializeField] private bool singleActionUse; //abstract bool for single action guns or for consumables
    //[SerializeField] private List<StatusEffect> statusEffects;
}