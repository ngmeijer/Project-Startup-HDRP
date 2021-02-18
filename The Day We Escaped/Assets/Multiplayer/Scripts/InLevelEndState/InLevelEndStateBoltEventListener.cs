using System;
using Bolt;
using UnityEngine;
using UnityTemplateProjects.PlayerTDEW;

namespace Multiplayer.Scripts.InLevelEndState
{
    public class InLevelEndStateBoltEventListener : GlobalEventListener
    {
        /// <summary>
        /// Must be linked in Inspector
        /// </summary>
        [SerializeField]
        private InLevelEndStateController _levelEndCtrl;

        public override void EntityAttached(BoltEntity entity)
        {
            if (entity.StateIs<IPlayerTDWEState>())
            {
                var playerCtrl = entity.GetComponent<PlayerTDEWController>();
                if (playerCtrl != null)
                    _levelEndCtrl.AddPlayer(playerCtrl);
            }
        }

        public override void EntityDetached(BoltEntity entity)
        {
            if (entity.StateIs<IPlayerTDWEState>())
            {
                var playerCtrl = entity.GetComponent<PlayerTDEWController>();
                if (playerCtrl != null)
                    _levelEndCtrl.Remove(playerCtrl);
            }
        }

        /// <summary>
        /// Receive the entity of the player and the val of the lever
        /// And change the State
        /// </summary>
        /// <param name="evnt"></param>
        public override void OnEvent(InLevelEndBoltEvent evnt)
        {
            _levelEndCtrl.state.PlayersAtEnd[evnt.IntVal] = evnt.Entity;
        }
    }
}