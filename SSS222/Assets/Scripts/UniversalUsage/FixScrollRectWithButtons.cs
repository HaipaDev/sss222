using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class FixScrollRectWithButtons : MonoBehaviour{
    float timeHeldNeeded=0.08f;
    float timeUpNeeded=0.1f;
    float timeDelayNeeded=0.01f;
    [DisableInEditorMode][SerializeField]float timerHeld=-4;
    [DisableInEditorMode][SerializeField]float timerUp;
    [DisableInEditorMode][SerializeField]float timerDelay=-4;
    [DisableInEditorMode][SerializeField]float timerDelay2=-4;
    [DisableInEditorMode][SerializeField]MouseOperations.MouseEventFlags fakeButton;
    void Update(){
        if((timerUp<=0&&timerHeld>=timeHeldNeeded)||(Input.GetAxis("Mouse ScrollWheel")!=0)){
            if(transform.GetComponentsInChildren<Button>()[0].enabled){foreach(Button b in transform.GetComponentsInChildren<Button>()){
                b.enabled=false;b.GetComponent<Image>().raycastTarget=false;
                Debug.Log("Disabled components");
                if(Input.GetAxis("Mouse ScrollWheel")==0){timerDelay=timeDelayNeeded;timerDelay2=timeDelayNeeded*2;}
            }}
        }else if((timerUp>=timeUpNeeded)){//||(fakeButton==MouseOperations.MouseEventFlags.None&&Input.GetAxis("Mouse ScrollWheel")==0)){
            if(!transform.GetComponentsInChildren<Button>()[0].enabled){foreach(Button b in transform.GetComponentsInChildren<Button>()){
                b.enabled=true;b.GetComponent<Image>().raycastTarget=true;
                Debug.Log("Enabled components");
            }}
        }
        //if(timerHeld>=timeHeldNeeded&&Input.GetAxis("Mouse ScrollWheel")==0){
            if(timerDelay<=0&&timerDelay!=-4){
                fakeButton=MouseOperations.MouseEventFlags.LeftUp;MouseOperations.MouseEvent(fakeButton);
                timerDelay=-4;
            }
            if(timerDelay2<=0&&timerDelay2!=-4){
                fakeButton=MouseOperations.MouseEventFlags.LeftDown;MouseOperations.MouseEvent(fakeButton);
                timerDelay2=-4;
            }
        //}
        if(timerDelay>0){timerDelay-=Time.unscaledDeltaTime;}if(timerDelay2>0){timerDelay2-=Time.unscaledDeltaTime;}

        if(Input.GetMouseButtonDown(0)&&fakeButton!=MouseOperations.MouseEventFlags.LeftDown/*&&(Array.Find(transform.GetComponentsInChildren<Button>(),x=>x.gameObject==FindObjectOfType<UIInputSystem>().currentSelected)!=null)*/){timerUp=0;timerHeld=0;}
        if(Input.GetMouseButtonUp(0)&&fakeButton!=MouseOperations.MouseEventFlags.LeftUp&&fakeButton!=MouseOperations.MouseEventFlags.None){timerHeld=-4;timerDelay=-4;timerDelay2=-4;fakeButton=MouseOperations.MouseEventFlags.None;}

        if(Input.GetMouseButton(0)&&fakeButton!=MouseOperations.MouseEventFlags.LeftDown){if(timerHeld>=0)timerHeld+=Time.unscaledDeltaTime;}
        else if(!Input.GetMouseButton(0)&&fakeButton!=MouseOperations.MouseEventFlags.LeftUp){if(timerUp>=0)timerUp+=Time.unscaledDeltaTime;}
    }
}
