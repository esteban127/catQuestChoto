using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]int maxZoomOut;
    [SerializeField] float initialZoom = 60.0f;
    [SerializeField]
    private float rotateSpeed = 6.0f;

    private float mauseInitialPos = 0.0f;
    private float currentRotation = 0.0f;
    int currentZoomOut = 0 ;
   
    
    private void Update()
    {

        Rotate();
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && currentZoomOut <maxZoomOut) // forward
        {
            currentZoomOut+=3;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentZoomOut > 0) // back
        {
            currentZoomOut-=3;
        }
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, initialZoom + currentZoomOut,0.5f);
    }

    private void Rotate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mauseInitialPos = Input.mousePosition.y;
        }
        if (Input.GetMouseButton(1))
        {
            currentRotation = ((Input.mousePosition.y - mauseInitialPos) / (Screen.width / 2));
        }
        if (Input.GetMouseButtonUp(1))
        {
            currentRotation = 0;
        }
        if(transform.localPosition.y > 0 && currentRotation < 0)
            transform.RotateAround(transform.parent.position, transform.parent.right, rotateSpeed * currentRotation * Time.deltaTime);  
        else       
        if(transform.localPosition.y < 3.75 && currentRotation > 0)
            transform.RotateAround(transform.parent.position, transform.parent.right, rotateSpeed * currentRotation * Time.deltaTime);

    }
}

