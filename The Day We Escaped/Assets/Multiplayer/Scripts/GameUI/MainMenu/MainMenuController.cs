using System;
using System.Collections;
using System.Linq;
using Bolt;
using Bolt.Matchmaking;
using Bolt.Utils;
using UdpKit;
using UdpKit.Platform;
using UdpKit.Platform.Photon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuController : Bolt.GlobalEventListener
    {
        [SerializeField] private bool _isShutingDown;
        [SerializeField] private string _gameLevel;

        public UnityEvent notifyStarting;
        public UnityEvent notifyServerStarting;
        public UnityEvent notifyClientStarting;
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

                // BoltMatchmaking.CreateSession(
                //     sessionID: matchName,
                //     sceneToLoad: _gameLevel
                //  );
            }
            else if (BoltNetwork.IsClient)
            {
                //BoltMatchmaking.JoinRandomSession();
            }
        }

        private void Update()
        {
            if (BoltNetwork.IsRunning && BoltNetwork.IsClient)
            {
                foreach (var session in BoltNetwork.SessionList)
                {
                    // Simple session
                    UdpSession udpSession = session.Value as UdpSession;

                    // Skip if is not a Photon session
                    if (udpSession.Source != UdpSessionSource.Photon)
                        continue;

                    // Photon Session
                    PhotonSession photonSession = udpSession as PhotonSession;

                    string sessionDescription = String.Format("{0} / {1} ({2})",
                        photonSession.Source, photonSession.HostName, photonSession.Id);


                    object value_t = -1;
                    object value_m = -1;

                    if (photonSession.Properties.ContainsKey("t"))
                    {
                        value_t = photonSession.Properties["t"];
                    }

                    if (photonSession.Properties.ContainsKey("m"))
                    {
                        value_m = photonSession.Properties["m"];
                    }

                    sessionDescription += String.Format(" :: {0}/{1}", value_t, value_m);

                    Debug.LogWarning($"key: {session.Key} | hostname: {session.Value.HostName} | {sessionDescription}");

                    //BoltMatchmaking.JoinSession(photonSession, connectToken);
                }
            }
        }

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            Debug.LogFormat("Session list updated: {0} total sessions", sessionList.Count);
        }

        public override void SessionCreatedOrUpdated(UdpSession session)
        {
            Debug.LogWarning($"SessionCreatedOrUpdated: {session.Id} | {session.HostName}");

            var splitId = session.Id.ToString().Split('-');

            notifySessionCreatedOrUpdated?.Invoke(splitId[splitId.Length - 1]);
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