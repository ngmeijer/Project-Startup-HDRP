using UnityEngine;

namespace Multiplayer.Scripts.Observers
{
    public class EnemyLocalEvent : GameLocalEvent
    {
        public enum EventType
        {
            Attached,
            Detached
        }

        public EventType eventType;
        public GameObject enemyGameObject;

        public EnemyLocalEvent(GameObject pEnemyGameObject, EventType evt)
        {
            eventType = evt;
            enemyGameObject = pEnemyGameObject;
        }
    }
}