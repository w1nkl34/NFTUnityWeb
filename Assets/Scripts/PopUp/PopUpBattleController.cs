using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBattleController : MonoBehaviour
{
    public GameObject workerMain;

    public Field field;
    public Text productType;

   public void GenerateProduct(Field field)
   {
       productType.text = "Battle";
   }

    public void ClosePop()
   {
        FindObjectOfType<UIController>().SetOnMenuToFalseCorCall();
       gameObject.SetActive(false);
   }


}
