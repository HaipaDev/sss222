using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowOneObject : MonoBehaviour{
    public GameObject targetObj;
    public float speedFollow=5f;
    [SerializeField] bool destroyAfter;
    [SerializeField] bool particlesOnDeath;
    [SerializeField] GameObject particlesPrefab;
    [SerializeField] float particleDestroyTime=0.05f;
    Rigidbody2D rb;
    void Start(){
        rb=GetComponent<Rigidbody2D>();
    }

    void Update(){
        float step=speedFollow*Time.deltaTime;
        if(targetObj!=null){var targetPos=targetObj.transform.position;
        transform.position=Vector2.MoveTowards(transform.position,targetPos,step);}
        if(targetObj==null){
            if(destroyAfter==true){Destroy(gameObject,0.001f);
                if(particlesOnDeath==true){
                    var part=Instantiate(particlesPrefab,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                    Destroy(part,particleDestroyTime);
                    }
                }
            }
    }
}
