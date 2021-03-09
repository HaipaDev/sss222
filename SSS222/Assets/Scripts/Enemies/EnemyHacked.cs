using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHacked : MonoBehaviour{
    [SerializeField] float time=2;
    float timer;
    bool wasShooting;
    void Start(){
        timer=time;
        if(GetComponent<Enemy>()!=null){
            wasShooting=GetComponent<Enemy>().shooting;
            GetComponent<Enemy>().shooting=false;
        }
    }

    void Update(){
        if(timer>=0){timer-=Time.deltaTime;}
        if(timer<=0){if(GetComponent<Enemy>()!=null){GetComponent<Enemy>().shooting=wasShooting;}Destroy(this,0.05f);}
    }
}
