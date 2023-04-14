using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public bowPart osnova;
    
    public void AddAsChildConnectedPoints() //button click
    {
        foreach (var connectPoint in osnova.connectPoints)
        {
            if(!connectPoint.targetConnectPoint) continue;
            connectPoint.targetConnectPoint.bowPart.AddAsChildToBow(connectPoint);
        }
    }
}
