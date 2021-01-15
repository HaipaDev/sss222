using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HLaserKillThis : MonoBehaviour{
    [SerializeField] GameObject hlaserPrefab;
    [SerializeField] GameObject cargoDrop;
    [SerializeField] float delay=1f;
    private void OnTriggerEnter2D(Collider2D other){
        var hlaserName = hlaserPrefab.name;
        if(other.gameObject.name.Contains(hlaserName)){
            StartCoroutine(KillDelay());
            //if(!gameObject.name.Contains("Cargo"))Destroy(gameObject);
            //else StartCoroutine(Cargo());
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        StopCoroutine(KillDelay());
    }
    IEnumerator KillDelay(){
        yield return new WaitForSeconds(delay);
        if(gameObject.name.Contains("Cargo")){
            LaunchRadialBullets bt = gameObject.AddComponent(typeof(LaunchRadialBullets)) as LaunchRadialBullets;
            bt.SetProjectile(cargoDrop);//GameObject.Find("Coin"));
            bt.Shoot();
            GameAssets.instance.VFX("Explosion",transform.position,0.33f);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
    /*IEnumerator Cargo(){
        yield return new WaitForSeconds(1f);
        LaunchRadialBullets bt = gameObject.AddComponent(typeof(LaunchRadialBullets)) as LaunchRadialBullets;
        bt.SetProjectile(cargoDrop);//GameObject.Find("Coin"));
        bt.Shoot();
        GameAssets.instance.VFX("Explosion",transform.position,0.33f);
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);
    }*/
}
