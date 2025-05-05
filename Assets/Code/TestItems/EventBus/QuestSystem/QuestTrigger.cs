using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    [SerializeField] private GEvent questGoal;
    [SerializeField] private int amount = 2;
    public Item item;

    private void OnEnable()
    {
        questGoal.Raise(this, item, amount);
    }

    
}
//This is an examplatory class, it raises event for questGoal to pickup. 
//In future this thing will be placed on different stuff, for example NPC that dies will send event for questGoals to pick 