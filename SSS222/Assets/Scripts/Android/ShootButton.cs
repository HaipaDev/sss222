using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    public bool pressed;
    public float cooldown=0.1f;
    public float timeHold=0.1f;
    float timer;
    float timerHold;
    void Start(){
        
    }
    void Update(){
        if(pressed){
            if(timer<=0){
                FindObjectOfType<Player>().ShootButton(true);
                //timer=FindObjectOfType<Player>().shootTimer;
                timer=cooldown;
            }else{
                FindObjectOfType<Player>().ShootButton(false);
                timerHold-=Time.deltaTime;
                if(timerHold>timeHold && timer>0){timer=0;}
            }
        }
        //if (Input.GetMouseButtonUp(0)){FindObjectOfType<Player>().ShootButton(false);pressed=false;}
    }
    public void OnPointerDown(PointerEventData eventData){
        if(timer>0){timer-=Time.deltaTime;}
            pressed=true;
            //timer=cooldown;
        //}
     }
     public void OnPointerUp(PointerEventData eventData){
        FindObjectOfType<Player>().ShootButton(false);
        pressed=false;
        //if(timer<=0)timer=FindObjectOfType<Player>().shootTimer;
        if(timer>0){timer-=Time.deltaTime;}
     }
}
