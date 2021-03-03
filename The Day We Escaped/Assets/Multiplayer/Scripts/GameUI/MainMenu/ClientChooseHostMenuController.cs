using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Bolt;
using Bolt.Matchmaking;
using Bolt.Utils;
using UdpKit;
using UdpKit.Platform.Photon;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Multiplayer.Scripts.GameUI.MainMenu
{
    public class ClientChooseHostMenuController : Bolt.GlobalEventListener
    {
        [SerializeField] private RectTransform _contentPanel;

        [SerializeField] private Button _joinHostButtonPrefab;

        private Dictionary<Guid, Button> _buttonsMap;

        public UnityEvent notifyNoHostsFound;
        public UnityEvent notifyHostFound;
        public UnityEvent notifyJoiningSession;
        public StringUnityEvent notifySessionJoined;
        public UnityEvent notifySceneLoadLocalBegin;
        public StringUnityEvent notifySessionConnectFailed;
        public StringUnityEvent notifyDisconnected;

        public UnityEvent notifyBoltShutDown;

        public GameObjectUnityEvent notifyFirstHostBtnAdded;
        
        private IEnumerator _checkForHosts;
        private bool _isCheckingForHost;
        
        private void Awake()
        {
            _buttonsMap = new Dictionary<Guid, Button>();
        }

        private IEnumerator Start()
        {
            CheckForHosts();
            
            yield break;
        }

        void CheckForHosts()
        {
            UpdateHostsUi();
            NotifyEmptySession(BoltNetwork.SessionList.Count == 0);
        }

        void NotifyEmptySession(bool empty)
        {
            if (empty)
            {
                notifyNoHostsFound?.Invoke();
            }
            else
            {
                notifyHostFound?.Invoke();
            }
        }

        void UpdateHostsUi()
        {
            foreach (var session in BoltNetwork.SessionList)
            {
                UdpSession udpSession = session.Value as UdpSession;

                // Skip if is not a Photon session
                if (udpSession.Source != UdpSessionSource.Photon)
                    continue;
                
                if (!_buttonsMap.ContainsKey(session.Key))
                {
                    //Add Button
                    AddButton(session);
                }
            }
            
            if (_buttonsMap.Count > BoltNetwork.SessionList.Count)
            {
                RemoveButtons();
            }
        }

        private void AddButton(KeyValuePair<Guid, UdpSession> session)
        {
            var btn = Instantiate(_joinHostButtonPrefab, _contentPanel);

            AddButtonOnClickEvent(btn, session.Value);
            AddRedStripeEffect(btn);

            _buttonsMap.Add(session.Key, btn);

            var guidStr = session.Key.ToString();
            var shortGuid = guidStr.Substring(guidStr.Length - 5, 5);

            btn.GetComponentInChildren<Text>(true).text = $"Host: {shortGuid}";

            if (_contentPanel.childCount == 1)
            {
                //first host added
                notifyFirstHostBtnAdded?.Invoke(btn.gameObject);
            }
        }

        void RemoveButtons()
        {
            var toRemove = new List<Guid>();
            foreach (var kv in _buttonsMap)
            {
                if (!BoltNetwork.SessionList.TryFind(kv.Key, out var udpSession))
                {
                    toRemove.Add(kv.Key);
                    Destroy(kv.Value.gameObject);
                }
            }

            for (int i = toRemove.Count - 1; i >= 0; i--)
            {
                _buttonsMap.Remove(toRemove[i]);
            }
        }

        void AddButtonOnClickEvent(Button btn, UdpSession session)
        {
            btn.onClick.AddListener(delegate { JoinSession(session); });
        }

        void AddRedStripeEffect(Button btn)
        {
            var evtTrigger = btn.GetComponent<EventTrigger>();
            var pointEnter = evtTrigger.triggers.FirstOrDefault(e => e.eventID == EventTriggerType.PointerEnter);
            if (pointEnter == null)
            {
                pointEnter = new EventTrigger.Entry()
                {
                    eventID = EventTriggerType.PointerEnter,
                };
                evtTrigger.triggers.Add(pointEnter);
            }

            var redStripesRoot = gameObject.transform.root;
            var redStripeT = redStripesRoot.Find("RedStripe Image");

            var tweenTo = redStripeT.GetComponent<TweenMoveUIToTarget>();
            if (tweenTo == null)
                return;
            
            pointEnter.callback.AddListener(delegate(BaseEventData arg0)
            {
                tweenTo.PlayToTarget(btn.GetComponent<RectTransform>());
            });
        }

        public void JoinSession(UdpSession session)
        {
            notifyJoiningSession?.Invoke();
            BoltMatchmaking.JoinSession(session);
        }

        public override void SessionConnected(UdpSession session, IProtocolToken token)
        {
            var guidStr = session.Id.ToString();
            var shortGuid = guidStr.Substring(guidStr.Length - 5, 5);

            notifySessionJoined?.Invoke($"Waiting for The Host {session.Id} starts the Level");
        }

        public override void SessionConnectFailed(UdpSession session, IProtocolToken token, UdpSessionError errorReason)
        {
            notifySessionConnectFailed?.Invoke($"Connection to Host failed: {errorReason}");
            
            StartCoroutine(WaitBeforeCheckForHosts());
        }

        public override void SceneLoadLocalBegin(string scene, IProtocolToken token)
        {
            BoltLog.Warn($"{this.gameObject}: SceneLoadLocalBegin");
            notifySceneLoadLocalBegin?.Invoke();

            StartCoroutine(WaitBeforeCheckForHosts());
        }

        public override void Disconnected(BoltConnection connection)
        {
            notifyDisconnected?.Invoke($"Disconnected from Host: {connection.DisconnectReason}");
            
            StartCoroutine(WaitBeforeCheckForHosts());
        }
        
        public override void BoltShutdownBegin(AddCallback registerDoneCallback, UdpConnectionDisconnectReason disconnectReason)
        {
            registerDoneCallback(() =>
            {
                BoltLog.Warn("Bolt is down");
                notifyBoltShutDown?.Invoke();
            });
        }

        IEnumerator WaitBeforeCheckForHosts()
        {
            yield return new WaitForSeconds(2);

            if (BoltNetwork.IsRunning == false)
            {
                BoltLauncher.StartClient();
            }
            
            CheckForHosts();
        }

        public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
        {
            BoltLog.Warn($"{this.gameObject}: SessionListUpdated callback");
            CheckForHosts();
        }
    }
}