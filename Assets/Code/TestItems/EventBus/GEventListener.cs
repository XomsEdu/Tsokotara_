using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
public class CustomGameEvent : UnityEvent <Component, object, int> {}

[System.Serializable]
public class GEventListener
{
    public GameEvent gameEvent;
    public CustomGameEvent response;

    public void Register()      {   gameEvent.RegisterListener(this);   }
    public void Unregister()    {   gameEvent.UnregisterListener(this); }

    public virtual void OnEventRaised(Component sender, object data, int amount)
        {   
            //response.Invoke(sender, data, amount);
        }
}

//This parental class is to be added and used in other MonoBehaviour scripts

/* Invoker works with GameEvent like this:

    gameEvent.Raise(this, dataToSend);
    Attach public GameEvent; (scriptable object)

    OnEnable of listener, GameEventListener calls GameEvent.RegisterListener in Register()
*/
