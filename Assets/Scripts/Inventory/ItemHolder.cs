using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
   public Text itemNameText;
   public Text itemCountText;

   public Image image;

   public string itemCount;
   public string itemName;
   public void OnEnable()
   {    
       if(itemCount == "0")
       image.color = new Color32(150,150,150,255);
       itemNameText.text = itemName;
       itemCountText.text = "x" + itemCount;
   }
}
