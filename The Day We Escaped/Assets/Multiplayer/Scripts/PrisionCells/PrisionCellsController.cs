using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PrisionCellsController : MonoBehaviour
{
    [SerializeField] private SimpleStateBehaviour[] _cellsBehaviours;
    private int[] cellsNumbers;

    public UnityEvent[] notifyCell;
    public UnityEvent[] notifyCellInServer;
    public UnityEvent notifyFail;
    public StringArrayUnityEvent notifySuccess;

    private void Awake()
    {
        cellsNumbers = _cellsBehaviours.Select(cb => cb.id).ToArray();
    }

    public void ToggleCellDoor(string val)
    {
        int number = int.Parse(val);
        int cellIndex = Array.IndexOf(cellsNumbers, number);
        if (cellIndex > -1)
        {
            var cell = _cellsBehaviours[cellIndex];

            var msg = cell.state.StateNumber == 0 ? new string[] {"CELL OPENED", ""} : new string[] {"CELL CLOSED", ""};
            
            notifySuccess?.Invoke(msg);
            
            notifyCell[cellIndex]?.Invoke();

            if (BoltNetwork.IsServer)
            {
                notifyCellInServer[cellIndex]?.Invoke();
            }
        }
        else
            notifyFail?.Invoke();
    }
}