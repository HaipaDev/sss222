using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Player") && !other.gameObject.name.Contains("MagneticPulse")){
            if(other.gameObject.CompareTag("PlayerWeapons")){}
            if(FindObjectOfType<DisruptersSpawner>()!=null&&other.GetComponent<Tag_PlayerWeaponBlockable>()!=null){
                var en=other.GetComponent<Tag_PlayerWeaponBlockable>().energy;
                if(other.GetComponent<Tag_DmgPhaseFreq>()!=null){en=0.05f;}
                FindObjectOfType<DisruptersSpawner>().AddMissed(en);}
            Destroy(other.gameObject,0.02f);
        }
    }
}
