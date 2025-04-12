using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Consuming")]
public partial class ItemResourceFed : ItemUsable
{
    [SerializeField] private List<Item> fedResource; //Ammo type etc.
    [SerializeField] private int resourceStore; //Rounds in chamber etc.
}