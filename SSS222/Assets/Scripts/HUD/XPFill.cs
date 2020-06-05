using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPFill : MonoBehaviour{
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite fillSprite;
    [SerializeField] GameObject particlePrefab;
    bool changed;
    [SerializeField] public string valueName;
    [SerializeField] public int valueReq;
    public int value;
    Image image;
    UpgradeMenu upgradeMenu;
    void Start(){
        image=GetComponent<Image>();
        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        //valueReq=
    }

    void Update(){
        value=(int)upgradeMenu.GetType().GetField(valueName).GetValue(upgradeMenu);
        if(value>=valueReq){
            if(changed==false){
                image.sprite=fillSprite;
                var pt=Instantiate(particlePrefab,transform);
                Destroy(pt,0.5f);
                changed=true;
            }
        }else{image.sprite=emptySprite;changed=false;}
    }
}
