using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpInfo : MonoBehaviour
{
    public Text messageText;
    public bool onMenu;
    public void ClosePop()
    {
        if(onMenu)
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
        gameObject.SetActive(false);
    }

}
