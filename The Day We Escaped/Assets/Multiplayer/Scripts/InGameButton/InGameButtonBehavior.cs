using System;
using System.Collections;
using Bolt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameButtonBehavior : Bolt.EntityEventListener<IInGameButtonState>, IPointerClickHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("Maximum distance to receive a click")]
    public float distanceAllowed = 3f;

    [Tooltip("Lock states change for some time, avoid burst clicks")]
    public float lockChangeDelay = 1f;

    private bool _isWaitingLockStateDelay;

    /// <summary>
    /// notifyStates is a array of UnityEvents, each time this buttons is clicked, a event of the array is fired
    /// if totalInteractionsAllowed == 1, at the end of the notifyStates array, no more clicks will be allowed
    /// if totalInteractionsAllowed == -1, no limit, the clicks will keep cycling through the notifyStates array
    /// Example: totalInteractionsAllowed == 2, 2 cycles of interactions will happen and then end
    /// </summary>
    [Tooltip("How many interactions will happen (interactions = flow through notifyStates, better explained in code)")]
    public int totalInteractionsAllowed = 1;

    [SerializeField] private int _interactionsCount;
    [SerializeField] private bool _disabled;

    [Header("Notify States")] public IntUnityEvent[] notifyStates;
    public IntUnityEvent notifyDisabled;

    [Header("Notify States (run in Server Only)")]
    public IntUnityEvent[] notifyStatesInServer;

    [Header("OnHover Events")] public UnityEvent notifyOnEnter;
    public UnityEvent notifyOnExit;

    public override void Attached()
    {
        _interactionsCount = 0;

        state.AddCallback("StateNumber", () =>
        {
            notifyStates[state.StateNumber]?.Invoke(state.StateNumber);

            if (BoltNetwork.IsServer)
            {
                notifyStatesInServer[state.StateNumber]?.Invoke(state.StateNumber);
            }
        });
    }

    public void SetNextStateInServer()
    {
        //Ignore if a change state is already happening
        if (_isWaitingLockStateDelay)
            return;

        if (notifyStates.Length > 0 && totalInteractionsAllowed > 0 &&
            _interactionsCount >= (totalInteractionsAllowed * notifyStates.Length))
        {
            notifyDisabled?.Invoke(state.StateNumber);
            return;
        }

        _isWaitingLockStateDelay = true;

        state.StateNumber = (state.StateNumber + 1) % notifyStates.Length;
        _interactionsCount++;

        if (lockChangeDelay > 0)
            StartCoroutine(LockDelayRoutine());
    }

    private IEnumerator LockDelayRoutine()
    {
        yield return new WaitForSeconds(lockChangeDelay);
        _isWaitingLockStateDelay = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_disabled)
            return;

        BoltLog.Info($"{this} clicked | distance: {eventData.pointerCurrentRaycast.distance}");

        // if (eventData.pointerCurrentRaycast.distance > distanceAllowed)
        //     return;

        var evt = InGameButtonBoltEvent.Create(GlobalTargets.OnlyServer);
        evt.Entity = this.entity;
        evt.Send();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        notifyOnEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        notifyOnExit?.Invoke();
    }
}