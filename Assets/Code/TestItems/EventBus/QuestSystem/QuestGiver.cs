using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public QuestCollector player;
    public Quest quest;

    private void Awake()
        {
            quest.OnQuestRegister();
            player.AddQuest(quest);
        }
}