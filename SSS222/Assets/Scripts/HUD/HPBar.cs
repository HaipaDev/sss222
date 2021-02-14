using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour{
    Player player;
    public bool gclover = false;
    Sprite HPBarNormal;
    [SerializeField] Sprite HPBarGold;
    void Start(){
        player=FindObjectOfType<Player>();
        HPBarNormal=GetComponent<Image>().sprite;
    }
    void Update(){
    if(player!=null){
        GetComponent<Image>().fillAmount=(player.health/player.maxHP);
        if(gclover==true){GetComponent<Image>().sprite=HPBarGold;}
        else{GetComponent<Image>().sprite=HPBarNormal;}
    }
    }
}
