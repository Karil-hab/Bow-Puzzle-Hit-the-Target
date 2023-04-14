using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class connectPoint : MonoBehaviour
{
    public bowPart bowPart;
    public connectPoint targetConnectPoint;
    public ManagementString controlString;

    private Color _startColor;

    public enum JointStatus
    {
        None,
        Fixed,
        Dragging
    }
    
    public JointStatus currentStatus = JointStatus.None;

    private void Awake()
    {
        controlString = FindObjectOfType<ManagementString>();
        _startColor = renderer.material.color;
    }

    public void Update()
    {
        if (targetConnectPoint && (gameObject.CompareTag("String") || gameObject.CompareTag("spring")))
        {
            bowPart.Connect(this, targetConnectPoint);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        print("Trigger something");
        if (other.TryGetComponent(out connectPoint connectPoint))
        {
            print("Have cp"); 
            CheckConnections(connectPoint);
        }
    }
    
    public bool CheckConnections(connectPoint connectPoint)
    {
        if (currentStatus != JointStatus.Dragging)
        {
            print("non-dragged");
            return false;
        }
        if (connectPoint.currentStatus != JointStatus.Fixed && currentStatus != JointStatus.None)
        {
            print("Exception of free other point");
            return false;
        }
        if(connectPoint.gameObject.layer != 6)
        {
            print("Wrong layer");
            return false;
        }
        if (targetConnectPoint != null)
        {
            print("dragged busy");
            return false;
        }
        if (connectPoint.targetConnectPoint != null)
        {
            print("target busy");
            return false;
        }
        
        targetConnectPoint = connectPoint;
        targetConnectPoint.targetConnectPoint = this;

        if(gameObject.CompareTag("String") || gameObject.CompareTag("spring"))
        {
            if(gameObject.CompareTag("String")) controlString.coutConnect++;
            if(gameObject.CompareTag("spring")) controlString.countSpringConnect++;

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            currentStatus = JointStatus.Fixed;

            bowPart.Connect(this, targetConnectPoint);
        }
        
        print(name+" connected in " + targetConnectPoint.name);
        ChangeColor(Color.green);
        targetConnectPoint.ChangeColor(Color.green);
        
        return true;
    }
    
    

    public void OnDragStart()
    {
        if (targetConnectPoint)
        {
            if(gameObject.CompareTag("String") || gameObject.CompareTag("spring"))
            {
                if(gameObject.CompareTag("String"))controlString.coutConnect--;
                if(gameObject.CompareTag("spring")) controlString.countSpringConnect--;
                
                gameObject.GetComponent<SphereCollider>().enabled = false;
                currentStatus = JointStatus.Dragging;
                StartCoroutine(DelayString());
            }
                
            if(targetConnectPoint.CompareTag("String") || targetConnectPoint.CompareTag("spring"))
            {
                if(targetConnectPoint.CompareTag("String"))targetConnectPoint.controlString.coutConnect--;
                if(gameObject.CompareTag("spring")) controlString.countSpringConnect--;
                
                targetConnectPoint.currentStatus = JointStatus.Dragging;
            }

            targetConnectPoint.ChangeColor(_startColor);
            targetConnectPoint.targetConnectPoint = null;
            targetConnectPoint = null;
        }

        currentStatus = JointStatus.Dragging;
        ChangeColor(Color.yellow);
    }
    public bool OnDragOver()
    {
        if(!targetConnectPoint)// proverka
        {
            ChangeColor(_startColor);
            return false;
        }

        if(targetConnectPoint) bowPart.Connect(this, targetConnectPoint);
        
        return true;
    }

    private IEnumerator DelayString()
    {
        yield return new WaitForSeconds(1.5f);
        gameObject.GetComponent<SphereCollider>().enabled = true;
        yield return true;
    }
    
    
    [Header("Rendering")] 
    public Renderer renderer;

    public void ChangeColor(Color newColor)
    {
        renderer.material.color = newColor;
    }

    
}