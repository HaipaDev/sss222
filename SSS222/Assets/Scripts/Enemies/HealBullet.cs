using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBullet : MonoBehaviour{
    public float healAmnt=0.1f;
    [SerializeField]GameObject targetObj;
    void Start(){
        targetObj=GetComponent<FollowOneObject>().targetObj;
    }

    void Update(){
        targetObj=GetComponent<FollowOneObject>().targetObj;
    }

    private void OnTriggerEnter2D(Collider2D other){
        /*
        if(other.GetComponent<Enemy>()!=null&&other.GetComponent<Tag_LivingEnemy>()!=null){other.GetComponent<Enemy>().health+=healAmnt;}
        else if(other.GetComponent<CometRandomProperties>()!=null){other.GetComponent<CometRandomProperties>().healhitCount++;}
        Destroy(gameObject,0.01f);
        */

        if(other.GetComponent<CometRandomProperties>()!=null){other.GetComponent<CometRandomProperties>().healhitCount++;}
        if(targetObj!=null){
            if(targetObj.GetComponent<HealingDrone>()!=null || other.GetComponent<HealingDrone>()==null){
                targetObj.GetComponent<Enemy>().health+=healAmnt;
                GetComponent<FollowOneObject>().targetObj=null;
                targetObj=null;
                AudioManager.instance.Play("Heal");
            }
        }
        
    }
}
