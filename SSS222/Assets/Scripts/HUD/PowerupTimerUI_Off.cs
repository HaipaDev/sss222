using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTimerUI_Off : MonoBehaviour{
    private void Start(){
        //StartCoroutine(Check());
    }
    /*IEnumerator Check(){
        yield return new WaitForSeconds(0.1f);
        if(Player.instance!=null){if(!Player.instance.weaponsLimited){Destroy(gameObject);}}
    }*/
    private void Update(){
        if(Player.instance!=null){
            if(Player.instance.powerup==Player.instance.powerupDefault||!Player.instance.weaponsLimited||Player.instance.powerupTimer==-4){
                foreach(Transform c in transform){if(c.gameObject.activeSelf==true)c.gameObject.SetActive(false);}
            }else{foreach(Transform c in transform){if(c.gameObject.activeSelf!=true)c.gameObject.SetActive(true);}}
        }
    }
}