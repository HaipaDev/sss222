using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tag_ButtonSelected : MonoBehaviour, ISelectHandler{
    [Sirenix.OdinInspector.DisableInEditorMode]public bool selected=false;
    public void OnSelect(BaseEventData eventData){
        foreach(Tag_ButtonSelected b in FindObjectsOfType<Tag_ButtonSelected>()){b.selected=false;}
        selected=true;
    }
}