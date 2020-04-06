using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightsaber : MonoBehaviour{
    [SerializeField] AudioClip lsaberBlockSFX;
    void Start()
    {
        int numberOfObj = FindObjectsOfType<Lightsaber>().Length;
        if (numberOfObj > 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(tag))
        {
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

                AudioSource.PlayClipAtPoint(lsaberBlockSFX, new Vector2(transform.position.x, transform.position.y));
                //var flare = Instantiate(flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                //Destroy(flare.gameObject, 0.3f);
            }
        }
    }
}
