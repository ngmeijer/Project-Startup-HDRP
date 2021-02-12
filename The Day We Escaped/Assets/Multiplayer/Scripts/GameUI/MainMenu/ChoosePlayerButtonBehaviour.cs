using System;
using Bolt;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class ChoosePlayerButtonBehaviour : Bolt.EntityEventListener<IChoosePlayerMenuState>
    {
        public int playerType;

        [SerializeField]
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.PlayerType = playerType;
                state.Enabled = 1;
            }
            else
            {
                _button.interactable = false;
            }
            
            state.AddCallback("Enabled", delegate(IState pState, string pPath, ArrayIndices pIndices)
            {
                if (state.Enabled == -1)
                {
                    var colors = _button.colors;
                    colors.disabledColor = Color.blue;
                    _button.colors = colors;

                    _button.interactable = false;
                }
            });
        }

        public void SendClickEvent()
        {
            var evnt = ChoosePlayerBoltEvent.Create(GlobalTargets.OnlyServer);
            evnt.Entity = this.entity;
            evnt.Send();
        }
    }
}