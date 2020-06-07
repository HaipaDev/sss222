using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDelay : MonoBehaviour{
    [SerializeField] public bool on;
    [SerializeField] public bool destroy;
    [SerializeField] public float delay=1f;
    [SerializeField] public GameObject prefab;
    [SerializeField] public bool sound=true;
    [SerializeField] public string sfx;
    float timer;
    void Start(){
        timer=delay;
    }
    void Update(){
        if(on==true)timer-=Time.deltaTime;
        if(timer<=0 && on==true){
            var part=Instantiate(prefab,transform.position,Quaternion.identity);
            Destroy(part,0.5f);
            if(sound==true)AudioManager.instance.Play(sfx);
            if(destroy==true)Destroy(gameObject);
        }
    }
}
