using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/Item/Animated")]
public partial class ItemAnimated : Item
{
    [SerializeField] private AnimatorOverrideController animationSet;
    //We need some kind of audioClips list here, not decided yet on how to use it in animation clip system
}