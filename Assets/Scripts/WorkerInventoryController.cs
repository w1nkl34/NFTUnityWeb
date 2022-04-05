using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WorkerInventoryController : MonoBehaviour
{

    public GameObject workerMain;

    public GameObject loadingWidget;
    private int currentWorkerCount = 0;
    private int totalWorkerCount = 0;
    public void ResetAllImages()
    {
        currentWorkerCount = 0;
        int count = 0;
        foreach(Transform child in workerMain.transform.parent)
        {
            if(count != 0)
            Destroy(child.gameObject);
            count++;
        }
    }
    public IEnumerator GenerateWorkers()
    {
        StopAllCoroutines();
        ResetAllImages();
        GenerateWorkersFaster();
        yield return null;
    }

    public void GenerateWorkersFaster()
    {
        loadingWidget.SetActive(true);
        totalWorkerCount = Constants.currentUser.workers.Count;
        foreach(Workers worker in Constants.currentUser.workers)
        {
        StartCoroutine(GetTexture(worker));
        }
    }

     IEnumerator GetTexture(Workers worker) {

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(worker.url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log(www.error);
        }
        else {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite mySprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            GameObject newImage = Instantiate(workerMain.gameObject,workerMain.transform.parent);

            newImage.GetComponent<WorkerHolder>().workerImage.sprite = mySprite;
            WorkerHolder workerHolder = newImage.GetComponent<WorkerHolder>();
            workerHolder.worker = new Workers();
            workerHolder.worker = worker;
            newImage.SetActive(true);
            currentWorkerCount ++;
            if(currentWorkerCount == totalWorkerCount)
            {
                loadingWidget.SetActive(false);
            }
        }
    }
    
}
