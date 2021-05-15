using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindnessUI : MonoBehaviour{
    public bool on;
    public float scale=3;
    void Update(){
        if(Player.instance!=null){on=Player.instance.blind;scale=7-Player.instance.blindStrenght;}
        GetComponent<Image>().enabled=on;
        transform.parent.GetComponent<Image>().enabled=on;
        GetComponent<RectTransform>().localScale=new Vector2(scale,scale);
    }
}
