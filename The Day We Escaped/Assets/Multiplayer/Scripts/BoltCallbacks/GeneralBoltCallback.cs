using System.Collections;
using System.Collections.Generic;
using Bolt;
using Multiplayer.Scripts.Audio;
using UnityEngine;
using UnityEngine.Audio;
using UnityTemplateProjects.PlayerTDEW;

[BoltGlobalBehaviour()]
public class GeneralBoltCallback : Bolt.GlobalEventListener
{
   private Dictionary<BoltConnection, PlayerTDEWController> _playerMap;

   private void Awake()
   {
      _playerMap = new Dictionary<BoltConnection, PlayerTDEWController>();
   }
   
   public override void SceneLoadLocalDone(string scene, IProtocolToken token)
   {
      StartCoroutine(MuteSfxsForSeconds(5));
   }

   private IEnumerator MuteSfxsForSeconds(float duration)
   {
      //AudioMixerSingleton.instance.MuteSfx();
      
      yield return new WaitForSeconds(duration);
      
      //AudioMixerSingleton.instance.UnMuteSfx();
   }

   public override void EntityAttached(BoltEntity entity)
   {
      if (entity.StateIs<IPlayerTDWEState>())
      {
         var player = entity.GetComponent<PlayerTDEWController>();
         if (player == null)
            return;
      }
   }

   public override void Connected(BoltConnection connection)
   {
      if (!_playerMap.ContainsKey(connection))
      {
         _playerMap.Add(connection, null);
      }
      BoltLog.Info($"{this} <== {connection} connect to this instance");
   }
}
