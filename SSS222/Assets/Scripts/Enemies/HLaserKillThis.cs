using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLaserKillThis : MonoBehaviour{
    [SerializeField] float delay=1f;
    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.name.Contains(AssetsManager.instance.Get("HLaser").name)||other.gameObject.name.Contains(AssetsManager.instance.Get("VLaser").name))StartCoroutine(KillDelay());
    }
    private void OnTriggerExit2D(Collider2D other){StopCoroutine(KillDelay());}
    IEnumerator KillDelay(){
        yield return new WaitForSeconds(delay);
        if(GetComponent<CargoShip>()!=null){GetComponent<CargoShip>().health=0;}else{Destroy(gameObject);}
    }
    /*IEnumerator Cargo(){
        yield return new WaitForSeconds(1f);
        LaunchRadialBullets bt = gameObject.AddComponent(typeof(LaunchRadialBullets)) as LaunchRadialBullets;
        bt.SetProjectile(cargoDrop);//GameObject.Find("Coin"));
        bt.Shoot();
        AssetsManager.instance.VFX("Explosion",transform.position,0.33f);
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }*/
}
