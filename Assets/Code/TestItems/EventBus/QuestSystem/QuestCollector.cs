using System.Collections.Generic;
using UnityEngine;

public class QuestCollector : MonoBehaviour
{
    public List <Quest> questList = new List<Quest>();

    public void AddQuest(Quest quest)
        {   questList.Add(quest);   }
}