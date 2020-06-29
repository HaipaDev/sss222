using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverDist : MonoBehaviour{
    [HeaderAttribute("Properties")]
    [SerializeField] float dmgBase=5.5f;
    [SerializeField] float dmgMin=0.3f;
    [SerializeField] float dmgMax=10;
    [SerializeField] bool gain;
    [SerializeField] float multiplier=1;
    [SerializeField] float distCap=0;
    [HeaderAttribute("Current")]
    [SerializeField]bool debug;
    [HideInInspector]public float dmg;
    [HideInInspector]public float dist;
    public float startTime;
    Rigidbody2D rb;
    void Start(){
        startTime=Time.time;
        rb=GetComponent<Rigidbody2D>();
        dmg=dmgBase;
    }

    void Update(){
        dmg=(float)System.Math.Round(dmg,2);
        dmg=Mathf.Clamp(dmg,dmgMin,dmgMax);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        dist=rb.velocity.y*(Time.time-startTime);
        //dmgBase*=(dist/dmgBase);
        if(dist>=distCap){
            if(gain==true)dmg+=(dist-distCap)*multiplier;
            else dmg-=(dist-distCap)*multiplier;
        }
        dmg=Mathf.Clamp(dmg,dmgMin,dmgMax);
        if(debug==true){
            Debug.Log("DMG: "+dmg);
            Debug.Log("Dist: "+dist);
        }
    }
}
