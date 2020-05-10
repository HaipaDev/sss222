using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverUIEnable : MonoBehaviour{
    [SerializeField] GameObject obj;
    GameObject self;
    void Start(){
        self=this.gameObject;
    }
    void Update(){
        if(IsPointerOverUIElement()){
            obj.SetActive(true);
        }else{
            obj.SetActive(false);
        }
    }

     ///Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            //if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
            if (curRaysastResult.gameObject.name == self.name)
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {   
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }
}
