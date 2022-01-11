using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour{
    [SerializeField]Vector2 startPos;
    void Awake(){
        if((Vector2)transform.localPosition!=Vector2.zero)startPos=transform.localPosition;
    }
    void Update(){
        transform.localScale=Player.instance.transform.localScale;
        if(startPos!=Vector2.zero){
            int ax=1;//if(Player.instance.localScale.x>Player.instance.shipScale);
            if(Player.instance.transform.localScale.x<Player.instance.shipScale){ax=-1;}
            if(Player.instance.transform.localScale.x!=Player.instance.shipScale)transform.localPosition=new Vector3(startPos.x,startPos.y+(ax*0.05f*Player.instance.shipScale),0.01f);
        }//i dont even know anymore whats that for
        else startPos=transform.localPosition;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
            if(other.GetComponent<Tag_EnemyWeapon>()!=null){
                    if(other.GetComponent<Tag_EnemyWeapon>().blockable){
                    Destroy(other.gameObject,0.01f);
                    AudioManager.instance.Play("LSaberBlock");
                }
            }
        }
    }
}
