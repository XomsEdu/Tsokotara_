using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCallPassing : MonoBehaviour
{
    ActionManager actionManager;
    void Start() => actionManager = GetComponentInParent<ActionManager>();
    public void AnimTrigger() => actionManager.AnimationEventTrigger();
}
