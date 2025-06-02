using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionItem : MonoBehaviour //needs kids for raycast, projectile spawn and event controlled stuff
{
    public ItemUsable item;
    public ActionManager actionManager;
    [NonSerialized] public Transform spawnerPosition;

    public virtual void SetAction(ComboMove move)
        {
            //for each collider or for raycast to be prepared (instantiated or aimed)
            //set list of status effects from combo move that was passed
            //set item info in order for StatsManager to correctly calculate DMG or whtever
        }

    public virtual void ExecuteAction()
        {
            //Call execution of something we set up in previous function
        }
}

//Custom slot will have transform data of hand in which we place this GO