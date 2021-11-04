using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour{
    [SerializeField]Vector2 startPos;
    void IEnumerator(){
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        if((Vector2)transform.localPosition!=Vector2.zero)startPos=transform.localPosition;
    }
    void Update(){
        transform.localScale=Player.instance.transform.localScale;
        if(startPos!=Vector2.zero){
            int ax=1;//if(Player.instance.localScale.x>Player.instance.shipScale);
            if(Player.instance.transform.localScale.x<Player.instance.shipScale){ax=-1;}
            if(Player.instance.transform.localScale.x!=Player.instance.shipScale)transform.localPosition=new Vector3(startPos.x,startPos.y+(ax*0.05f*Player.instance.shipScale),0.01f);
        }
        else startPos=transform.localPosition;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
                if(other.gameObject.CompareTag("EnemyBullet")){
                    Destroy(other.gameObject, 0.05f);
                    AudioManager.instance.Play("LSaberBlock");
                    //GameObject flare=Instantiate(flareHitVFX,new Vector2(other.transform.position.x,transform.position.y+0.5f),Quaternion.identity);
                    //Destroy(flare,0.3f);
                }
        }
    }
}
