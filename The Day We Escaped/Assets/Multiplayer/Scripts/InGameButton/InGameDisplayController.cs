using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class InGameDisplayController : MonoBehaviour
{
    [SerializeField]
    public float[] durations;
    [SerializeField]
    public string[] messages;
    
    public StringUnityEvent notifyMessage;
    public UnityEvent notifyAfterDuration;

    public void SetMessages(string[] pMessages)
    {
        messages = pMessages;
    }
    
    public void DisplayMessages()
    {
        StartCoroutine(DisplayMessagesRoutine());
    }
    
    public void DisplayMessages(string[] pMessages, float[] pDurations)
    {
        messages = pMessages;
        durations = pDurations;
        StartCoroutine(DisplayMessagesRoutine());
    }
    
    public IEnumerator DisplayMessagesRoutine()
    {
        for (int i = 0; i< messages.Length; i++)
        {
            notifyMessage?.Invoke(messages[i]);
            yield return new WaitForSeconds(durations[i]);
        }
        
        notifyAfterDuration?.Invoke();
    }
}