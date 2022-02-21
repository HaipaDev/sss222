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
            var flareObj=Instantiate(ShipCustomizationManager.instance.GetFlareVFX(),flaresParent);
                GameAssets.instance.TransformIntoUIParticle(flareObj,0,-1);flareObj.transform.localPosition=new Vector2(-44f,6f);
            flareObj=Instantiate(ShipCustomizationManager.instance.GetFlareVFX(),flaresParent);
                GameAssets.instance.TransformIntoUIParticle(flareObj,0,-1);flareObj.transform.localPosition=new Vector2(44f,6f);
            yield return new WaitForSeconds(psMain.startLifetime.constantMax+psMain.duration*2);
            if(flaresPreview)StartCoroutine(FlaresPreviewI());
        }else{yield return null;}
    }
}
