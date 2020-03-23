using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour{
    [HeaderAttribute("Powerups")]
    [SerializeField] GameObject CoinPrefab;
    [SerializeField] GameObject enBallPrefab;
    [SerializeField] GameObject armorPwrupPrefab;
    [SerializeField] GameObject armorUPwrupPrefab;
    [SerializeField] GameObject laser2PwrupPrefab;
    [SerializeField] GameObject laser3PwrupPrefab;
    [SerializeField] GameObject phaserPwrupPrefab;
    [SerializeField] GameObject hrocketPwrupPrefab;
    [SerializeField] GameObject mlaserPwrupPrefab;
    [SerializeField] GameObject lsaberPwrupPrefab;
    [SerializeField] GameObject flipPwrupPrefab;
    [SerializeField] GameObject gcloverPwrupPrefab;
    [SerializeField] GameObject shadowPwrupPrefab;
    [SerializeField] GameObject shadowBTPwrupPrefab;
    [HeaderAttribute("Damage Dealers")]
    [SerializeField] GameObject cometPrefab;
    [SerializeField] GameObject batPrefab;
    [SerializeField] GameObject enShip1Prefab;
    [SerializeField] GameObject soundwavePrefab;
    [SerializeField] GameObject EBtPrefab;
    [SerializeField] GameObject leechPrefab;
    [HeaderAttribute("Other")]
    [SerializeField] float dmgFreq=0.38f;
    public float dmgTimer;

    Player player;
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag(tag))
        {
            #region//Enemies
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
            {
                DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                if (!damageDealer) { return; }
                bool en = false;
                bool destroy = true;
                var dmg = damageDealer.GetDamage();

                var cometName = cometPrefab.name; var cometName1 = cometPrefab.name + "(Clone)";
                if (other.gameObject.name == cometName || other.gameObject.name == cometName1) { dmg = damageDealer.GetDamageComet(); en = true; }
                var batName = batPrefab.name; var batName1 = batPrefab.name + "(Clone)";
                if (other.gameObject.name == batName || other.gameObject.name == batName1) { dmg = damageDealer.GetDamageBat(); en = true; }
                var enShip1Name = enShip1Prefab.name; var enShip1Name1 = enShip1Prefab.name + "(Clone)";
                if (other.gameObject.name == enShip1Name || other.gameObject.name == enShip1Name1) { dmg = damageDealer.GetDamageEnemyShip1(); en = true; }

                var Sname = soundwavePrefab.name; var Sname1 = soundwavePrefab.name + "(Clone)";
                if (other.gameObject.name == Sname || other.gameObject.name == Sname1) { dmg = damageDealer.GetDamageSoundwave(); AudioSource.PlayClipAtPoint(player.soundwaveHitSFX, new Vector2(transform.position.x, transform.position.y)); }
                var EBtname = EBtPrefab.name; var EBtname1 = EBtPrefab.name + "(Clone)";
                if (other.gameObject.name == EBtname || other.gameObject.name == EBtname1) dmg = damageDealer.GetDamageEBt();

                var leechName = leechPrefab.name; var leechName1 = leechPrefab.name + "(Clone)";
                if (other.gameObject.name == leechName || other.gameObject.name == leechName1) { en = true;  destroy = false; }

                if(player.dashing==false){
                    player.health -= dmg;
                    if(destroy==true){
                        if (en != true) { Destroy(other.gameObject, 0.05f); }
                        else { other.GetComponent<Enemy>().givePts = false; other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                    }else{ }
                    player.damaged = true;
                    AudioSource.PlayClipAtPoint(player.shipHitSFX, new Vector2(transform.position.x, transform.position.y));
                    var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    Destroy(flare.gameObject, 0.3f);
                }
                else if(player.shadow==true && player.dashing==true){
                    //if (destroy == true){
                        if (en != true) { Destroy(other.gameObject, 0.05f); }
                        else { other.GetComponent<Enemy>().health = -1; other.GetComponent<Enemy>().Die(); }
                    //}else{ }
                }

                
            }
            #endregion
            #region//Powerups
            else if (other.gameObject.CompareTag("Powerups"))
            {
                var enBallName = enBallPrefab.name; var enBallName1 = enBallPrefab.name + "(Clone)";
                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1) { player.energy += player.energyBallGet; }

                var CoinName = CoinPrefab.name; var CoinName1 = CoinPrefab.name + "(Clone)";
                if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1) { gameSession.coins += 1; }


                var armorName = armorPwrupPrefab.name; var armorName1 = armorPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorName || other.gameObject.name == armorName1) { player.health += 25; player.energy += player.medkitEnergyGet; player.healed = true; }
                var armorUName = armorUPwrupPrefab.name; var armorUName1 = armorUPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == armorUName || other.gameObject.name == armorUName1) { player.health += 58; player.energy += player.medkitUEnergyGet; player.healed = true; }

                var flipName = flipPwrupPrefab.name; var flipName1 = flipPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == flipName || other.gameObject.name == flipName1) { player.flip = true; player.flipTimer = player.flipTime; }

                var gcloverName = gcloverPwrupPrefab.name; var gcloverName1 = gcloverPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    player.gclover = true; player.gcloverTimer = player.gcloverTime;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    GameObject gcloverexOVFX = Instantiate(player.gcloverOVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    Destroy(gcloverexOVFX, 1f);
                }
                var shadowName = shadowPwrupPrefab.name; var shadowName1 = shadowPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == shadowName || other.gameObject.name == shadowName1)
                {
                    player.shadow = true;
                    player.shadowTimer = player.shadowTime;
                    player.shadowed = true;
                    //GameObject gcloverexVFX = Instantiate(gcloverVFX, new Vector2(0, 0), Quaternion.identity);
                    //GameObject gcloverexOVFX = Instantiate(shadowEVFX, new Vector2(0, 0), Quaternion.identity);
                    //Destroy(gcloverexVFX, 1f);
                    //Destroy(gcloverexOVFX, 1f);
                }



                var laser2Name = laser2PwrupPrefab.name; var laser2Name1 = laser2PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser2Name || other.gameObject.name == laser2Name1) { player.powerup = "laser2"; player.energy += player.pwrupEnergyGet; }

                var laser3Name = laser3PwrupPrefab.name; var laser3Name1 = laser3PwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == laser3Name || other.gameObject.name == laser3Name1) { player.powerup = "laser3"; player.energy += player.pwrupEnergyGet; }

                var phaserName = phaserPwrupPrefab.name; var phaserName1 = phaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == phaserName || other.gameObject.name == phaserName1) { player.powerup = "phaser"; player.energy += player.pwrupEnergyGet; }

                var hrocketName = hrocketPwrupPrefab.name; var hrocketName1 = hrocketPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == hrocketName || other.gameObject.name == hrocketName1) { player.powerup = "hrockets"; player.energy += player.pwrupEnergyGet; }

                var minilaserName = mlaserPwrupPrefab.name; var minilaserName1 = mlaserPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == minilaserName || other.gameObject.name == minilaserName1) { player.powerup = "mlaser"; player.energy += player.pwrupEnergyGet; }

                var lsaberName = lsaberPwrupPrefab.name; var lsaberName1 = lsaberPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == lsaberName || other.gameObject.name == lsaberName1) { player.powerup = "lsaber"; player.energy += player.pwrupEnergyGet; }

                var shadowbtName = shadowBTPwrupPrefab.name; var shadowbtName1 = shadowBTPwrupPrefab.name + "(Clone)";
                if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1) { player.powerup = "shadowbt"; player.energy += player.pwrupEnergyGet; }


                if (other.gameObject.name == enBallName || other.gameObject.name == enBallName1)
                {
                    AudioSource.PlayClipAtPoint(player.energyBallSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if (other.gameObject.name == CoinName || other.gameObject.name == CoinName1)
                {
                    AudioSource.PlayClipAtPoint(player.coinSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if (other.gameObject.name == gcloverName || other.gameObject.name == gcloverName1)
                {
                    AudioSource.PlayClipAtPoint(player.gcloverSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else if (other.gameObject.name == shadowbtName || other.gameObject.name == shadowbtName1)
                {
                    AudioSource.PlayClipAtPoint(player.shadowbtPwrupSFX, new Vector2(transform.position.x, transform.position.y));
                }
                else
                {
                    AudioSource.PlayClipAtPoint(player.powerupSFX, new Vector2(transform.position.x, transform.position.y));
                }
                Destroy(other.gameObject, 0.05f);
            }
            #endregion
        }
    }
    private void OnTriggerStay2D(Collider2D other){
        if (!other.CompareTag(tag))
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
            {
                DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
                if (!damageDealer) { return; }
                //bool en = false;
                var dmg = damageDealer.GetDamage();

                var leechName = leechPrefab.name; var leechName1 = leechPrefab.name + "(Clone)";
                if (other.gameObject.name == leechName || other.gameObject.name == leechName1) { dmg = damageDealer.GetDamageLeech(); }

                if(dmgTimer<=0){
                    player.health -= dmg;
                    player.damaged = true;
                    AudioSource.PlayClipAtPoint(player.shipHitSFX, new Vector2(transform.position.x, transform.position.y));
                    //var flare = Instantiate(player.flareHitVFX, new Vector2(other.transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
                    //Destroy(flare.gameObject, 0.3f);
                    dmgTimer = dmgFreq;
                }else{ dmgTimer -= Time.deltaTime; }
            }
        }
    }
}
