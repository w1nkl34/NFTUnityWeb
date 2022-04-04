using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{

    public TownManager tm;
    public bool dragging = false;
    private float startingPosition;
    private float startingPositiony;
    private float touchposx;
    private float touchposy;

    public Cinemachine.CinemachineVirtualCamera cm;

    public Transform cameraLookAtTarget;
    public Transform cameraLookAtTargetWorld;
    float touchDist = 0;
    float lastDist = 0;
    Vector3 touchStart;
    float minZoomValue=-40;
    float maxZoomValue = -20;
     float maxPosX = 20;
     float minPosX = -19;
     float maxPosY = 4;
     float minPosY = -12;

     float lastZoomValue = -3400;

     float cameraSpeedX  = 0.035f;
     float cameraSpeedY = 0.035f;
     public void ChangeWorldMode()
     {
         float tempValue = cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;
         if(!Constants.onWorldMap)
         {
            cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
            new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
            lastZoomValue);
            cm.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.1f;
            cm.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.1f;
            cm.m_Follow = cameraLookAtTarget;
            cm.m_LookAt = cameraLookAtTarget;
            minZoomValue = -40;
            maxZoomValue = -20;
            maxPosX = 20;
            minPosX = -19;
            maxPosY = 4;
            minPosY = -12;
            cameraSpeedY = 0.03f;
            cameraSpeedX = 0.03f;
         }
         else
         {
            cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
            new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
            lastZoomValue);
            cm.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
            cm.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
            cm.m_Follow = cameraLookAtTargetWorld;
            cm.m_LookAt = cameraLookAtTargetWorld;
            minZoomValue = -40;
            maxZoomValue = -20;
            maxPosX = 22;
            minPosX = -22;
            maxPosY = 14;
            minPosY = -6;
            cameraSpeedY = 0.03f;
            cameraSpeedX = 0.03f;
         }
         lastZoomValue = tempValue;
     }
 
     void Update()
     {
         if(Constants.authenticated == true)
         {
         if(!Constants.onWorldMap)
         { 
            OnClickHandle();
            TouchHandle();
            CameraMovement(cameraLookAtTarget);
           
         }
         else
         {
            CameraMovement(cameraLookAtTargetWorld);     
         }
         }
     }

     public void TouchHandle()
     {
        if (Input.touchCount > 0)
        {       
               Touch touch = Input.GetTouch(0);
                if(touch.phase == TouchPhase.Began)
                {
                        startingPosition = touch.position.x;
                        startingPositiony = touch.position.y;
                        touchposx = startingPosition;
                        touchposy = startingPositiony;
                        dragging = false;
                }
                if(touch.phase == TouchPhase.Moved)
                {
                    if (startingPosition > touch.position.x && Mathf.Abs(startingPosition - touch.position.x) > 25)
                        {
                            dragging = true;
                            if (touchposx > touch.position.x)
                                touchposx = touch.position.x;
                            else
                                startingPosition = touch.position.x;
                        }
                        else if (startingPosition < touch.position.x && Mathf.Abs(startingPosition - touch.position.x) > 25)
                        {
                            dragging = true;
                            if (touchposx < touch.position.x)
                                touchposx = touch.position.x;
                            else
                                startingPosition = touch.position.x;

                        }

                        if (startingPositiony > touch.position.y && Mathf.Abs(startingPositiony - touch.position.y) > 25)
                        {

                            dragging = true;
                            if (touchposy > touch.position.y)
                                touchposy = touch.position.y;
                            else
                                startingPositiony = touch.position.y;
                        }
                        else if (startingPositiony < touch.position.y && Mathf.Abs(startingPositiony - touch.position.y) > 25)
                        {
                            dragging = true;
                            if (touchposy < touch.position.y)
                                touchposy = touch.position.y;
                            else
                                startingPositiony = touch.position.y;
                        }
                }
                 if(touch.phase == TouchPhase.Ended)
                 dragging = false;
               
        }
     }

     public void OnClickHandle()
     {
        if(!Constants.onMenu)
        {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit,200)) 
        {
            if(hit.collider != null)
            {
                if(!dragging)
                {
                if(Input.GetMouseButtonUp(0))
                {
                tm.ManageColliderHit(hit);
                if(hit.collider.gameObject.tag != "onClickOpen" && hit.collider.gameObject.tag != "onClickUpgrade"  && hit.collider.gameObject.tag != "onClickInfo" )  
                cameraLookAtTarget.transform.position = new Vector3(hit.collider.transform.position.x,hit.collider.transform.position.y,cameraLookAtTarget.transform.position.z);
                }
                
                }

            }
        }
        else
          if(Input.GetMouseButtonDown(0))
            tm.CloseAllOnClicks();
        }
     }

    public void zoom(float increment)
    {
          cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
             new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
               Mathf.Clamp(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z + increment, minZoomValue, maxZoomValue));
    }
     public void CameraMovement(Transform cameraLookAtTarget)
     {
        if(!Constants.onMenu)
        {
            if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                zoom(difference * 0.075f);
            
            }
            if(Input.GetMouseButtonDown(0)){
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if(Input.GetMouseButton(0)){
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));


        if (Input.touchCount == 1)
        {       
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
            cameraLookAtTarget.position += new Vector3(- touch.deltaPosition.x * cameraSpeedX, -touch.deltaPosition.y * cameraSpeedY);
            }
         }
         if( cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z < minZoomValue)
         {
                cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
             new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
               minZoomValue); 
         }
           if( cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z > maxZoomValue)
         {
            cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
             new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
              maxZoomValue); 
         }
        
        if(cameraLookAtTarget.localPosition.x  > maxPosX)
            cameraLookAtTarget.localPosition  = Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(maxPosX,cameraLookAtTarget.localPosition .y,cameraLookAtTarget.localPosition.z),Time.deltaTime * (cameraLookAtTarget.localPosition.x - Mathf.Abs(maxPosX)));
        if(cameraLookAtTarget.localPosition.x  < minPosX)
            cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(minPosX,cameraLookAtTarget.localPosition .y,cameraLookAtTarget.localPosition.z),Time.deltaTime * (Mathf.Abs( cameraLookAtTarget.localPosition.x) - Mathf.Abs(minPosX)));
        if(cameraLookAtTarget.localPosition.y  > maxPosY)
            cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(cameraLookAtTarget.localPosition.x,maxPosY,cameraLookAtTarget.localPosition.z),Time.deltaTime * (cameraLookAtTarget.localPosition.y - Mathf.Abs(maxPosY)));
        if(cameraLookAtTarget.localPosition.y  < minPosY)
            cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(cameraLookAtTarget.localPosition.x,minPosY,cameraLookAtTarget.localPosition.z),Time.deltaTime * (Mathf.Abs(cameraLookAtTarget.localPosition.y) - Mathf.Abs(minPosY)));
        }
     }

}
