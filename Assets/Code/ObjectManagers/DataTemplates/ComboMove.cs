using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/ComboMove")]
public class ComboMove : ScriptableObject
{
    [field: SerializeField] public MoveType moveType {get; private set;}
    [field: SerializeField] public bool getsInterrupted {get; private set;} = true;
    [field: SerializeField] public ComboMove requiredMove {get; private set;} //probably will de a list in future
    [field: SerializeField] public List <ActionsInput> requieredInputActions {get; private set;} = new();
    [field: SerializeField] public AnimationClip animationClip {get; private set;}
    [field: SerializeField] public List<StatusEffect> statusEffects {get; private set;} = new();
    [field: SerializeField] public float resetComboTimer {get; private set;} = 1f;
    [field: SerializeField] public GameObject projectile {get; private set;}
    
    private void OnEnable()
        {   if (requieredInputActions != null) requieredInputActions.Sort();    }
}

public enum MoveType
{
    Tap,
    Hold
}