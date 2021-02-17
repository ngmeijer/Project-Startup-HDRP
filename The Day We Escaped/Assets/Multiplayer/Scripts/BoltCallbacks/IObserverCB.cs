using System.Collections.Generic;
using UnityEngine;

public interface IObserverCB
{
    void ReceiveNetworkUpdate(List<GameObject> pPlayerList);
}