using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindnessUI : MonoBehaviour{
    public static BlindnessUI instance;
    public bool on;
    public bool scaleWithPlayer=true;
    public float scale=3;
    void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;}
    }
    void Update(){
        if(scaleWithPlayer){if(Player.instance!=null){if(Player.instance._hasStatus("blind")){scale=7-Player.instance.GetStatus("blind").strength;}else{on=false;}}else{on=false;}}
        GetComponent<Image>().enabled=on;
        transform.parent.GetComponent<Image>().enabled=on;
        GetComponent<RectTransform>().localScale=new Vector2(scale,scale);
    }
}
