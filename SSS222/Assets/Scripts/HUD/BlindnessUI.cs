using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlindnessUI : MonoBehaviour{
    public bool on;
    public float scale=3;
    void Start(){
        
    }

    void Update(){
        if(FindObjectOfType<Player>()!=null){on=FindObjectOfType<Player>().blind;scale=7-FindObjectOfType<Player>().blindStrenght;}
        GetComponent<Image>().enabled=on;
        transform.parent.GetComponent<Image>().enabled=on;
        GetComponent<RectTransform>().localScale=new Vector2(scale,scale);
    }
}
