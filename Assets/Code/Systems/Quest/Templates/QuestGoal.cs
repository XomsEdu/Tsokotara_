using System;
using UnityEngine;

[System.Serializable]
public class QuestGoal : GEventListener
{
    [SerializeField] private string description;
    [SerializeField] private UnityEngine.Object requieredData;
    [SerializeField] private int requieredAmount;
    //[SerializeField] private int questStage; //On which of the quest stage this questGoal opens

    [SerializeField] private int currentAmount = 0;
    [NonSerialized] public Quest papaQuest;

    public bool isCompleted()   {   return currentAmount >= requieredAmount;  }

    public override void OnEventRaised(Component sender, object data, int amount)
        {
            if ((UnityEngine.Object) data == requieredData || requieredData == null && amount != 0)
                currentAmount += amount;
            if (isCompleted() || amount < 0) papaQuest.CompleteProgressCheck();
        }
}