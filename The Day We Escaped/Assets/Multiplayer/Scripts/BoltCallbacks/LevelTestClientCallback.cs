using Bolt;
using UnityEngine;


[BoltGlobalBehaviour(BoltNetworkModes.Client, "Level Test", "MovementTestScene", "Nils - Level 1")]
public class LevelTestClientCallback : LevelTestCallbackBase
{
    /// <summary>
    /// After scene is loaded, creates the Client Player, this player type will be different from the server player
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="token"></param>
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        var serverPlayerToken = token as IntBoltToken;
        int playerType = 2;
        if (serverPlayerToken == null)
        {
            BoltLog.Warn("Null SceneLoadLocalDone token received");
        }
        else
        {
            playerType = serverPlayerToken.intVal;
        }

        var spawnPosition = Vector3.zero;
        GameObject spawnPoint;
        
        BoltLog.Warn($"{this} Client | playerType: " + playerType);
        
        //playerType is the type of the server player, the client gets the other one
        //Example: if Server Player get the Morgue(type 2), Client will get the Cell (type 1)
        if (playerType == 1)
        {
            //Instantiate player at Morgue
            spawnPoint = GameObject.Find("SpawnPoint Morgue");
        }
        else
        {
            //Instantiate player at Cell
            spawnPoint = GameObject.Find("SpawnPoint Cell");
        }

        var clientPlayer = InstantiatePlayerAtSpawnPoint(spawnPoint);
    }
}