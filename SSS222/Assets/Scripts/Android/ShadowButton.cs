using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShadowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    public bool pressed;
    public float cooldown=0.1f;
    float timer;
    Vector2 pos;
    Vector2 lastPos;
    void Start(){
        pos=transform.position;
    }
    void Update(){
        if(FindObjectOfType<Player>().shadow!=true){GetComponent<Button>().enabled=false;GetComponent<Image>().enabled=false;transform.GetChild(0).gameObject.SetActive(false);}else{GetComponent<Button>().enabled=true;GetComponent<Image>().enabled=true;transform.GetChild(0).gameObject.SetActive(true);}
        if(pressed){
            transform.position=Camera.main.WorldToScreenPoint(FindObjectOfType<Player>().mousePos);
            lastPos=FindObjectOfType<Player>().mousePos;
            //if(timer<=0){
                //FindObjectOfType<Player>().ShadowButton(true);
                //timer=FindObjectOfType<Player>().shootTimer;
                //timer=cooldown;
            //}
        }else{
            //FindObjectOfType<Player>().ShadowButton(false);
        }
        //if (Input.GetMouseButtonUp(0)){FindObjectOfType<Player>().ShootButton(false);pressed=false;}
    }
    public void OnPointerDown(PointerEventData eventData){
        //if(timer>0){timer-=Time.deltaTime;}
            pressed=true;
            
            //timer=cooldown;
        //}
     }
     public void OnPointerUp(PointerEventData eventData){
        FindObjectOfType<Player>().ShadowButton(lastPos);
        pressed=false;
        transform.position=pos;
        //if(timer<=0)timer=FindObjectOfType<Player>().shootTimer;
        //if(timer>0){timer-=Time.deltaTime;}
     }
}
