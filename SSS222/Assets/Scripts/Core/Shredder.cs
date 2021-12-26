using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag("Enemy")&&!other.gameObject.CompareTag("Player")&&!other.gameObject.CompareTag("World")&&!other.gameObject.name.Contains("MagneticPulse")){
            if(other.GetComponent<Tag_PlayerWeapon>()!=null){
                var en=other.GetComponent<Tag_PlayerWeapon>().energy;
                if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){en*=0.01f;}
                spawnReqsMono.AddMissed(en);
            }
            Destroy(other.gameObject,0.02f);
        }
    }
}
