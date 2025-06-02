using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Usable")]
public partial class ItemUsable : Item
{
    [field: SerializeField] public float strength {get; private set;} //abstract for damage or effect strength
    [field: SerializeField] public float speed {get; private set;} //abstract for projectile speed or other parameters to use
    [field: SerializeField] public List<ComboMove> comboMoves {get; private set;} = new();
    [field: SerializeField] public Vector3 barrelOffset {get; private set;}
}