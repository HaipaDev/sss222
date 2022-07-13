using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ShipUI : MonoBehaviour{
    [DisableIf("@this.followMouse||this.followZones")][SerializeField] bool followSelectedButton=true;
    [ShowIf("followSelectedButton")][SerializeField] RectTransform buttonsList;
    [ShowIf("followSelectedButton")][SerializeField] float spacingY=-400f;
    [ShowIf("followSelectedButton")][SerializeField] float speedFollowButton=5000f;

    [DisableIf("@this.followSelectedButton||this.followZones")][SerializeField] bool followMouse=false;
    [ShowIf("followMouse")][SerializeField] bool followMouseOnDrag=true;
    [ShowIf("followMouse")][SerializeField] float speedFollowMouse=500f;
    [ShowIf("followMouse")][SerializeField] float distanceFollowMouse=200f;

    
    [DisableIf("@this.followMouse||this.followSelectedButton")][SerializeField] bool followZones=false;
    [ShowIf("followZones")][SerializeField] float spacingY_zone=-150f;
    [ShowIf("followZones")][SerializeField] float spacingY_zoneBoss=-200f;
    [ShowIf("followZones")][SerializeField] bool displayTravel=true;
    [ShowIf("followZones")][SerializeField] bool travelPosExactDistance=true;
    [ShowIf("followZones")][SerializeField] bool rotateTowardsTravelDest=true;

    [SerializeField] bool flaresPreview=true;
    [ShowIf("flaresPreview")][SerializeField] Transform flaresParent;

    RectTransform rt;
    void Start(){
        rt=GetComponent<RectTransform>();
        if(flaresPreview){StartCoroutine(FlaresPreviewI());}
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
        else if(followZones){
            var _zoneId=0;
            if(GameSession.instance.zoneSelected!=-1){_zoneId=GameSession.instance.zoneSelected;}
            if(GameSession.instance.zoneToTravelTo==-1){
                var _spacingY=spacingY_zone;
                if(GameCreator.instance.adventureZones[_zoneId].isBoss){_spacingY=spacingY_zoneBoss;}
                var _pos=new Vector2(GameCreator.instance.adventureZones[_zoneId].pos.x,
                    GameCreator.instance.adventureZones[_zoneId].pos.y+_spacingY);
                rt.anchoredPosition=_pos;
            }else{
                if(displayTravel){
                    var _pos=(GameCreator.instance.adventureZones[_zoneId].pos+GameCreator.instance.adventureZones[GameSession.instance.zoneToTravelTo].pos)/2;
                    if(travelPosExactDistance){
                        //_pos=(GameCreator.instance.adventureZones[GameSession.instance.zoneToTravelTo].pos-GameCreator.instance.adventureZones[_zoneId].pos)*(GameSession.instance.NormalizedZoneTravelTimeLeft());
                        //var ab=(GameCreator.instance.adventureZones[GameSession.instance.zoneToTravelTo].pos-GameCreator.instance.adventureZones[_zoneId].pos);
                        //_pos=GameCreator.instance.adventureZones[_zoneId].pos+(GameSession.instance.NormalizedZoneTravelTimeLeft()*ab.normalized);
                        _pos=Vector3.Lerp(GameCreator.instance.adventureZones[_zoneId].pos, GameCreator.instance.adventureZones[GameSession.instance.zoneToTravelTo].pos, Mathf.Abs(1-GameSession.instance.NormalizedZoneTravelTimeLeft()));
                    }
                    rt.anchoredPosition=_pos;
                    if(rotateTowardsTravelDest){
                        transform.rotation = GameAssets.QuatRotateTowards(GameCreator.instance.adventureZones[GameSession.instance.zoneToTravelTo].pos, rt.anchoredPosition, 90);//Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 60);
                    }
                }
            }
        }
        if(GetComponent<TrailVFX>()!=null){if(GetComponent<TrailVFX>().trailObj!=null){
            GameAssets.instance.TransformIntoUIParticle(GetComponent<TrailVFX>().trailObj);
            GetComponent<TrailVFX>().trailObj.transform.localPosition=GetComponent<TrailVFX>().offset*-200;
        }}
    }
    bool _mousePressedInBound=false;
    void MousePressedInBounds(){
        if((Input.GetMouseButtonDown(0)&&(Input.mousePosition.x<transform.position.x+distanceFollowMouse&&Input.mousePosition.x>transform.position.x-distanceFollowMouse)&&
        (Input.mousePosition.y<transform.position.y+distanceFollowMouse&&Input.mousePosition.y>transform.position.y-distanceFollowMouse))||distanceFollowMouse==0){_mousePressedInBound=true;}
        if(!Input.GetMouseButton(0)){_mousePressedInBound=false;}
    }

    IEnumerator FlaresPreviewI(){
        if(flaresParent.childCount==0&&ShipCustomizationManager.instance!=null){
            var ps=ShipCustomizationManager.instance.GetFlareVFX().GetComponent<ParticleSystem>();var psMain=ps.main;var dur=psMain.duration;
            MakeFlares();
            yield return new WaitForSeconds(psMain.startLifetime.constantMax+psMain.duration*2);
            if(flaresPreview)StartCoroutine(FlaresPreviewI());
        }else{yield return null;}
    }
    public void MakeFlares(){
        var flareObj=Instantiate(ShipCustomizationManager.instance.GetFlareVFX(),flaresParent);
            GameAssets.instance.TransformIntoUIParticle(flareObj,0,-1);flareObj.transform.localPosition=new Vector2(-44f,6f);
        flareObj=Instantiate(ShipCustomizationManager.instance.GetFlareVFX(),flaresParent);
            GameAssets.instance.TransformIntoUIParticle(flareObj,0,-1);flareObj.transform.localPosition=new Vector2(44f,6f);
    }
}
