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
        var ps=transform.GetChild(0).GetComponent<ParticleSystem>();
        if(ps.startSize<10)ps.startSize*=10;
        transform.GetChild(0).GetComponent<UnityEngine.UI.Extensions.UIParticleSystem>().material=ps.GetComponent<Renderer>().material;
    }
    void Update(){
        var step=Time.unscaledDeltaTime;
        if(followSelectedButton){
            Tag_ButtonSelected selectedButton=null;
            foreach(Tag_ButtonSelected b in buttonsList.transform.GetComponentsInChildren<Tag_ButtonSelected>()){if(b.selected)selectedButton=b;}
            if(selectedButton!=null){
                step=speedFollowButton*Time.unscaledDeltaTime;
                transform.localPosition=Vector2.MoveTowards(transform.localPosition,new Vector2(selectedButton.transform.localPosition.x,selectedButton.transform.localPosition.y+spacingY),step);
            }
        }
        else if(followMouse){
            if((followMouseOnDrag&&Input.GetMouseButton(0))||!followMouseOnDrag){
                step=speedFollowMouse*Time.unscaledDeltaTime;
                if(((Input.mousePosition.x<transform.position.x+distanceFollowMouse&&Input.mousePosition.x>transform.position.x-distanceFollowMouse)&&
                (Input.mousePosition.y<transform.position.y+distanceFollowMouse&&Input.mousePosition.y>transform.position.y-distanceFollowMouse))||distanceFollowMouse==0){
                    transform.position=Vector2.MoveTowards(transform.position,Input.mousePosition,step);}
            }
        }
    }
    
}
