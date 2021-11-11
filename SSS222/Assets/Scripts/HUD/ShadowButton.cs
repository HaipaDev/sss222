using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShadowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    [SerializeField]public bool drag;
    [Header("Variables")]
    public bool pressed;
    Vector2 defPos;
    [SerializeField]Vector2 lastPos;
    [SerializeField]Vector3 pPosScr;
    [SerializeField]Vector3 mPosScr;
    bool enabledd=true;
    void Start(){
        defPos=transform.position;
        TurnOff();
        if(!GameRules.instance.dashingEnabled)Destroy(gameObject);
    }
    void Update(){
        if(Player.instance!=null){
            if(!Player.instance.shadow||((!drag&&SaveSerial.instance.settingsData.inputType!=InputType.touch)||
            (drag&&(SaveSerial.instance.settingsData.inputType!=InputType.mouse||!SaveSerial.instance.settingsData.dtapMouseShoot)))){
                TurnOff();}
            else if(Player.instance.shadow&&((!drag&&SaveSerial.instance.settingsData.inputType==InputType.touch)||
            (drag&&SaveSerial.instance.settingsData.inputType==InputType.mouse&&SaveSerial.instance.settingsData.dtapMouseShoot))){
                TurnOn();}
            pPosScr=Camera.main.WorldToScreenPoint(Player.instance.transform.position);
            mPosScr=Camera.main.WorldToScreenPoint(Player.instance.mousePos);
            if(pressed){
                transform.position=new Vector2(
                Mathf.Clamp(mPosScr.x,pPosScr.x-150,pPosScr.x+150),
                Mathf.Clamp(mPosScr.y,pPosScr.y-150,pPosScr.y+150)
                );
                lastPos=Player.instance.mousePos;
            }else if(!pressed&&drag){
                defPos=Player.instance.transform.position;
                transform.position=pPosScr;
                transform.localScale=Player.instance.transform.localScale;
            }
        }else{Destroy(gameObject);}
    }
    void TurnOn(){if(!enabledd){transform.GetChild(0).gameObject.SetActive(true);enabledd=true;}}
    void TurnOff(){if(enabledd){transform.GetChild(0).gameObject.SetActive(false);enabledd=false;}}
    public void OnPointerDown(PointerEventData eventData){
        if(enabledd)pressed=true;
    }
    public void OnPointerUp(PointerEventData eventData){
        if(enabledd){
            Player.instance.ShadowButton(lastPos);
            pressed=false;
            transform.position=defPos;
        }
    }
}
