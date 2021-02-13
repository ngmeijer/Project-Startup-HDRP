using System.Collections;
using Bolt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class InGameSimpleButtonBoltCallback : Bolt.GlobalEventListener
    {
        [Tooltip("Lock states change for some time, avoid burst clicks")]
        public float lockChangeDelay = 1f;

        private bool _isWaitingLockStateDelay;

        [SerializeField] private int _parentId;
        [SerializeField] private int _id;
        public int Id => _id;
        
        [SerializeField] private bool _disabled;

        public UnityEvent onEventReceived;

        public override void OnEvent(SimpleButtonBoltEvent evnt)
        {
            BoltLog.Warn($"{this} evt received | {evnt}");
            
            if (evnt.ParentId != _parentId || evnt.Id != _id)
            {
                return;
            }

            if (_isWaitingLockStateDelay)
            {
                return;
            }

            StartCoroutine(LockDelayRoutine());
            
            onEventReceived?.Invoke();
        }

        private IEnumerator LockDelayRoutine()
        {
            _isWaitingLockStateDelay = true;
            yield return new WaitForSeconds(lockChangeDelay);
            _isWaitingLockStateDelay = false;
        }
        
        public void OnClickByClient(PointerEventData eventData)
        {
            if (_disabled)
                return;

            BoltLog.Info($"{this} clicked | distance: {eventData.pointerCurrentRaycast.distance}");

            var evt =  SimpleButtonBoltEvent.Create(GlobalTargets.Everyone);
            evt.ParentId = _parentId;
            evt.Id = _id;
            evt.Send();
        }

        public void OnEnter(PointerEventData eventData)
        {
        }

        public void OnExit(PointerEventData eventData)
        {
        }
    }
