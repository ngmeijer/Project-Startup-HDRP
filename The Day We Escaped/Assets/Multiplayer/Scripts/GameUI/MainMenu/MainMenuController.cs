using System;
using System.Collections;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuController : Bolt.GlobalEventListener
    {
        private BoltConfig _config;

        [SerializeField] private bool _isShutingDown;

        [SerializeField] private string _gameLevel;

        private void Start()
        {
            _config = BoltRuntimeSettings.instance.GetConfigCopy();

            if (BoltNetwork.IsRunning)
            {
                BoltNetwork.Shutdown();
            }
        }

        public void StartServer()
        {
            StartCoroutine(StartServerRoutine());
        }

        IEnumerator StartServerRoutine()
        {
            while (_isShutingDown)
            {
                yield return null;
            }

            BoltLauncher.StartServer(_config);
        }

        public void StartClient()
        {
            StartCoroutine(StartClientRoutine());
        }

        IEnumerator StartClientRoutine()
        {
            while (_isShutingDown)
            {
                yield return null;
            }

            BoltLauncher.StartClient(_config);
        }

        public override void BoltStartDone()
        {
            if (BoltNetwork.IsServer)
            {
                var id = Guid.NewGuid().ToString().Split('-')[0];
                var matchName = string.Format("{0} - {1}", id, _gameLevel);

                BoltMatchmaking.CreateSession(
                    sessionID: matchName,
                    sceneToLoad: _gameLevel
                );
            }
            else if (BoltNetwork.IsClient)
            {
                BoltMatchmaking.JoinRandomSession();
            }
        }

        public override void BoltShutdownBegin(AddCallback registerDoneCallback,
            UdpConnectionDisconnectReason disconnectReason)
        {
            _isShutingDown = true;
            registerDoneCallback(() =>
            {
                _isShutingDown = false;

            });
        }
    }
}