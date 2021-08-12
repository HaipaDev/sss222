using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour{
    Vector2 targetPos;
    [SerializeField] GameObject target;
    [SerializeField] string targetTag;
    //public float dist;
    [SerializeField] float xx;
    [SerializeField] float yy;
    public GameObject targetObj;
    void Start(){
        if(targetObj==null){
            if(target!=null){targetObj=GameObject.FindGameObjectWithTag(target.tag);}
            else{if(targetTag!="")targetObj=GameObject.FindGameObjectWithTag(targetTag);}
        }
    }

    void Update(){
        if(targetObj!=null){
        targetPos=new Vector2(targetObj.transform.position.x+xx,targetObj.transform.position.y+yy);
        transform.position=Camera.main.WorldToScreenPoint(targetPos);
        }else{StartCoroutine(DestroyIfNull());}
    }
    IEnumerator DestroyIfNull(){
        yield return new WaitForSeconds(0.5f);
        if(targetObj==null){Destroy(gameObject);}
    }
}