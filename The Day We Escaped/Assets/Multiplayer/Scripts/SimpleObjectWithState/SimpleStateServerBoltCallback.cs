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
        {
            throw new UnityException($"Entity received in {nameof(SimpleStateServerBoltCallback)} is NULL");
            return;
        }

        simpleBehaviour.NextStateInServer();
    }
}
