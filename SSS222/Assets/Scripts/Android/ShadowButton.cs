using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShadowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
    public bool pressed;
    Vector2 defPos;
    [SerializeField]Vector2 lastPos;
    [SerializeField]Vector3 pPosScr;
    [SerializeField]Vector3 mPosScr;
    void Start(){
        defPos=transform.position;
    }
    void Update(){
        if(Player.instance.shadow!=true){GetComponent<Button>().enabled=false;GetComponent<Image>().enabled=false;transform.GetChild(0).gameObject.SetActive(false);}else{GetComponent<Button>().enabled=true;GetComponent<Image>().enabled=true;transform.GetChild(0).gameObject.SetActive(true);}
        pPosScr=Camera.main.WorldToScreenPoint(Player.instance.transform.position);
        mPosScr=Camera.main.WorldToScreenPoint(Player.instance.mousePos);
        if(pressed){
            transform.position=new Vector2(
            Mathf.Clamp(mPosScr.x,pPosScr.x-150,pPosScr.x+150),
            Mathf.Clamp(mPosScr.y,pPosScr.y-150,pPosScr.y+150)
            );
            lastPos=Player.instance.mousePos;
        }
        //if(Input.GetMouseButton(0)){FindObjectOfType<Player>().ShootButton(false);pressed=true;}
    }
    public void OnPointerDown(PointerEventData eventData){
        pressed=true;
     }
     public void OnPointerUp(PointerEventData eventData){
        Player.instance.ShadowButton(lastPos);
        pressed=false;
        transform.position=defPos;
     }
}
