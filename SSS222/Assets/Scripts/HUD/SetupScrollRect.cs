using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupScrollRect : MonoBehaviour{
    [SerializeField]GameObject go;
    [SerializeField]float setScrollbarPos=1;
    void Awake(){Fix();}
    void Start(){Fix();}
    void OnEnable(){Fix();}
    void Fix(){
        if(GetComponent<Mask>()!=null)GetComponent<Mask>().enabled=true;
        if(go!=null){go.SetActive(true);}
        if(GetComponentInChildren<Scrollbar>()!=null){GetComponentInChildren<Scrollbar>().value=setScrollbarPos;}
    }
    void OnGUI(){
        if(GetComponent<ScrollRect>()!=null)GetComponent<ScrollRect>().verticalNormalizedPosition+=Input.mouseScrollDelta.y*0.05f;
    }
}
