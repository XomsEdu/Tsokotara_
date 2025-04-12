using UnityEngine;

[System.Serializable]
public class QuestGoal : GEventListener
{
    [HideInInspector] public Quest papaQuest;
    public string description;
    [SerializeField] private GameObject requieredSenderGO;
    public int requieredAmount;

    public int currentAmount;
    public bool isCompleted { get; private set; } = false; //RO

    public void Register()
        {   gameEvent.RegisterListener(this);   }

    public void Unregister()
        {   gameEvent.UnregisterListener(this); }

    public override void OnEventRaised(Component sender, object data)
        {
            //Debug.Log("Event raised number " + data + " from " + sender);

            GameObject senderGameObject = sender.gameObject;    int? intData = data as int?;
            
            if (senderGameObject = requieredSenderGO)
                {
                    if (intData.HasValue)
                    currentAmount += intData.Value;
                }

            if (currentAmount >= requieredAmount) 
                {
                    isCompleted = true;
                    papaQuest.CompleteProgressCheck();
                }
        }
}