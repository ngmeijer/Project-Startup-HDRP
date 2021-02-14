using Bolt;
using UnityEngine;
using UnityTemplateProjects.CustomUnityEvents;

public class SimpleIntStateBehaviour : Bolt.EntityEventListener<ISimpleIntState>
{
    public int id;
    public int initialState = 0;
    public IntUnityEvent[] notifier;
    
    public override void Attached()
    {
        if (BoltNetwork.IsServer)
            state.StateNumber = initialState;
        
        state.AddCallback("StateNumber", () =>
        {
            notifier[state.StateNumber]?.Invoke(state.StateNumber);
        });
    }
    
    public void SetStateInServer(int pStateNumber)
    {
        state.StateNumber = pStateNumber % notifier.Length;
    }

    /// <summary>
    /// Send event to server
    /// </summary>
    public void SendNextStateBoltEvent()
    {
        var evnt = SimpleIntNextStateBoltEvent.Create(GlobalTargets.OnlyServer);
        evnt.Entity = this.entity;
        evnt.Send();
    }
    
    /// <summary>
    /// Commonly executed by SimpleStateServerBoltCallback after receive bolt event 
    /// </summary>
    public void NextStateInServer()
    {
        state.StateNumber = (state.StateNumber + 1) % notifier.Length;
    }
}