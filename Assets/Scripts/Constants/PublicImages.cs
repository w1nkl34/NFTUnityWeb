using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class PublicImages : MonoBehaviour
{
    public Sprite loadingSprite;
    public IEnumerator GetTexture(string iconUrl,Image sp) {
    {
        string persistentUrl = iconUrl.Replace("/","");
        UnityWebRequest www = new UnityWebRequest();
        if(File.Exists("file://" + Application.persistentDataPath  + "/" + persistentUrl +".png"))
        {
            www = UnityWebRequestTexture.GetTexture("file://" + Application.persistentDataPath  + "/" + persistentUrl +".png");
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success) 
                    {
                        Debug.Log(www.error);
                    }
                    else 
                    {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            Sprite mySprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            sp.sprite = mySprite;
                    }
        }
        else
        {
            FirebaseStorage storage = FirebaseStorage.DefaultInstance;
            StorageReference pathReference =
            storage.GetReference(iconUrl + ".png");
            var task = pathReference.GetDownloadUrlAsync();
            yield return new WaitUntil(predicate: () => task.IsCompleted);
            if(task.Exception != null)
            {
            }
            else
            {
                    www = UnityWebRequestTexture.GetTexture(task.Result);
                    yield return www.SendWebRequest();
                    if (www.result != UnityWebRequest.Result.Success) 
                    {
                        Debug.Log(www.error);
                    }
                    else 
                    {
                        File.WriteAllBytes("file://" + Application.persistentDataPath  + "/" + persistentUrl +".png",((DownloadHandlerTexture)www.downloadHandler).data);
                        Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                        Sprite mySprite = Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        sp.sprite = mySprite;
                    }
                }
            }
      
        }
    }
}
