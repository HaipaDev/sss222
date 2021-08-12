using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowStrict : MonoBehaviour{
    Vector2 targetPos;
    //new Vector2 selfPos;
    [SerializeField] GameObject target;
    [SerializeField] string targetTag;
    //public float dist;
    [SerializeField] public float xx;
    [SerializeField] public float yy;
    public GameObject targetObj;
    void Start(){
        if(targetObj==null){
            if(target!=null){if(GameObject.Find(target.name)!=null){targetObj=GameObject.Find(target.name);}else{targetObj=GameObject.Find(target.name+"(Clone)");}}
            else{if(targetTag!="")targetObj=GameObject.FindGameObjectWithTag(targetTag);}
        }
    }

    void Update(){
        if(targetObj==null){Destroy(gameObject);}
        else targetPos=new Vector2(targetObj.transform.position.x+xx, targetObj.transform.position.y+yy);
        transform.position=targetPos;
    }
}
