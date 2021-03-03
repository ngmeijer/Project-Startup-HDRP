using System;
using System.Linq;
using Multiplayer.Scripts.SimpleObjectWithState;
using UnityEngine;
using UnityEngine.Events;

public class PrisionCellsController : MonoBehaviour
{
    [SerializeField] private SimpleIntStateBehaviour[] _cellsBehaviours;
    private int[] cellsNumbers;
    
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
            
            if (BoltNetwork.IsServer)
            {
                cell.NextStateInServer();
            }
        }
        else
            notifyFail?.Invoke();
    }
}