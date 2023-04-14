using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CameraMove : MonoBehaviour
{
    private Camera _camera;
    private float _cycleLength = 2f;

    private arrow _permissionTargetArrow;

    private ManagementString _permissionStart;

    private arrow _arrowModel;
    private Vector3 _target;
    private float _speed = 3f;
    
    public float _x = -20;
    public float _y = 17f;
    public float _z = -20f;
    
    public ParticleSystem animWin;

    private void Awake()
    {
        _camera = gameObject.GetComponent<Camera>();
    }


    public void TargetArrow(Transform arrowModel, arrow arrowScript)
    {
        _target = new Vector3(arrowModel.position.x -_x, arrowModel.position.y +_y,arrowModel.position.z +_z);
        transform.position = Vector3.Lerp(transform.position, _target, _speed * Time.fixedDeltaTime);
        _permissionTargetArrow = arrowScript;
    }
    public void PreparationShotBow( ManagementString permission)
    {
        _camera.depth = 0;
        _permissionStart = permission;
        transform.DOMove(new Vector3(176f, 200f, -3f), _cycleLength,false);
        transform.DORotate(new Vector3(0,60.5f,0),_cycleLength);
    }

    public void GoToApple()
    {
        _permissionTargetArrow.permissionTargetArrow = false;
        
        transform.DOMove(new Vector3(223f, 200f, 9f), 1f,false);
        
        StartCoroutine(EndLvl(false));
    }

    public void AnimationWin()
    {
        _permissionTargetArrow.permissionTargetArrow = false;
        transform.DOMove(new Vector3(223f, 200f, 9f), 1f,false);

        StartCoroutine(EndLvl(true));
        
        //Instantiate(animWin, new Vector3(111.6f, 51f, -7f), Quaternion.identity);
        print("WIIIIIIIN !!!!!!!!!");
    }

    public void StartPosition()
    {
        _permissionTargetArrow.permissionTargetArrow = false;
        _permissionStart.cameraStartPosition = true;
            
        transform.DOMove(new Vector3(200f, 200f, -10f), _cycleLength,false);
        transform.DORotate(new Vector3(0,0,0),_cycleLength);
    }

    private IEnumerator EndLvl(bool win)
    {
        _permissionTargetArrow.permissionGoBack = false;
        yield return new WaitForSeconds(3f);
        StartPosition();

        if(win)
        {
            yield return new WaitForSeconds(3f);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            yield return new WaitForSeconds(3f);
            this.gameObject.SetActive(false);
        }

        yield return true;
    }
}
