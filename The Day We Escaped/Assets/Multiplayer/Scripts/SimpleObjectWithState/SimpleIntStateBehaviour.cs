using Bolt;
using UnityEngine;

namespace Multiplayer.Scripts.SimpleObjectWithState
{
    public class SimpleIntStateBehaviour : Bolt.EntityEventListener<ISimpleIntState>
    {
        public int id;
        public int initialState = 0;

        [Tooltip("UnityEvent executed when state changes in server/client, each state number is a index of " +
                 "the array of UnityEvents")]
        public IntUnityEvent[] onStateChanged;

        [Tooltip("UnityEvent array will be executed only in Server/Owner, must if greater than 0 must match the same" +
                 " length of onStateChanged")]
        public IntUnityEvent[] onStateChangedInServer;

        private void Start()
        {
            if (onStateChangedInServer.Length > 0 && onStateChanged.Length != onStateChangedInServer.Length)
            {
                throw new UnityException($"{this} onStateChangedInServer array greater then 0 but lenght mismatch" +
                                         " with onStateChanged");
            }
        }

        public override void Attached()
        {
            if (BoltNetwork.IsServer)
                state.StateNumber = initialState;

            state.AddCallback("StateNumber", () =>
            {
                onStateChanged[state.StateNumber]?.Invoke(state.StateNumber);

                if (!entity.IsOwner) 
                    return;
            
                //Only runs in the owner/server
                //this switch was created by Jetbrains Rider's code hint
                switch (onStateChangedInServer.Length > 0)
                {
                    case true when onStateChanged.Length == onStateChangedInServer.Length:
                        onStateChangedInServer[state.StateNumber]?.Invoke(state.StateNumber);
                        break;
                    case true:
                        throw new UnityException($"{this} onStateChangedInServer array greater then 0 but lenght mismatch" +
                                                 " with onStateChanged");
                }
            });
        }

        public void SetStateInServer(int pStateNumber)
        {
            state.StateNumber = pStateNumber % onStateChanged.Length;
        }

        /// <summary>
        /// Send event to server, runs in clients and server player
        /// </summary>
        public void SendNextStateBoltEvent()
        {
            var evnt = SimpleIntNextStateBoltEvent.Create(GlobalTargets.OnlyServer);
            evnt.Entity = this.entity;
            evnt.Send();
        }

        /// <summary>
        /// Commonly executed by SimpleStateServerBoltCallback after receive bolt event 
        /// </summary>
        public void NextStateInServer()
        {
            if (onStateChanged.Length > 0)
                state.StateNumber = (state.StateNumber + 1) % onStateChanged.Length;
        }
    }
}