using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PopUpWorkProductionController : MonoBehaviour
{

    public GameObject workerMain;

    public Field field;
    public Text productType;

   public void GenerateProduct(Field field)
   {
       productType.text = field.productItem;
       StartCoroutine(GenerateWorkers());
   }

   public void ClosePop()
   {
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
       gameObject.SetActive(false);
   }

    public void ResetAllImages()
    {
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
        GetTexture();
    }


     public void GetTexture() {
         for(int i = 0; i<Constants.workerSprites.Count; i++)
         {
            Sprite mySprite = Constants.workerSprites[i];
            GameObject newImage = Instantiate(workerMain.gameObject,workerMain.transform.parent);
            newImage.GetComponent<WorkerHolderProduction>().workerImage.sprite = mySprite;
            WorkerHolderProduction workerHolder = newImage.GetComponent<WorkerHolderProduction>();
            workerHolder.worker = new Workers();
            workerHolder.worker = Constants.workersData[i];
            newImage.SetActive(true);
           
         }
       
    }

 
}
