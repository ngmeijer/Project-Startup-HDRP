using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour()]
public class FindPlayersBoltCallback : Bolt.GlobalEventListener, ISubjectCB
{
    private List<GameObject> _playerList = new List<GameObject>();
    private List<IObserverCB> _networkPlayerObserver = new List<IObserverCB>();

    public int State { get; set; }

    public override void EntityAttached(BoltEntity entity)
    {
        if (entity.StateIs<IPlayerTDWEState>())
        {
            _playerList.Add(entity.gameObject);
            UpdateNPCObservers();
        }
    }

    public override void EntityDetached(BoltEntity entity)
    {
        if (entity.StateIs<IPlayerTDWEState>())
        {
            _playerList.Remove(entity.gameObject);
            UpdateNPCObservers();
        }
    }

    public List<GameObject> ReturnPlayerList()
    {
        return _playerList;
    }

    public void AttachPlayerNetwork(IObserverCB pObserver)
    {
        _networkPlayerObserver.Add(pObserver);
    }

    public void DetachPlayerNetwork(IObserverCB pObserver)
    {
        _networkPlayerObserver.Remove(pObserver);
    }

    public void UpdateNPCObservers()
    {
        foreach (IObserverCB observer in _networkPlayerObserver)
        {
            observer.ReceiveNetworkUpdate(_playerList);
        }
    }
}
