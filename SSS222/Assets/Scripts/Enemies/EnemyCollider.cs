using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour{
    public float dmgTimer;
    public List<colliTypes> collisionTypes=UniCollider.colliTypesForEn;
    void OnTriggerEnter2D(Collider2D other){
        //if(Player.instance.shadowRaycast[Player.instance.shadowRaycast.FindIndex(Player.instance.shadowRaycast.Count,(x) => x == this)]==this){Die();}
        if(!other.CompareTag(tag)&&!other.CompareTag("EnemyBullet")&&!other.CompareTag("Untagged")&&other.GetComponent<Tag_OutsideZone>()==null){
            if(other.GetComponent<Player>()!=null){if(other.GetComponent<Player>().dashing==true){GetComponent<Enemy>().Die();}}

            float dmg=UniCollider.TriggerEnter(other,transform,collisionTypes);

            if(GetComponent<VortexWheel>()!=null){if(!other.gameObject.name.Contains("Rocket")){dmg/=3;}}
            if(Player.instance!=null)dmg*=Player.instance.dmgMulti;

            if(dmg!=0){
                GetComponent<Enemy>().health-=dmg;
                UniCollider.DMG_VFX(0,other,transform,dmg);
            }
        }else if(other.CompareTag(tag)){
            if(other.gameObject.name.Contains(GameAssets.instance.Get("HLaser").name)||other.gameObject.name.Contains(GameAssets.instance.Get("VLaser").name)){GetComponent<Enemy>().giveScore=false;GetComponent<Enemy>().health=-1;GetComponent<Enemy>().Die();}
        }
    }
    void OnTriggerStay2D(Collider2D other){
    if(!other.CompareTag(tag)&&!other.CompareTag("Untagged")&&other.GetComponent<Tag_OutsideZone>()==null){
    if(dmgTimer<=0){
            float dmg=UniCollider.TriggerStay(other,transform,collisionTypes);

            if(Player.instance!=null)dmg*=Player.instance.dmgMulti;
            if(dmg!=0){
                GetComponent<Enemy>().health-=dmg;
                UniCollider.DMG_VFX(1,other,transform,dmg);
            }
            if(other.GetComponent<Tag_DmgPhaseFreq>()!=null)dmgTimer=other.GetComponent<Tag_DmgPhaseFreq>().dmgFreq;
    }else{dmgTimer-=Time.deltaTime;}
    }
    }
}