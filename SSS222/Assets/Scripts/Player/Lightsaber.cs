using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour{
    [SerializeField] bool enemy=false;
    Player player;
    EnCombatant enCombatant;
    void Start()
    {
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
        player=FindObjectOfType<Player>();
        enCombatant=FindObjectOfType<EnCombatant>();
        //int numberOfObj = FindObjectsOfType<Lightsaber>().Length;if (numberOfObj > 1){Destroy(gameObject);}
    }
    private void Update() {
        if(enemy!=true)transform.localScale = player.transform.localScale;
        else{transform.localRotation=enCombatant.transform.localRotation;}
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(tag))
        {
            if(enemy!=true){
                if (other.gameObject.CompareTag("EnemyBullet"))
                {
                    /*DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                    if (!damageDealer) { return; }
                    
                    var Sname = soundwavePrefab.name; var Sname1 = soundwavePrefab.name + "(Clone)";
                    if (other.gameObject.name == Sname || other.gameObject.name == Sname1) { dmg = damageDealer.GetDamageSoundwave(); AudioSource.PlayClipAtPoint(soundwaveHitSFX, new Vector2(transform.position.x, transform.position.y)); }
                    var EBtname = EBtPrefab.name; var EBtname1 = EBtPrefab.name + "(Clone)";
                    if (other.gameObject.name == EBtname || other.gameObject.name == EBtname1) dmg = damageDealer.GetDamageEBt();*/

                    Destroy(other.gameObject, 0.05f);
                    //else { if (other.GetComponent<Enemy>().health > -1) { other.GetComponent<Enemy>().givePts = false; other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); } }

                    AudioManager.instance.Play("LSaberBlock");
                    //var flare = Instantiate(flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    //Destroy(flare.gameObject, 0.3f);
                }
            }else{
                if (other.gameObject.CompareTag("PlayerWeapons")){
                    if(other.GetComponent<Tag_PlayerWeaponBlockable>()!=null){
                        Destroy(other.gameObject, 0.05f);
                        AudioManager.instance.Play("LSaberBlockEnemy");
                    }if(other.GetComponent<Lightsaber>()!=null){
                        AudioManager.instance.Play("LSaberBlockEnemy");
                    }
                }
            }
        }
    }
}
