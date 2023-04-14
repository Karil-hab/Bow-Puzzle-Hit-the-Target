using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apple : MonoBehaviour
{
    private CameraMove _camera;

    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    

    private void Awake()
    {
        _camera = FindObjectOfType<CameraMove>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("arrow"))
        {
            ArrowConnect(other.transform);
            transform.GetComponent<Rigidbody>().isKinematic = false;
            transform.GetComponent<Rigidbody>().AddForce(25f,0f,0f,ForceMode.Impulse);
            other.transform.SetParent(this.transform);
            _camera.AnimationWin();
        }
    }

    private void ArrowConnect(Transform arrow)
    {
        arrow.parent.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
        FindObjectOfType<arrow>().gotApple = false;
        Instantiate(effect1, transform, false);
        Instantiate(effect2, transform, false);
        Instantiate(effect3, new Vector3(transform.position.x-6f,transform.position.y,transform.position.z-2f),Quaternion.identity);
    }
}
