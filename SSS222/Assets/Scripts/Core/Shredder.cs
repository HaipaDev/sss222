using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour{
    private void OnTriggerEnter2D(Collider2D other){
        if(!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Player")){
            if(other.gameObject.CompareTag("PlayerWeapons")){if(FindObjectOfType<DisruptersSpawner>()!=null&&other.GetComponent<Tag_PlayerWeaponBlockable>()!=null)if(FindObjectOfType<DisruptersSpawner>().spawnGlareDevil)FindObjectOfType<DisruptersSpawner>().EnergyCountGlareDevil+=other.GetComponent<Tag_PlayerWeaponBlockable>().energy;}
            Destroy(other.gameObject,0.02f);
        }
    }
}
