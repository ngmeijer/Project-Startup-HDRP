using System;
using System.Collections;
using System.Linq;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;
using UdpKit.Platform;
using UdpKit.Platform.Photon;
using UnityEngine;
using UnityEngine.Events;

namespace Multiplayer.Scripts.GameUI.MainMenu
{
    public class MainMenuController : Bolt.GlobalEventListener
    {
        [SerializeField] private bool _isShutingDown;
        [SerializeField] private string _gameLevel;

        public UnityEvent notifyStarting;
        
        [Header("Server Start Notify")]
        public UnityEvent notifyServerStarting;
        public UnityEvent notifyServerStarted;
        
        [Header("Client Start Notify")]
        public UnityEvent notifyClientStarting;
        public UnityEvent notifyClientStarted;
        public UnityEvent notifyClientConnectToSession;
        public StringUnityEvent notifyBoltStartFailed;
        public UnityEvent notifyApplicationQuitting;

        public StringUnityEvent notifySessionCreatedOrUpdated;

        private void Start()
        {
            BoltLauncher.SetUdpPlatform(new PhotonPlatform());

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

            BoltLauncher.StartServer();
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

            BoltLauncher.StartClient();
        }

        public override void BoltStartDone()
        {
            if (BoltNetwork.IsServer)
            {
                var id = Guid.NewGuid().ToString().Split('-')[0];
                var matchName = string.Format("{0}", id);

                BoltMatchmaking.CreateSession(matchName, null, null);

                notifyServerStarted?.Invoke();
            }
            else if (BoltNetwork.IsClient)
            {
                notifyClientStarted?.Invoke();
            }
        }

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);
        }

        public override void SessionCreatedOrUpdated(UdpSession session)
        {
            Debug.LogWarning($"SessionCreatedOrUpdated: {session.Id} | {session.HostName}");

            var guidStr = session.Id.ToString();
            var shortGuid = guidStr.Substring(guidStr.Length - 5, 5);
            
            notifySessionCreatedOrUpdated?.Invoke("Host: " + shortGuid);
        }

        public override void SessionCreationFailed(UdpSession session, UdpSessionError errorReason)
        {
            Debug.LogError($"SessionCreationFailed: {session.Id} | {session.HostName} | {errorReason}");
        }

        public override void BoltShutdownBegin(AddCallback registerDoneCallback,
            UdpConnectionDisconnectReason disconnectReason)
        {
            _isShutingDown = true;
            registerDoneCallback(() => { _isShutingDown = false; });
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

        public override void SessionConnected(UdpSession session, IProtocolToken token)
        {
            notifyClientConnectToSession?.Invoke();
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