using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTimerUI_Off : MonoBehaviour{
    private void Start(){
        StartCoroutine(Check());
    }
    IEnumerator Check(){
        yield return new WaitForSeconds(0.1f);
        if(FindObjectOfType<Player>()!=null){if(!FindObjectOfType<Player>().weaponsLimited){Destroy(gameObject);}}
    }
    private void Update() {
        if(FindObjectOfType<Player>()!=null){if(FindObjectOfType<Player>().weaponsLimited){
            if(FindObjectOfType<Player>().powerupTimer==-4){foreach(Transform c in transform){if(c.gameObject.activeSelf==true)c.gameObject.SetActive(false);}}else{foreach(Transform c in transform){if(c.gameObject.activeSelf!=true)c.gameObject.SetActive(true);}}
        }}
    }
}