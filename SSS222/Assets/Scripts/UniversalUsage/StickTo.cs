using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickTo : MonoBehaviour{
    [SerializeField] bool enemies=true;
    [SerializeField] bool player=true;
    [SerializeField] bool sound=true;
    [SerializeField] string sfx;
    void Start(){
        
    }
    void Update(){
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if((other.gameObject.CompareTag("Enemy")&&other.gameObject.GetComponent<VortexWheel>()==null&&enemies==true)||(other.gameObject.CompareTag("Player")&&player==true)){
            GetComponent<FollowOneObject>().targetObj=other.gameObject;
            GetComponent<Rigidbody2D>().velocity=Vector2.zero;
            if(GetComponent<ParticleDelay>()!=null){GetComponent<ParticleDelay>().on=true;}
            if(sound==true){AudioManager.instance.Play(sfx);}
        }
    }
}
