using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
     void LateUpdate () {
        
        if (Input.GetMouseButton(0)) 
         {
            // if (!EventSystem.current.IsPointerOverGameObject())
                       if (Input.touchCount == 1)
             Camera.main.transform.position += new Vector3(-Input.GetAxis("Mouse X") / 3, -Input.GetAxis("Mouse Y") / 5);
         }

         #if UNITY_EDITOR
           if (Input.touchCount > 0)
        {       
               Touch touch = Input.GetTouch(0);
            // if (!EventSystem.current.IsPointerOverGameObject())
            if (touch.phase == TouchPhase.Moved)
            {
             Camera.main.transform.position += new Vector3(- touch.deltaPosition.x  * Time.deltaTime * 8, - touch.deltaPosition.y *  Time.deltaTime * 8);

            }
         }
         #endif
        if(Camera.main.transform.position.x  > 16)
            Camera.main.transform.position  = new Vector3(16,Camera.main.transform.position .y,Camera.main.transform.position.z);
        if(Camera.main.transform.position.x  < -15)
            Camera.main.transform.position  = new Vector3(-15,Camera.main.transform.position .y,Camera.main.transform.position.z);
        if(Camera.main.transform.position.y  >2)
            Camera.main.transform.position  = new Vector3(Camera.main.transform.position.x,2,Camera.main.transform.position.z);
        if(Camera.main.transform.position.y  < -8)
            Camera.main.transform.position  = new Vector3(Camera.main.transform.position.x,-8,Camera.main.transform.position.z);

     }
}
