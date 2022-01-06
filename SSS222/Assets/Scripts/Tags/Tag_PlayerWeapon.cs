using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PlayerWeapon:MonoBehaviour{
    public float energy;
    public bool charged;
    public bool blockable;
    public bool healing;
    [SerializeField] float opacity=0.3f;
    void Start(){
        if(GetComponent<Tag_PauseVelocity>()==null){
        gameObject.AddComponent<Tag_PauseVelocity>();
        if(GameRules.instance.playerWeaponsFade&&SaveSerial.instance.settingsData.playerWeaponsFade){
            var spr=GetComponent<SpriteRenderer>();
            var tempColor=spr.color;
            tempColor.a=opacity;
            spr.color=tempColor;
            GetComponent<Glow>().color.a=opacity;
        }
    }}
}