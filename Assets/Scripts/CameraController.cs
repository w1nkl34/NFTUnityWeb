using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{


    public GameManager gm;
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
     public float lastZoomValue = -30;

     public float lastFocusZoomValue = -14;
     float cameraSpeedX  = 0.035f;
     float cameraSpeedY = 0.035f;
     public bool focusWorldZone = false;


     public void ChangeToFocusZone(RaycastHit hit)
     {
                  float tempValue = cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;

                    int toAddX = 4;
                    bool isLarge = false;
                    if(hit.collider.gameObject.GetComponent<MainZoneController>() != null)
                    {
                    gm.wm.selectedMainZone = hit.collider.gameObject.GetComponent<MainZoneController>();
                    gm.wm.selectedMainZone.GenerateZones();
                    if(hit.collider.gameObject.GetComponent<MainZoneController>().isLarge == true)
                    isLarge = true;
                    }
                    gm.wm.ChangeZoneFocus(false);
                    focusWorldZone = true;
                    gm.uIController.OpenZoneFocus();
                    minZoomValue = -14;
                    maxZoomValue = -14;
                    cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
                    new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
                    lastFocusZoomValue); 
                    cameraLookAtTargetWorld.transform.position = new Vector3(hit.collider.transform.position.x,hit.collider.transform.position.y,cameraLookAtTarget.transform.position.z);
                    if(isLarge)
                    toAddX = 6;
                    maxPosX = cameraLookAtTargetWorld.transform.localPosition.x + toAddX;
                    minPosX = cameraLookAtTargetWorld.transform.localPosition.x - toAddX;
                    maxPosY = cameraLookAtTargetWorld.transform.localPosition.y +2;
                    minPosY = cameraLookAtTargetWorld.transform.localPosition.y -2;
                             lastFocusZoomValue = tempValue;

     }

     public void LeaveFromFocusZone()
     {
         float tempValue = cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z;
        cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
            new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
            lastFocusZoomValue);

            cm.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.1f;
            cm.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.1f;
            cm.m_Follow = cameraLookAtTargetWorld;
            cm.m_LookAt = cameraLookAtTargetWorld;
            minZoomValue = -30;
            maxZoomValue = -20;
            maxPosX = 24;
            minPosX = -24;
            maxPosY = 16;
            minPosY = -8;
            cameraSpeedY = 0.03f;
            cameraSpeedX = 0.03f;
         lastFocusZoomValue = tempValue;


     }

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
            cm.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0.1f;
            cm.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0.1f;
            cm.m_Follow = cameraLookAtTargetWorld;
            cm.m_LookAt = cameraLookAtTargetWorld;
            minZoomValue = -30;
            maxZoomValue = -20;
            maxPosX = 24;
            minPosX = -24;
            maxPosY = 16;
            minPosY = -8;
            cameraSpeedY = 0.03f;
            cameraSpeedX = 0.03f;
         }
         lastZoomValue = tempValue;
     }
 
     void Update()
     {
         if(Constants.authenticated == true)
         {
            TouchHandle();

         if(!Constants.onWorldMap)
         { 
            OnClickHandle();
            CameraMovement(cameraLookAtTarget);
           
         }
         else
         {
            OnClickHandleWorld();
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
                    if (startingPosition > touch.position.x && Mathf.Abs(startingPosition - touch.position.x) > 10)
                        {
                            dragging = true;
                            if (touchposx > touch.position.x)
                                touchposx = touch.position.x;
                            else
                                startingPosition = touch.position.x;
                        }
                        else if (startingPosition < touch.position.x && Mathf.Abs(startingPosition - touch.position.x) > 10)
                        {
                            dragging = true;
                            if (touchposx < touch.position.x)
                                touchposx = touch.position.x;
                            else
                                startingPosition = touch.position.x;

                        }

                        if (startingPositiony > touch.position.y && Mathf.Abs(startingPositiony - touch.position.y) > 10)
                        {

                            dragging = true;
                            if (touchposy > touch.position.y)
                                touchposy = touch.position.y;
                            else
                                startingPositiony = touch.position.y;
                        }
                        else if (startingPositiony < touch.position.y && Mathf.Abs(startingPositiony - touch.position.y) > 10)
                        {
                            dragging = true;
                            if (touchposy < touch.position.y)
                                touchposy = touch.position.y;
                            else
                                startingPositiony = touch.position.y;
                        }
                }
            
               
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
                gm.tm.ManageColliderHit(hit);
                if(hit.collider.gameObject.tag != "onClickOpen" && hit.collider.gameObject.tag != "onClickUpgrade"  && hit.collider.gameObject.tag != "onClickInfo" )  
                cameraLookAtTarget.transform.position = new Vector3(hit.collider.transform.position.x,hit.collider.transform.position.y,cameraLookAtTarget.transform.position.z);
                }
                }
            }
        }
        else
          if(Input.GetMouseButtonDown(0))
            gm.tm.CloseAllOnClicks();
        }
     }

     public void OnClickHandleWorld()
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
                if(hit.collider.gameObject.tag == "worldZone" && !focusWorldZone )  
                {
                   ChangeToFocusZone(hit);
                }
                }
                }
            }
        }
        }
     }


    public void zoom(float increment)
    {
          cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = 
             new Vector3(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x,  cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y,
               Mathf.Clamp(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z + increment, minZoomValue, maxZoomValue));
    }

    public void CameraZoom()
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
    }
     public void CameraMovement(Transform cameraLookAtTarget)
     {
         if(!Constants.onMenu)   
        {
            CameraZoom();

        if (Input.touchCount == 1)
        {       
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                cameraLookAtTarget.position += new Vector3(- touch.deltaPosition.x * cameraSpeedX * (1 - ((float)0.4 - Mathf.Abs(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z)/ 250)), - touch.deltaPosition.y * cameraSpeedY * (1 - ((float)0.4 - (Mathf.Abs(cm.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z)/ 250))));
            }
         }
       
            if( cameraLookAtTarget.localPosition.x  > maxPosX)
            {
                 if(maxPosX > 0)
                    cameraLookAtTarget.localPosition  = Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(maxPosX,cameraLookAtTarget.localPosition .y,cameraLookAtTarget.localPosition.z),Time.deltaTime * (cameraLookAtTarget.localPosition.x - Mathf.Abs(maxPosX)));
                else
                    cameraLookAtTarget.localPosition  = Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(maxPosX,cameraLookAtTarget.localPosition .y,cameraLookAtTarget.localPosition.z),Time.deltaTime * (cameraLookAtTarget.localPosition.x + Mathf.Abs(maxPosX)));

            }
  
            if(cameraLookAtTarget.localPosition.x  < minPosX)
            {
                if(minPosX < 0)
                    cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(minPosX,cameraLookAtTarget.localPosition .y,cameraLookAtTarget.localPosition.z),Time.deltaTime * (Mathf.Abs( cameraLookAtTarget.localPosition.x) - Mathf.Abs(minPosX)));
                else
                    cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(minPosX,cameraLookAtTarget.localPosition .y,cameraLookAtTarget.localPosition.z),Time.deltaTime * (Mathf.Abs( cameraLookAtTarget.localPosition.x) + Mathf.Abs(minPosX)));
            }

     
        if(cameraLookAtTarget.localPosition.y  > maxPosY)
        {
            if(maxPosY > 0)
                cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(cameraLookAtTarget.localPosition.x,maxPosY,cameraLookAtTarget.localPosition.z),Time.deltaTime * (cameraLookAtTarget.localPosition.y - Mathf.Abs(maxPosY)));
            else
                cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(cameraLookAtTarget.localPosition.x,maxPosY,cameraLookAtTarget.localPosition.z),Time.deltaTime * (cameraLookAtTarget.localPosition.y + Mathf.Abs(maxPosY)));

        }
        if(cameraLookAtTarget.localPosition.y  < minPosY)
        {
            if(minPosY < 0)
                cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(cameraLookAtTarget.localPosition.x,minPosY,cameraLookAtTarget.localPosition.z),Time.deltaTime * (Mathf.Abs(cameraLookAtTarget.localPosition.y) - Mathf.Abs(minPosY)));
            else
                cameraLookAtTarget.localPosition  =  Vector3.Lerp( cameraLookAtTarget.localPosition, new Vector3(cameraLookAtTarget.localPosition.x,minPosY,cameraLookAtTarget.localPosition.z),Time.deltaTime * (Mathf.Abs(cameraLookAtTarget.localPosition.y) + Mathf.Abs(minPosY)));

        }
        }
        
     }

}
