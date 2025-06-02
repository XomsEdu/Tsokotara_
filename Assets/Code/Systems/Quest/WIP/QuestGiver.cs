using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public QuestCollector player;
    public Quest quest;

    private void Awake()    {   player.AddQuest(quest); }
}
//This needs to be a class to use in other like constuctor