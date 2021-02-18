using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltShutDownUtils : MonoBehaviour
{
    public void ShutDownBolt()
    {
        if (BoltNetwork.IsRunning == false)
            return;
        
        BoltNetwork.Shutdown();
    }
}
