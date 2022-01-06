using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPellet : MonoBehaviour{
    public float healAmnt=0.1f;
    [SerializeField]GameObject targetObj;
    void Start(){
        targetObj=GetComponent<FollowOneObject>().targetObj;
    }

    void Update(){
        targetObj=GetComponent<FollowOneObject>().targetObj;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<CometRandomProperties>()!=null){other.GetComponent<CometRandomProperties>().healhitCount++;}
        if(targetObj!=null&&other.gameObject==targetObj){
            if(targetObj.GetComponent<Enemy>().health<targetObj.GetComponent<Enemy>().healthMax){
                targetObj.GetComponent<Enemy>().health+=healAmnt;
                WorldCanvas.instance.DMGPopup(healAmnt,transform.position,ColorInt32.Int2Color(ColorInt32.dmgHealColor),1,false);
            }
            GetComponent<FollowOneObject>().targetObj=null;
            targetObj=null;
            AudioManager.instance.Play("Heal");
        }
        
    }
}
