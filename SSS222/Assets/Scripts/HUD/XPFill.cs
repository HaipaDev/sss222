using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPFill : MonoBehaviour{
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite fillSprite;
    [SerializeField] GameObject particlePrefab;
    public bool changed;
    [SerializeField] public string valueName;
    [SerializeField] public int valueReq;
    public int value;
    Image image;
    UpgradeMenu upgradeMenu;
    //Shake shake;
    void Start(){
        image=GetComponent<Image>();
        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        //shake = GameObject.FindObjectOfType<Shake>().GetComponent<Shake>();
    }

    void Update(){
        value=(int)upgradeMenu.GetType().GetField(valueName).GetValue(upgradeMenu);
        if(value>=valueReq){
            if(changed==false){
                image.sprite=fillSprite;
                UpgradeParticles();
                //shake.CamShake();
                changed=true;
            }
        }else{image.sprite=emptySprite;changed=false;}
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
