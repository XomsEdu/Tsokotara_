using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent <Component, object> {}
public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public CustomGameEvent response;

    private void OnEnable()
        {   gameEvent.RegisterListener(this);   }

    private void OnDisable()
        {   gameEvent.UnregisterListener(this); }

    public void OnEventRaised(Component sender, object data)
        {   response.Invoke(sender, data);  }
}

/* Invoker works with GameEvent like this:

    onGEInvoked.Raise(this, dataToSend);

    Attach public GameEvent; (scriptable object) 
    and on Enable (read subscription) of listener GameEventListener calls GameEvent.RegisterListener.
    Listener object will work something like:
    private void UpdateData(Component sender, object data)
        {
            if (sender is player) or if (sender is Health) or if (data is int)
                {
                    SetData(data);
                }
        }

    private void SetData(int health)
        {
            healthText.text = health.ToString();
        }

*/

//This script will majoraly be used to code questStep class that will listen to events so basically
//we need to write it inheritantly from this class