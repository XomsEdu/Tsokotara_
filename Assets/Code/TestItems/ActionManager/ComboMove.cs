using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/ComboMove")]
public class ComboMove : ScriptableObject
{
    public ComboMove requieredMove;
    public List <ActionsInput> requieredInputActions;
    public AnimationClip animationClip;
    public float comboResetTime;

    private void OnEnable()
        {   if (requieredInputActions != null) requieredInputActions.Sort();    }

}
