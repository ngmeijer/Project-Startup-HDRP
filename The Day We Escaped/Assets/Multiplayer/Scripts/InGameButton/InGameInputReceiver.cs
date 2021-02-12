using UnityEngine;
using UnityEngine.Events;

public class InGameInputReceiver : MonoBehaviour
{
    public int maxChars;
    [SerializeField]
    private string _storedVal;

    [SerializeField] private bool _stopReceiveInput;
    
    public StringUnityEvent notifyValAdded;
    public StringUnityEvent notifyVal;

    public StringUnityEvent notifyFilled;

    public UnityEvent notifyCleared;
    
    public void ReceiveInput(string val)
    {
        if (_stopReceiveInput)
            return;
        
        var tempVal = _storedVal + val;

        if (tempVal.Length <= maxChars)
        {
            _storedVal = tempVal;
            
            //Trigger val added
            notifyValAdded?.Invoke(val);
            notifyVal?.Invoke(_storedVal);
        }

        if (_storedVal.Length == maxChars)
        {
            BoltLog.Warn($"{this} filled triggered | val: {_storedVal}");
            
            notifyFilled?.Invoke(_storedVal);
            _stopReceiveInput = true;
        }
    }

    public void ResetState()
    {
        ClearInput();
        _stopReceiveInput = false;
    }
    
    public void ClearInput()
    {
        _storedVal = "";
        notifyCleared?.Invoke();
    }
}