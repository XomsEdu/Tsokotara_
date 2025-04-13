using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestStatus questStatus;
    public string questDescription;

    [SerializeField] private List <QuestGoal> questGoals = new List <QuestGoal>();

    public void OnQuestRegister()
        {
            //Debug.Log("Quest goals to complete: " + questGoals.Count);

            foreach (var goal in questGoals)
                {   goal.Register();    goal.papaQuest = this;  }
        }

    public void CompleteProgressCheck()
        {
            int completedGoals = 0;

            foreach (var goal in questGoals)
                if (goal.isCompleted()) completedGoals ++;

            if (completedGoals >= questGoals.Count) OnQuestEnd();
        }

    private void OnQuestEnd()
        {
            foreach (var goal in questGoals)
                goal.Unregister();
            
            questStatus = QuestStatus.Complete;

            // Reward(). Spawn some items in player inv, give some XP, give next quest to questGiver

            Debug.Log("Quest complete!");
        }

    // private void OnQuestFailed()
}

//Need to add rewards in maybe like a list
//Also quest needs requierements check to be added, 
// i think for that except simple lvl we can do check for Quests that are marked complete.
// Or in quest rewards we simply grant new quest to questGiver
// For questGiver to have new quest we gonna need some sort of list
// with assigned new quests and respective questGiver to send it to.
// Also we gonna need assign not only questGiver, but dialouge choice that will unlock the quest.

public enum QuestStatus
{
    InProgress,
    Complete,
    Failed //if quest was failed load save or speak with giver to get it again
}

//Need additional check of the inventory on quest begining 
// (probably just invoke every inventory item event like once)