using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent questGoal;
    [SerializeField] private int amount = 2;

    private void OnEnable()
    {
        questGoal.Raise(this, amount);
    }

    
}
//This is an examplatory class, it raises event for questGoal to pickup. 
//In future this thing will be placed on different stuff, for example NPC that dies will send event for questGoals to pick 