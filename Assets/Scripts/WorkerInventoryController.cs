using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WorkerInventoryController : MonoBehaviour
{
    public Image workersImage;

    public void ResetAllImages()
    {
        int count = 0;
        foreach(Transform child in workersImage.transform.parent)
        {
            if(count != 0)
            Destroy(child.gameObject);
            count++;
        }
    }
    public IEnumerator GenerateWorkers()
    {
        ResetAllImages();
        foreach(Workers worker in Constants.currentUser.workers)
        {
            yield return StartCoroutine(GetTexture(worker));
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
            GameObject newImage = Instantiate(workersImage.gameObject,workersImage.transform.parent);
            newImage.GetComponent<Image>().sprite = mySprite;
            WorkerHolder workerHolder = newImage.GetComponent<WorkerHolder>();
            workerHolder.worker = new Workers();
            workerHolder.worker = worker;
            newImage.SetActive(true);

        }
    }
    
}
