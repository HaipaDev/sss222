using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class BlindnessUI : MonoBehaviour{
    public static BlindnessUI instance;
    [ChildGameObjectsOnly]public GameObject maskCutout;
    //[ChildGameObjectsOnly]public GameObject blackBg;
    public bool on;
    public bool scaleWithPlayer=true;
    public float scale=3;
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;}
    }
    void Update(){
        if(scaleWithPlayer){if(Player.instance!=null){if(Player.instance._hasStatus("blind")){scale=Mathf.Clamp(7-Player.instance.GetStatus("blind").strength,0,99);}else{on=false;}}else{on=false;}}else{scale=3;}
        foreach(Transform t in transform){
            if(t.gameObject.activeSelf!=on){t.gameObject.SetActive(on);}
        }
        /*
        blackBg.GetComponent<Image>().enabled=on;
        maskCutout.transform.GetComponent<Image>().enabled=on;//Mask img
        maskCutout.transform.GetChild(0).GetComponent<Image>().enabled=on;//Blindness overlay
        */
        maskCutout.transform.GetComponent<RectTransform>().localScale=new Vector2(scale,scale);//Mask img paent scale
    }
    void OnValidate(){
        foreach(Transform t in transform){
            if(t.gameObject.activeSelf!=on){t.gameObject.SetActive(on);}
        }
    }
}
