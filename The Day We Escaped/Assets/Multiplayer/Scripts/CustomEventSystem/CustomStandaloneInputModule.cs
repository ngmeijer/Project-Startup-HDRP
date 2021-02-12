using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Event/Custom Standalone Input Module")]

/// <summary>
/// Works with CursorMode.Locked
/// </summary>
public class CustomStandaloneInputModule : StandaloneInputModule
{
    private readonly MouseState m_MouseState = new MouseState();

    protected override MouseState GetMousePointerEventData()
    {
        return GetMousePointerEventData(0);
    }
    
    protected override MouseState GetMousePointerEventData(int id)
    {
        // Populate the left button...
        PointerEventData leftData;
        var created = GetPointerData(kMouseLeftId, out leftData, true);

        leftData.Reset();

        if (created)
            leftData.position = input.mousePosition;

        Vector2 pos = input.mousePosition;
        
        leftData.delta = pos - leftData.position;
        leftData.position = pos;

        leftData.scrollDelta = input.mouseScrollDelta;
        leftData.button = PointerEventData.InputButton.Left;
        eventSystem.RaycastAll(leftData, m_RaycastResultCache);
        var raycast = FindFirstRaycast(m_RaycastResultCache);
        leftData.pointerCurrentRaycast = raycast;
        m_RaycastResultCache.Clear();

        // copy the apropriate data into right and middle slots
        PointerEventData rightData;
        GetPointerData(kMouseRightId, out rightData, true);
        CopyFromTo(leftData, rightData);
        rightData.button = PointerEventData.InputButton.Right;

        PointerEventData middleData;
        GetPointerData(kMouseMiddleId, out middleData, true);
        CopyFromTo(leftData, middleData);
        middleData.button = PointerEventData.InputButton.Middle;

        m_MouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
        m_MouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
        m_MouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);

        return m_MouseState;
    }

    protected override void ProcessMove(PointerEventData pointerEvent)
    {
        var targetGO = pointerEvent.pointerCurrentRaycast.gameObject;
        HandlePointerExitAndEnter(pointerEvent, targetGO);
    }

    protected override void ProcessDrag(PointerEventData pointerEvent)
    {
        if (!pointerEvent.IsPointerMoving() ||
            pointerEvent.pointerDrag == null)
            return;

        if (!pointerEvent.dragging
            && ShouldStartDragExtended(pointerEvent.pressPosition, pointerEvent.position,
                eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
        {
            ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
            pointerEvent.dragging = true;
        }

        // Drag notification
        if (pointerEvent.dragging)
        {
            // Before doing drag we should cancel any pointer down state
            // And clear selection!
            if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
            {
                ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                pointerEvent.eligibleForClick = false;
                pointerEvent.pointerPress = null;
                pointerEvent.rawPointerPress = null;
            }

            ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
        }
    }

    private static bool ShouldStartDragExtended(Vector2 pressPos, Vector2 currentPos, float threshold,
        bool useDragThreshold)
    {
        if (!useDragThreshold)
            return true;

        return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
    }
}