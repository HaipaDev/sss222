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
        if(GameRules.instance.autoShootPlayer){gameObject.SetActive(false);}//Destroy(gameObject);}
    }
    void Update(){
        if(pressed){
            if(timer<=0){
                Player.instance.ShootButton(true);
                //timer=FindObjectOfType<Player>().shootTimer;
                timer=cooldown;
            }else{
                //
                timerHold-=Time.deltaTime;
                if(timerHold>timeHold && timer>0){timer=0;}
            }
        }else{
            Player.instance.ShootButton(false);
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
        Player.instance.ShootButton(false);
        pressed=false;
        //if(timer<=0)timer=FindObjectOfType<Player>().shootTimer;
        if(timer>0){timer-=Time.deltaTime;}
     }
}
