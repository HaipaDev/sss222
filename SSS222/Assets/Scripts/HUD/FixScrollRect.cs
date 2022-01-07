using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixScrollRect : MonoBehaviour{
    [SerializeField]GameObject go;
    void Start(){
        if(GetComponent<Mask>()!=null)GetComponent<Mask>().enabled=true;
        if(go!=null){go.SetActive(true);}
    }
    void OnGUI(){
        if(GetComponent<ScrollRect>()!=null)GetComponent<ScrollRect>().verticalNormalizedPosition+=Input.mouseScrollDelta.y*0.05f;
    }
}
