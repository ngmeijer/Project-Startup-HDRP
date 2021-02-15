using System.Collections;
using System.Collections.Generic;
using Bolt;
using Multiplayer.Scripts.Audio;
using UnityEngine;
using UnityEngine.Audio;

[BoltGlobalBehaviour()]
public class GeneralBoltCallback : Bolt.GlobalEventListener
{
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
}
