using System;
using System.Collections;
using System.Collections.Generic;
using Bolt;
using UnityEngine;
using UnityTemplateProjects.PlayerTDEW;

public class ForceServerPlayerToSpawPoint : Bolt.GlobalEventListener
{
    public Transform spawnPoint;
    public bool destroyThisInBuild = true;

    private void Awake()
    {
#if !UNITY_EDITOR
        if (destroyThisInBuild && BoltNetwork.IsRunning
)
        {
            BoltNetwork.Destroy(this.gameObject);
        }
#endif
    }

    private void Start()
    {
        if (!BoltNetwork.IsServer)
        {
            Destroy(this.gameObject);
        }
    }

    public override void EntityAttached(BoltEntity entity)
    {
        if (entity.StateIs<IPlayerTDWEState>())
        {
            StartCoroutine(ExecuteAtEndOfFrameRoutine(entity));
        }
    }

    IEnumerator ExecuteAtEndOfFrameRoutine(BoltEntity entity)
    {
        yield return new WaitForEndOfFrame();

        var playerCtrl = entity.GetComponent<PlayerTDEWController>();

        if (spawnPoint != null)
            playerCtrl.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }
}