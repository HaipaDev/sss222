using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPFill : MonoBehaviour{
    [SerializeField] bool replaceSprites=true;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite fillSprite;
    [SerializeField] GameObject particlePrefab;
    public bool changed;
    public bool shop;
    [SerializeField] public string valueName;
    [SerializeField] public int valueReq;
    public int value;
    Image img;
    //Shake shake;
    void Start(){
        img=GetComponent<Image>();
        if(replaceSprites){
            if(transform.root.gameObject.name.Contains("UpgradeCanvas")){
                emptySprite=GameAssets.instance.Spr("upgradeEmpty");
                fillSprite=GameAssets.instance.Spr("upgradeFill");
                particlePrefab=GameAssets.instance.GetVFX("UpgradeVFX");
            }
        }
        //shake = GameObject.FindObjectOfType<Shake>().GetComponent<Shake>();
    }

    void Update(){
        if(valueReq!=0){
            if(shop!=true)value=(int)UpgradeMenu.instance.GetType().GetField(valueName).GetValue(UpgradeMenu.instance);
            else value=(int)Shop.instance.GetType().GetField(valueName).GetValue(Shop.instance);
            if(value>=valueReq){
                if(changed==false){
                    img.sprite=fillSprite;
                    UpgradeParticles();
                    //shake.CamShake();
                    changed=true;
                }
            }else{img.sprite=emptySprite;changed=false;}
        }else{
            if(shop!=true)value=Convert.ToInt32(UpgradeMenu.instance.GetType().GetField(valueName).GetValue(UpgradeMenu.instance));
            else value=Convert.ToInt32(Shop.instance.GetType().GetField(valueName).GetValue(Shop.instance));
            if(value==1){
                if(changed==false){
                    img.sprite=fillSprite;
                    UpgradeParticles();
                    changed=true;
                }
            }else{img.sprite=emptySprite;changed=false;}
        }
    }
    public void UpgradeParticles(){
        var pt=Instantiate(particlePrefab,transform);
        var ps=pt.GetComponent<ParticleSystem>();
        var sh=ps.shape;
        sh.radius*=GetComponent<RectTransform>().sizeDelta.x/160;
        var pm=ps.main;
        pm.maxParticles*=Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x/110);
        var pe=ps.emission;
        pe.rateOverTime=Mathf.RoundToInt(GetComponent<RectTransform>().sizeDelta.x);
        Destroy(pt,0.5f);
    }
}
