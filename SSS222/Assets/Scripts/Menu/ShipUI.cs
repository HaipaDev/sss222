using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShipUI : MonoBehaviour{
    [DisableIf("followMouse")][SerializeField] bool followSelectedButton=true;
    [ShowIf("followSelectedButton")][SerializeField] RectTransform buttonsList;
    [ShowIf("followSelectedButton")][SerializeField] float spacingY=-400f;
    [ShowIf("followSelectedButton")][SerializeField] float speedFollowButton=5000f;

    [DisableIf("followSelectedButton")][SerializeField] bool followMouse=false;
    [ShowIf("followMouse")][SerializeField] bool followMouseOnDrag=true;
    [ShowIf("followMouse")][SerializeField] float speedFollowMouse=500f;
    [ShowIf("followMouse")][SerializeField] float distanceFollowMouse=200f;

    RectTransform rt;
    void Start(){
        rt=GetComponent<RectTransform>();
        var ps=GetComponentInChildren<ParticleSystem>();
        if(ps.startSize<10)ps.startSize*=10;
        GetComponentInChildren<UnityEngine.UI.Extensions.UIParticleSystem>().material=ps.GetComponent<Renderer>().material;
    }
    void Update(){
        var step=Time.unscaledDeltaTime;
        if(followSelectedButton){
            step=speedFollowButton*Time.unscaledDeltaTime;
            ButtonScript selectedButton=null;
            foreach(ButtonScript b in buttonsList.transform.GetComponentsInChildren<ButtonScript>()){if(b.selected)selectedButton=b;}
            if(selectedButton!=null){
                transform.localPosition=Vector2.MoveTowards(transform.localPosition,new Vector2(selectedButton.transform.localPosition.x,selectedButton.transform.localPosition.y+spacingY),step);
            }
        }
        else if(followMouse){
            step=speedFollowMouse*Time.unscaledDeltaTime;
            MousePressedInBounds();
            if((followMouseOnDrag&&_mousePressedInBound)||!followMouseOnDrag){
                if(Input.GetMouseButton(0)||!followMouseOnDrag){
                    transform.position=Vector2.MoveTowards(transform.position,Input.mousePosition,step);
                }
            }
        }
    }
    bool _mousePressedInBound=false;
    void MousePressedInBounds(){
        if((Input.GetMouseButtonDown(0)&&(Input.mousePosition.x<transform.position.x+distanceFollowMouse&&Input.mousePosition.x>transform.position.x-distanceFollowMouse)&&
        (Input.mousePosition.y<transform.position.y+distanceFollowMouse&&Input.mousePosition.y>transform.position.y-distanceFollowMouse))||distanceFollowMouse==0){_mousePressedInBound=true;}
        if(!Input.GetMouseButton(0)){_mousePressedInBound=false;}
    }
}
