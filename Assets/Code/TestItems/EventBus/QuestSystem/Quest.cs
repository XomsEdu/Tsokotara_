using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questDescription;

    [SerializeField] private List <QuestGoal> questGoals = new List <QuestGoal>();
    private int completedGoals = 0;

    private bool isQuestComplete = false;

    public void CompleteProgressCheck()
        {
            foreach (var goal in questGoals)
                if (goal.isCompleted)   completedGoals ++;

            if (questGoals.Count >= completedGoals) OnQuestEnd();
        }

    public void OnQuestBegin()
        {
            foreach (var goal in questGoals)
                {
                    goal.Register();
                    goal.papaQuest = this;
                }
        }

    private void OnQuestEnd()
        {
            isQuestComplete = true;
            foreach (var goal in questGoals)
                goal.Unregister();

            // Reward(). Spawn some items in player inv, give some XP, give next quest to questGiver

            Debug.Log("Quest complete!");
        }
}

//Need to rewrite a little requierements for quest completion later
//Also add rewards in maybe like a list
//Also quest needs requierements check to be added, 
// i think for that except simple lvl we can do check for Quests that are marked complete.
// Or in quest rewards we simply grant new quest to questGiver