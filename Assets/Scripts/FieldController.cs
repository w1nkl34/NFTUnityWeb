using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldController : MonoBehaviour
{

    GameManager gm;
    public Field field;

    public void Awake()
    {
       gm = FindObjectOfType<GameManager>();

        if(field.fieldType == "production")
        GetComponent<Image>().sprite = gm.wm.productionImage;
        else
        GetComponent<Image>().sprite = gm.wm.battleImage;
  
    }
    public void OpenProductDetails()
    {
        if(field.fieldType == "production")
        {
        gm.popUpController.OpenProductionPop();
        gm.popUpController.popUpWorkProductionController.GenerateProduct(field);
        }
        else
        {
        gm.popUpController.OpenBattlePop();
        }

    }

    
}
