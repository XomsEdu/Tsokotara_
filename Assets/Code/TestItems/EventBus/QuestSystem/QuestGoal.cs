using UnityEngine;

[System.Serializable]
public class QuestGoal : GEventListener
{
    [SerializeField] private string description;
    [SerializeField] private Object requieredData;
    [SerializeField] private int requieredAmount;
    [SerializeField] private int questStage; //On which of the quest stage this quest opens

    [SerializeField] private int currentAmount = 0;
    [HideInInspector] public Quest papaQuest;

    public bool isCompleted()   {   return currentAmount >= requieredAmount;  }

    public override void OnEventRaised(Component sender, object data, int amount)
        {
            //Debug.Log("Event raised number " + data + " from " + sender);

            if ((Object) data == requieredData || requieredData == null && amount != 0)
                currentAmount += amount;
            if (isCompleted() || amount < 0) papaQuest.CompleteProgressCheck();
        }
}