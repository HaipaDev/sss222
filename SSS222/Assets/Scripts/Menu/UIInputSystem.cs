using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIInputSystem : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler{
    [SerializeField]EventSystem es;
    [SerializeField]Button btn;
    [SerializeField]bool inputSelecting=true;
    //[SerializeField]Vector2 mousePosCanv;
    [SerializeField]Vector2 mousePosPrev;
    //float inputSelectingTime=2;
    //[SerializeField]float inputSelectingTimer=-4;
    void Start(){
        es=FindObjectOfType<EventSystem>();
        if(es!=null)btn=es.firstSelectedGameObject.GetComponent<Button>();mousePosPrev=Input.mousePosition;
    }
    public void SetSelected(){
        es.SetSelectedGameObject(null);
        if(es!=null&&btn!=null)es.SetSelectedGameObject(btn.gameObject);
    }
    void Update(){
        //over=IsPointerOverUIObject();
        //mousePosCanv=MousePosToCanvas();
        if(Input.GetButtonDown("Horizontal")||Input.GetButtonDown("Vertical")){inputSelecting=true;mousePosPrev=Input.mousePosition;}//inputSelectingTimer=inputSelectingTime;}
        else{if((Vector2)Input.mousePosition!=mousePosPrev){inputSelecting=false;}}
        //else{if(inputSelectingTimer==-4){inputSelectingTimer=inputSelectingTime;}else{if(inputSelectingTimer>0){inputSelectingTimer-=Time.unscaledDeltaTime;}else if(inputSelectingTimer<=0&&inputSelectingTimer!=-4){inputSelecting=false;}}}
        if(this.gameObject==es.gameObject&&!inputSelecting){if(GetButtonUnderMouse()!=null)btn=GetButtonUnderMouse();SetSelected();}
        /*Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit=Physics2D.GetRayIntersection(ray,Mathf.Infinity);
        if(hit.collider!=null&&hit.collider.GetComponent<Button>()!=null){btn=hit.collider.GetComponent<Button>();SetSelected();}*/
    }
    public Button FindClosestButton(){
        KdTree<Button> Buttons=new KdTree<Button>();
        Button[] ButtonsArr;
        ButtonsArr=FindObjectsOfType<Button>();
        foreach(Button button in ButtonsArr){Buttons.Add(button);}
        Button closest=Buttons.FindClosest(Input.mousePosition);
        return closest;
    }
    public Button GetButtonUnderMouse(){
        PointerEventData eventDataCurrentPosition=new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position=(Vector2)Input.mousePosition;
        List<RaycastResult> results=new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
        Button btn=null;
        foreach(RaycastResult rr in results){if(rr.gameObject.GetComponent<Button>()!=null){btn=rr.gameObject.GetComponent<Button>();}}//else{btn=null;}}
        return btn;
        /*Button btn=null;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 2.0F)){if(hit.transform.GetComponent<Button>()!=null){btn=hit.transform.GetComponent<Button>();}}else{}
        return btn;*/
    }
    Vector3 MousePosToCanvas(){return CanvasPositioningExtensions.ScreenToCanvasPosition(FindObjectOfType<Canvas>(),Input.mousePosition);}
    public void OnPointerEnter(PointerEventData eventData){
        //if(eventData.pointerCurrentRaycast.gameObject!=null){Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);if(eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>()!=null){btn=eventData.pointerCurrentRaycast.gameObject.GetComponent<Button>();}}
        foreach(GameObject go in eventData.hovered){Debug.Log(go.name);if(go.GetComponent<Button>()!=null){btn=go.GetComponent<Button>();}}
    }
    public void OnPointerExit(PointerEventData eventData){}
    public static bool IsPointerOverUIObject(){
        PointerEventData eventDataCurrentPosition=new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position=(Vector2)Input.mousePosition;
        List<RaycastResult> results=new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition,results);
        return results.Count>0;
    }
}
