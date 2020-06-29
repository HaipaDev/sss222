using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPFill : MonoBehaviour{
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite fillSprite;
    [SerializeField] GameObject particlePrefab;
    public bool changed;
    public bool shop;
    [SerializeField] public string valueName;
    [SerializeField] public int valueReq;
    public int value;
    Image img;
    UpgradeMenu upgradeMenu;
    Shop shopMenu;
    //Shake shake;
    void Start(){
        img=GetComponent<Image>();
        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        if(shop==true)shopMenu=FindObjectOfType<Shop>();
        //shake = GameObject.FindObjectOfType<Shake>().GetComponent<Shake>();
    }

    void Update(){
        if(shop!=true)value=(int)upgradeMenu.GetType().GetField(valueName).GetValue(upgradeMenu);
        else value=(int)shopMenu.GetType().GetField(valueName).GetValue(shopMenu);
        if(value>=valueReq){
            if(changed==false){
                img.sprite=fillSprite;
                UpgradeParticles();
                //shake.CamShake();
                changed=true;
            }
        }else{img.sprite=emptySprite;changed=false;}
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
