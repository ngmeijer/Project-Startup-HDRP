using Bolt;
using UnityEngine;
using UnityTemplateProjects.PlayerTDEW;

/// <summary>
/// Need refactor
/// </summary>
public class LevelTestCallbackBase : GlobalEventListener
{
    //TODO: refactor everything

    protected BoltEntity InstantiatePlayerAtSpawnPoint(GameObject spawnPoint)
    {
        var player = InstantiatePlayer();

        if (spawnPoint != null)
        {
            SetPlayerPositionAndRotation(player, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }

        return player;
    }
    
    protected BoltEntity InstantiatePlayer(Vector3 pos = default, Quaternion rot = default)
    {
        return BoltNetwork.Instantiate(BoltPrefabs.PlayerTDWERigid, pos, rot);
    }
    
    protected void SetPlayerPositionAndRotation(BoltEntity entity, Vector3 pos, Quaternion rot)
    {
        var playerController = entity.GetComponent<PlayerTDEWController>();
        if (playerController != null)
        {
            playerController.SetPositionAndRotation(pos, rot);
        }
    }
}