using Bolt;
using UnityEngine;
using UnityEngine.Events;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class InGameButtonServerEventListener : Bolt.GlobalEventListener
{
    public override void OnEvent(InGameButtonBoltEvent evnt)
    {
        if (evnt.Entity != null)
        {
            var button = evnt.Entity.GetComponent<InGameButtonBehavior>();
            button.SetNextStateInServer();
        }
    }
}