using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    [System.NonSerialized] public List<GEventListener> listeners = new List<GEventListener>();
    //List in Scriptable object can`t serialize MonoBehaviour classes and GameObjects
    // to avoid problems we say not to serialize list either

    public void Raise(Component sender, object data, int amount)
        {
            foreach (var listener in listeners)
                Debug.Log("Raising event for: " + listener); 
            // This will log each listener in the console


            for (int i = 0; i < listeners.Count; i++)
                listeners[i].OnEventRaised(sender, data, amount);
        }

    public void RegisterListener(GEventListener listener)
        {
            if(!listeners.Contains(listener))   listeners.Add(listener);
        }

    public void UnregisterListener(GEventListener listener)
        {
            if(listeners.Contains(listener))   listeners.Remove(listener);
            Debug.Log("Something was removed");
        }
    
}

// This SO is basically event itself that subscribes it`s listeners on their call automatically