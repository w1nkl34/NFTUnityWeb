using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpInfo : MonoBehaviour
{
    public Text messageText;
    public void ClosePop()
    {
        gameObject.SetActive(false);
    }
}
