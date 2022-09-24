using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enlarge : MonoBehaviour{
    [SerializeField] float speed=0.5f;
    [SerializeField] float limit=10f;
    void Update(){
        if(!GameManager.GlobalTimeIsPaused){
            if(transform.localScale.x<limit && transform.localScale.y<limit){
                var step=speed*Time.deltaTime;
                var scaleChange=step+=speed;
                var scale=transform.localScale+=new Vector3(scaleChange,scaleChange,0);
                transform.localScale=scale;
            }else{Destroy(gameObject);}
        }
    }
}
