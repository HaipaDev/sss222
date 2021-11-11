using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tag_CheatModeDisable : MonoBehaviour{
    [SerializeField]bool components;//by default disable all childObjects
    [SerializeField]bool button;
    [SerializeField]bool inOrderedList;
    void Update(){
        if(!components){
            if(!inOrderedList){
                foreach(Transform t in transform){
                    if(GameSession.instance.cheatmode!=true)t.gameObject.SetActive(false);
                    else t.gameObject.SetActive(true);
                }
            }else{
                foreach(Transform t in transform){
                    if(GameSession.instance.cheatmode!=true)t.gameObject.SetActive(false);
                    else t.gameObject.SetActive(true);
                }
                if(GameSession.instance.cheatmode!=true)GetComponent<RectTransform>().sizeDelta=Vector2.zero;
                else GetComponent<RectTransform>().sizeDelta=new Vector2(100,100);
            }
        }else{
            if(!button){
                foreach(MonoBehaviour c in GetComponents(typeof(MonoBehaviour))){
                if(c!=this){
                    if(GameSession.instance.cheatmode!=true){c.enabled=false;}
                    else{c.enabled=true;}
                }
                }
            }else{
                if(GameSession.instance.cheatmode!=true)GetComponent<Button>().interactable=false;
                else GetComponent<Button>().interactable=true;
            }
        }
    }
}
