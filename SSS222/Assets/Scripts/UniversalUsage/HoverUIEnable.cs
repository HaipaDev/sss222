using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverUIEnable : MonoBehaviour{
    [SerializeField] GameObject obj;
    [SerializeField] bool followMouse=true;
    void Update(){
        if(CheckAllOtherPointers()){obj.SetActive(true);}
        else{obj.SetActive(false);}

        if(followMouse){
            obj.transform.position=Input.mousePosition;
        }
    }
    bool CheckAllOtherPointers(){
        if(Array.Exists(FindObjectsOfType<HoverUIEnable>(),x=>x.IsPointerOverUIElement())){return true;}
        else{return false;}
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement(){
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults){
        for(int i=0;i<eventSystemRaysastResults.Count;i++){
            RaycastResult curRaysastResult=eventSystemRaysastResults[i];
            if(curRaysastResult.gameObject.name==gameObject.name)
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults(){
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position=Input.mousePosition;
        List<RaycastResult> raysastResults=new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
