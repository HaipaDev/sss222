using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_EnemyWeapon:MonoBehaviour{
    public bool blockable;
    void Start(){
        if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}
        if(GameSession.maskMode!=0)if(GetComponent<SpriteRenderer>()!=null)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
    }
}