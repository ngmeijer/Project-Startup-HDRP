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
    
    public void NextStateInServer()
    {
        state.StateNumber = (state.StateNumber + 1) % notifier.Length;
    }
}