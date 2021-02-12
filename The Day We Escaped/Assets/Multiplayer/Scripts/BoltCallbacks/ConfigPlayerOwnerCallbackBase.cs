using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ConfigPlayerOwnerCallbackBase : Bolt.GlobalEventListener
{
    public UnityEvent notify;
    
    public override void EntityAttached(BoltEntity entity)
    {
        if (entity.StateIs<IPlayerTDWEState>() && entity.IsOwner)
        {
            StartCoroutine(ExecuteAtEndOfFrame());
        }
    }

    private IEnumerator ExecuteAtEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        
        notify?.Invoke();
    }
}