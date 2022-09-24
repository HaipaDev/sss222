using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Tag_PlayerWeapon:MonoBehaviour{
    [SerializeField] float opacity=0.3f;
    [SerializeField] public bool blockable;
    [SerializeField] public bool healing;
    [SerializeField] public bool shreddable=true;
    [DisableInEditorMode] public float energy;
    [DisableInEditorMode] public bool charged;
    void Start(){
        if(GetComponent<Tag_PauseVelocity>()==null)gameObject.AddComponent<Tag_PauseVelocity>();
        if(GameRules.instance.playerWeaponsFade&&SaveSerial.instance.settingsData.playerWeaponsFade){
            var spr=GetComponent<SpriteRenderer>();
            var tempColor=spr.color;
            tempColor.a=opacity;
            spr.color=tempColor;
            GetComponent<Glow>().color.a=opacity;
        }
        if(GameManager.maskMode!=0)if(GetComponent<SpriteRenderer>()!=null)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameManager.maskMode;
    }
}