using System.Collections;
using Bolt;
using Bolt.Matchmaking;
using UnityEngine;
using UnityEngine.Events;

public class ConfigPlayerOwnerCallbackBase : Bolt.GlobalEventListener
{
    public UnityEvent notify;

    public StringUnityEvent notifySceneLoadedWithSessionGuid;

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

    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        var guid = BoltMatchmaking.CurrentSession.Id.ToString();
        var shortGuid = guid.Substring(guid.Length - 5, 5);

        notifySceneLoadedWithSessionGuid?.Invoke($"Host: {shortGuid}");
    }
}