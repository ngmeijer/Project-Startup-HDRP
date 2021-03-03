using Bolt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Multiplayer.Scripts.GameUI.MainMenu
{
    public class ChoosePlayerButtonBehaviour : Bolt.EntityEventListener<IChoosePlayerMenuState>
    {
        public int playerType;

        [SerializeField]
        private Button _button;

        public UnityEvent notifyServerLoadingScene;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.interactable = false;
        }

        public override void Attached()
        {
            if (entity.IsOwner)
            {
                state.PlayerType = playerType;
                state.Enabled = 1;
                _button.interactable = true;
            }

            state.AddCallback("Enabled", delegate(IState pState, string pPath, ArrayIndices pIndices)
            {
                if (state.Enabled == -1)
                {
                    var colors = _button.colors;
                    //colors.disabledColor = Color.blue;
                    _button.colors = colors;

                    _button.interactable = false;

                    notifyServerLoadingScene?.Invoke();
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