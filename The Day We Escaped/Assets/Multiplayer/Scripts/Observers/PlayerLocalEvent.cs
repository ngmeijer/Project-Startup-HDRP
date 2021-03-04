using UnityEngine;

namespace Multiplayer.Scripts.Observers
{
    public class PlayerLocalEvent : GameLocalEvent
    {
        public enum EventType
        {
            Attached,
            Detached
        }

        public EventType eventType;
        public GameObject playerGameObject;

        public PlayerLocalEvent(GameObject pPlayerGameObject, EventType evt)
        {
            eventType = evt;
            playerGameObject = pPlayerGameObject;
        }
    }
}