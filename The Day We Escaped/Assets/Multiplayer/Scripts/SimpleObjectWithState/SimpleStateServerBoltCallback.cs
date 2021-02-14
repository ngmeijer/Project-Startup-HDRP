using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class SimpleStateServerBoltCallback : Bolt.GlobalEventListener
{
    public override void OnEvent(SimpleIntNextStateBoltEvent evnt)
    {
        if (evnt.Entity == null)
            return;

        var simpleBehaviour = evnt.Entity.GetComponent<SimpleIntStateBehaviour>();
        if (simpleBehaviour == null)
            return;
        
        simpleBehaviour.NextStateInServer();
    }
}
