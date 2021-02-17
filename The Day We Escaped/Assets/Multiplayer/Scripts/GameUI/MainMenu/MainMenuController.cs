using System;
using System.Collections;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using UnityEngine;
using UnityEngine.Events;

namespace MainMenu
{
    public class MainMenuController : Bolt.GlobalEventListener
    {
        private BoltConfig _config;

        [SerializeField] private bool _isShutingDown;
        [SerializeField] private string _gameLevel;

        public UnityEvent notifyStarting;
        public UnityEvent notifyServerStarting;
        public UnityEvent notifyClientStarting;
        public StringUnityEvent notifyBoltStartFailed;
        public UnityEvent notifyApplicationQuitting;
        
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
            if (BoltNetwork.IsRunning)
            {
                BoltNetwork.Shutdown();
            }
            
            StartCoroutine(StartServerRoutine());
        }

        IEnumerator StartServerRoutine()
        {
            notifyServerStarting?.Invoke();
            notifyStarting?.Invoke();

            while (_isShutingDown)
            {
                yield return null;
            }

            BoltLauncher.StartServer(_config);
        }

        public void StartClient()
        {
            if (BoltNetwork.IsRunning)
            {
                BoltNetwork.Shutdown();
            }
            
            StartCoroutine(StartClientRoutine());
        }

        IEnumerator StartClientRoutine()
        {
            notifyClientStarting?.Invoke();
            notifyStarting?.Invoke();
            
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

        public override void BoltStartFailed(UdpConnectionDisconnectReason disconnectReason)
        {
            var msg = "Fail reason: ";

            switch (disconnectReason)
            {
                case UdpConnectionDisconnectReason.Error:
                    msg += "Not connected";
                    break;
                default:
                    msg += disconnectReason.ToString();
                    break;
            }
            
            var errorEnum = disconnectReason.ToString();
            notifyBoltStartFailed?.Invoke(msg);
        }

        public override void SessionConnectFailed(UdpSession session, IProtocolToken token, UdpSessionError errorReason)
        {
            var errorEnum = errorReason.ToString();
            notifyBoltStartFailed?.Invoke($"Fail reason: {errorEnum}");
        }

        public void QuitApplication()
        {
            notifyApplicationQuitting?.Invoke();
            
            if (BoltNetwork.IsRunning)
            {
                BoltNetwork.Shutdown();
            }
            
            Application.Quit();
        }
    }
}