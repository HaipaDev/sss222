using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tag_QualityDisable : MonoBehaviour{
    [SerializeField]QualityLevel level=(QualityLevel)1;
    [SerializeField]bool moreThan=false;
    [SerializeField]bool components;//by default disable all childObjects
    [SerializeField]bool button;
    [SerializeField]bool inOrderedList;
    void Update(){
        if(!components){
            if(!inOrderedList){
                foreach(Transform t in transform){
                    if(condition())t.gameObject.SetActive(false);
                    else t.gameObject.SetActive(true);
                }
            }else{
                foreach(Transform t in transform){
                    if(condition())t.gameObject.SetActive(false);
                    else t.gameObject.SetActive(true);
                }
                if(condition())GetComponent<RectTransform>().sizeDelta=Vector2.zero;
                else GetComponent<RectTransform>().sizeDelta=new Vector2(100,100);
                GetComponent<LayoutElement>().enabled=false;GetComponent<LayoutElement>().enabled=true;
            }
        }else{
            if(!button){
                foreach(MonoBehaviour c in GetComponents(typeof(MonoBehaviour))){
                if(c!=this){
                    if(condition()){c.enabled=false;}
                    else{c.enabled=true;}
                }
                }
            }else{
                if(condition())GetComponent<Button>().interactable=false;
                else GetComponent<Button>().interactable=true;
            }
        }
    }
    bool condition(){if((!moreThan&&QualitySettings.GetQualityLevel()<=(int)level) || (moreThan&&QualitySettings.GetQualityLevel()>=(int)level))return true;else return false;}
}
