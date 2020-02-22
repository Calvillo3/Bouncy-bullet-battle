using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ButtonTextColorChange : MonoBehaviour, ISelectHandler, IDeselectHandler {

    [SerializeField] Color color;
    [SerializeField] Button button;

    Color grey = new Color(105, 195, 195, 255);
 
     public void OnSelect(BaseEventData eventData)
     {
        
         button.GetComponentInChildren<TextMeshProUGUI>().color = color;
 
     }
     public void OnDeselect(BaseEventData eventData)
         {
             button.GetComponentInChildren<TextMeshProUGUI>().color = grey;
     
         }
     }
