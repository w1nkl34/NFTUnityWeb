using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
     void LateUpdate () {
        
        if (Input.GetMouseButton(0)) 
         {
            if (!EventSystem.current.IsPointerOverGameObject())
             Camera.main.transform.position += new Vector3(-Input.GetAxis("Mouse X") / 2, -Input.GetAxis("Mouse Y") / 2,0);
         }
        if(Camera.main.transform.position.x  > 15)
            Camera.main.transform.position  = new Vector3(15,Camera.main.transform.position .y,Camera.main.transform.position.z);
        if(Camera.main.transform.position.x  < -15)
            Camera.main.transform.position  = new Vector3(-15,Camera.main.transform.position .y,Camera.main.transform.position.z);
        if(Camera.main.transform.position.y  > 5)
            Camera.main.transform.position  = new Vector3(Camera.main.transform.position.x,5,Camera.main.transform.position.z);
        if(Camera.main.transform.position.y  < -12)
            Camera.main.transform.position  = new Vector3(Camera.main.transform.position.x,-12,Camera.main.transform.position.z);
     }
}
