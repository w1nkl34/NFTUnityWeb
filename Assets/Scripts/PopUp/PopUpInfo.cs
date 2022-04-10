using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpInfo : MonoBehaviour
{
    public Text messageText;
    public void ClosePop()
    {
        StartCoroutine(SetOnMenuToFalse());
        gameObject.SetActive(false);
    }

    public IEnumerator SetOnMenuToFalse()
    {
        yield return new WaitForSeconds(0.05f);
        Constants.onMenu = false;
    }
}
