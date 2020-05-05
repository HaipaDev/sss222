using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBullet : MonoBehaviour{
    public float healAmnt=0.1f;
    [SerializeField]GameObject targetObj;
    //[SerializeField] GameObject healParticlePrefab;
    void Start(){
        targetObj=GetComponent<FollowOneObject>().targetObj;
    }

    void Update(){
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        //if(other==targetObj){
        if(targetObj.GetComponent<HealingDrone>()!=null || other.GetComponent<HealingDrone>()==null){
            targetObj.GetComponent<Enemy>().health+=healAmnt;
            GetComponent<FollowOneObject>().targetObj=null;
            targetObj=null;
        }
    }
}
