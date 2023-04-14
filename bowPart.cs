using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bowPart : MonoBehaviour
{
    public List<connectPoint> connectPoints;
    
    private Vector3 _pointScreen;
    private Vector3 _offset;
    private Camera _baseCamera;

    private int numeFalse;

   

    private void Start()
    {
        _baseCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    
    private void OnMouseDown()
    {
        _pointScreen = _baseCamera.WorldToScreenPoint(transform.position);
        _offset = transform.position - _baseCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
        OnDragStart();
    }
    
    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _pointScreen.z);
        Vector3 curPosition = _baseCamera.ScreenToWorldPoint(curScreenPoint);
        transform.position = curPosition;
    }

    void OnMouseUp()
    {
        OnDragOver();
    }

    //Connect bone
    public void Connect(connectPoint myPoint, connectPoint otherPoint)
    {
       
        
        var difference = myPoint.transform.position - otherPoint.transform.position;

        if(myPoint.transform.CompareTag("String") || myPoint.transform.CompareTag("spring"))
            myPoint.transform.position -= difference;
        else
            myPoint.transform.parent.position -= difference;
        
        
        
        foreach (var cPoint in connectPoints)
        {
            cPoint.currentStatus = connectPoint.JointStatus.Fixed;
        }
    }

    public void OnDragStart()
    {
        foreach (var cPoint in connectPoints)
        {
            cPoint.OnDragStart();
        }
    }
    
    public void OnDragOver()
    {
        foreach (var cPoint in connectPoints)
        {
            cPoint.OnDragOver();
            if (!cPoint.OnDragOver()) numeFalse++;
        }
        
        if(numeFalse == 3)
        {
            foreach (var cPoint in connectPoints)
            {
                cPoint.currentStatus = connectPoint.JointStatus.None;
            }
        }

        numeFalse = 0;
    }
    
    public void AddAsChildToBow(connectPoint parentConnectPoint)
    {
        if(transform.parent!=null) return;
        if (this == parentConnectPoint.bowPart) return;
        transform.SetParent(parentConnectPoint.transform);
        foreach (var connectPoint in connectPoints)
        {
            if(!connectPoint.targetConnectPoint) continue;
            if(!connectPoint.targetConnectPoint==parentConnectPoint) continue;
            connectPoint.targetConnectPoint.bowPart.AddAsChildToBow(parentConnectPoint);
        }
    }

}
