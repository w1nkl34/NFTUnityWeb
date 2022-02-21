using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerBuildingController : MonoBehaviour
{
   public void CreateWorkerRequest(int index)
   {
        FindObjectOfType<ReactSend>().CreateWorkerCall(index);
   }
}
