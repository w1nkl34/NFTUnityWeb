using UnityEngine;
using System.Runtime.InteropServices;

public class ReactSend : MonoBehaviour {

  [DllImport("__Internal")]
  private static extern void CreateNFT (string docId);
  public void CreateNFTCall (string docIdCall) {
    #if UNITY_WEBGL == true && UNITY_EDITOR == false
        CreateNFT(docIdCall);
    #endif
  }

  [DllImport("__Internal")]
  private static extern void CreateWorker(int index);
  public void CreateWorkerCall (int indexCall) {
    #if UNITY_WEBGL == true && UNITY_EDITOR == false
        CreateWorker(indexCall);
    #endif
  }
}