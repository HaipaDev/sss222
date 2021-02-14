using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SetSelectedButton : MonoBehaviour{
    
    EventSystem es;
    [SerializeField]Button btn;
    [SerializeField]bool onEnable=true;

    void Start(){
        es=FindObjectOfType<EventSystem>();
    }
    void OnEnable(){
        if(onEnable)if(btn!=null)SetSelected(btn.gameObject);
    }
    public void SetSelected(GameObject go){
    if(es!=null){
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(go);
    }
    }
}
