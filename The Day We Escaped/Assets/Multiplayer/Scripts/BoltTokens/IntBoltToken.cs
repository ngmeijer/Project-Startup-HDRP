using System.Collections;
using System.Collections.Generic;
using UdpKit;
using UnityEngine;

public class IntBoltToken : Bolt.IProtocolToken
{
    public int intVal;
    
    public void Read(UdpPacket packet)
    {
        intVal = packet.ReadInt();
    }

    public void Write(UdpPacket packet)
    {
        packet.WriteInt(intVal);
    }
}
