using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Use LocalEvents for AddListeners
/// </summary>
public class GameLocalEvent
{

}

public class LocalEvents
{
    private static LocalEvents eventsInstance = null;

    public static LocalEvents instance
    {
        get
        {
            if (eventsInstance == null)
            {
                eventsInstance = new LocalEvents ();
            }

            return eventsInstance;
        }
    }

    public delegate void EventDelegate<T> (T e) where T : GameLocalEvent;

    private Dictionary<System.Type, System.Delegate> delegates = new Dictionary<System.Type, System.Delegate> ();

    public void AddListener<T> (EventDelegate<T> del) where T : GameLocalEvent
    {
        if (delegates.ContainsKey (typeof (T)))
        {
            System.Delegate tempDel = delegates[typeof (T)];

            delegates[typeof (T)] = System.Delegate.Combine (tempDel, del);
        }
        else
        {
            delegates[typeof (T)] = del;
        }
    }

    public void RemoveListener<T> (EventDelegate<T> del) where T : GameLocalEvent
    {
        if (delegates.ContainsKey (typeof (T)))
        {
            var currentDel = System.Delegate.Remove (delegates[typeof (T)], del);

            if (currentDel == null)
            {
                delegates.Remove (typeof (T));
            }
            else
            {
                delegates[typeof (T)] = currentDel;
            }
        }
    }

    public void Raise (GameLocalEvent e)
    {
        if (e == null)
        {
            return;
        }

        if (delegates.ContainsKey (e.GetType ()))
        {
            delegates[e.GetType ()].DynamicInvoke (e);
        }
    }
}