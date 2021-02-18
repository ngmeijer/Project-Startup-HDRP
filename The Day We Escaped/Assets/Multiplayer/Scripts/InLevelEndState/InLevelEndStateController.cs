using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Run in both Server and Player
/// States are in a SimpleIntState entity
/// </summary>
public class InLevelEndStateController : Bolt.EntityBehaviour<ISimpleIntState>
{
    public int totalPlayerToEscape;

    public float endLevelNotifyDelay;
    
    public UnityEvent notifyEndLevel;
    public UnityEvent notifyAfterEndLevelDelay;

    
    public override void Attached()
    {
        state.AddCallback("StateNumber", () =>
        {
            BoltLog.Warn($"Player to escape set | total: {state.StateNumber}");
            CheckPlayerAtEnd();
        });
    }

    private void CheckPlayerAtEnd()
    {
        if (state.StateNumber >= totalPlayerToEscape)
        {
            BoltLog.Warn($"Players Escaped");
            
            StartCoroutine(NotifyEndLevelDelay());
            notifyEndLevel?.Invoke();
        }
    }

    private IEnumerator NotifyEndLevelDelay()
    {
        yield return new WaitForSeconds(endLevelNotifyDelay);
        
        notifyAfterEndLevelDelay?.Invoke();

    }

    public void AddPlayerCountInServer()
    {
        state.StateNumber++;
        BoltLog.Warn($"Player to escape added | total: {state.StateNumber}");
    }

    public void RemovePlayerCountInServer()
    {
        state.StateNumber--;
        BoltLog.Warn($"Player to escape removed | total: {state.StateNumber}");
    }
}