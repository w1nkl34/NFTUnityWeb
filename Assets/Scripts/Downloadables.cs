using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Downloadables : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartDownload());
    }
    public IEnumerator StartDownload()
    {
        GameObject AssetGO = null;
        UnityWebRequest uwr;
       
        uwr = UnityWebRequestAssetBundle.GetAssetBundle("https://github.com/w1nkl34/Test/raw/main/town");
        yield return uwr.SendWebRequest();

        var loadAsset = DownloadHandlerAssetBundle.GetContent(uwr).LoadAssetAsync<GameObject>("Assets/AssetBundles/" + "town" + ".prefab");
        yield return loadAsset;
        AssetGO = (GameObject)loadAsset.asset;
        Instantiate(AssetGO);
    }


}   
