using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour{
    public bool gclover = false;
    Sprite HPBarNormal;
    [SerializeField] Sprite HPBarGold;
    void Start(){
        HPBarNormal=GetComponent<Image>().sprite;
    }
    void Update(){
    if(Player.instance!=null){
        GetComponent<Image>().fillAmount=(Player.instance.health/Player.instance.maxHP);
        if(gclover==true){GetComponent<Image>().sprite=HPBarGold;}
        else{GetComponent<Image>().sprite=HPBarNormal;}
    }
    }
}
