using MainMenu;
using UnityEngine;
using UnityEngine.Events;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "MainMenu_ChoosePlayer")]
public class ChoosePlayerButtonServerEventListener : Bolt.GlobalEventListener
{
    public override void OnEvent(ChoosePlayerBoltEvent evnt)
    {
        BoltLog.Info($"{this.name}: receive event from {evnt.Entity}" );

        var chooseState = evnt.Entity.GetState<IChoosePlayerMenuState>();
        chooseState.Enabled = -1;
        var playerType = chooseState.PlayerType;
        
        GameObject.FindObjectOfType<ChoosePlayerMenuController>().LoadPlayerScene(playerType);
    }
}