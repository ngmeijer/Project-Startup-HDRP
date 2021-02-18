using Bolt;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityTemplateProjects.PlayerTDEW;

namespace Multiplayer.Scripts.InLevelEndState
{
    /// <summary>
    /// This was created because UnityEvents cannot have 2 parameter in the callback function
    /// So this one holds the val (int) and get the player entity from the PointerEventData camera
    ///
    /// This will send a event with the Entity(player) and the lever int
    /// Resuming: will send a event with the player that pressed the lever ans the lever number
    /// </summary>
    public class InLevelEndStateBoltEventSender : MonoBehaviour
    {
        [Header("The Lever trigger will call SendInLevelEndBoltEvent(pointerEventData)")]
        
        public int leverInt;

        public void SendInLevelEndBoltEvent(PointerEventData pointerEventData)
        {
            //Try get player entity, its happens only in the owner of the player instance
            if (pointerEventData.enterEventCamera == null)
                return;

            var playerCtrl = pointerEventData.enterEventCamera.GetComponentInParent<PlayerTDEWController>();
            if (playerCtrl == null)
                return;

            if (playerCtrl.entity == null)
                return;
            
            var entity = playerCtrl.entity;

            var evtn = InLevelEndBoltEvent.Create(GlobalTargets.OnlyServer);
            evtn.Entity = entity;
            evtn.IntVal = leverInt;
            evtn.Send();
        }
    }
}