using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Consuming")]
public partial class ItemResourceFed : ItemUsable
{
    [SerializeField] private List<Item> fedResource; //Ammo type etc.
    [SerializeField] private int resourceCapacity; //Rounds in chamber etc.
}