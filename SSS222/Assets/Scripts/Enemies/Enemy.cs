using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour{
    [HeaderAttribute("Enemy")]
    [SerializeField] public float health = 100f;
    float shotCounter;
    [SerializeField] bool shooting = false;
    [SerializeField] float minTimeBtwnShots=0.2f;
    [SerializeField] float maxTimeBtwnShots=1f;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] bool DBullets = false;
    [SerializeField] float bulletDist=0.35f;
    [SerializeField] bool randomizeWaveDeath = false;
    [SerializeField] bool flyOff = false;
    [HeaderAttribute("Drops & Points")]
    //[SerializeField] int scoreValue = 1;
    [SerializeField] public bool givePts = true;
    [SerializeField] int scoreValueStart = 1;
    [SerializeField] int scoreValueEnd = 10;
    [SerializeField] float enBallchanceInit = 30f;
    [SerializeField] float CoinchanceInit = 3f;
    [SerializeField] float powercoreChanceInit = 0f;
    [SerializeField] float xpAmnt = 0f;
    [HideInInspector] public float enBallchance;
    [HideInInspector] public float Coinchance;
    [HideInInspector] public float powercoreChance;
    
    [HeaderAttribute("Effects")]
    #region//VFX
    GameObject explosionVFX;
    GameObject explosionSmallVFX;
    GameObject flareHitVFX;
    #endregion
    #region//SFX
    /*
    [SerializeField] public AudioClip explosionSFX;
    [SerializeField] public AudioClip enemyHitSFX;
    [SerializeField] public AudioClip mlaserHitSFX;
    [SerializeField] public AudioClip hrocketHitSFX;
    [SerializeField] public AudioClip phaserHitSFX;
    [SerializeField] public AudioClip lsaberHitSFX;
    [SerializeField] public AudioClip lclawsHitSFX;
    [SerializeField] public AudioClip shadowbtHitSFX;
    [SerializeField] public AudioClip qrocketHitSFX;
    [SerializeField] public AudioClip procketHitSFX;
    [SerializeField] public AudioClip cbulletHitSFX;
    */
    #endregion
    #region//Prefabs
    [HeaderAttribute("Damage Dealers")]
    GameObject laserPrefab;
    GameObject mlaserPrefab;
    GameObject hrocketPrefab;
    GameObject phaserPrefab;
    GameObject lsaberPrefab;
    GameObject shadowbtPrefab;
    GameObject hlaserPrefab;
    GameObject qrocketPrefab;
    GameObject procketPrefab;
    GameObject procketExplPrefab;
    GameObject cbulletPrefab;
    GameObject lclawsPrefab;
    GameObject lclawsPartPrefab;
    GameObject mPulsePrefab;
    GameObject plaserPrefab;
    [HeaderAttribute("Drops")]
    GameObject enBallPrefab;
    GameObject coinPrefab;
    GameObject powercorePrefab;
    #endregion
    [HeaderAttribute("Others")]
    //[SerializeField] public bool cTagged=false;
    [SerializeField] public bool yeeted=false;
    [SerializeField] public GameObject dmgPopupPrefab;
    public bool dmgCounted;
    public float dmgCount;
    GameObject dmgCountPopup;

    Rigidbody2D rb;
    GameSession gameSession;
    Player player;
    Shake shake;

    // Start is called before the first frame update
    void Start(){
        enBallchance = Random.Range(0f, 100f);
        Coinchance = Random.Range(0f, 100f);
        powercoreChance = Random.Range(0f, 100f);
        if (enBallchance <= enBallchanceInit && enBallchanceInit>0){ enBallchance = 1; }
        if (Coinchance <= CoinchanceInit && CoinchanceInit>0) { Coinchance = 1; }
        if (powercoreChance <= powercoreChanceInit && powercoreChanceInit>0) { powercoreChance = 1; }
        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
        rb=GetComponent<Rigidbody2D>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        shake = GameObject.FindObjectOfType<Shake>().GetComponent<Shake>();

        SetPrefabs();
    }

    void SetPrefabs(){
        coinPrefab=GameAssets.instance.Get("Coin");
        enBallPrefab=GameAssets.instance.Get("EnBall");
        powercorePrefab=GameAssets.instance.Get("PowerCore");

        laserPrefab=GameAssets.instance.Get("Laser");
        mlaserPrefab=GameAssets.instance.Get("MLaser");
        hrocketPrefab=GameAssets.instance.Get("HRocket");
        phaserPrefab=GameAssets.instance.Get("Phaser");
        lsaberPrefab=GameAssets.instance.Get("LSaber");
        shadowbtPrefab=GameAssets.instance.Get("ShadowBt");
        qrocketPrefab=GameAssets.instance.Get("QRocket");
        procketPrefab=GameAssets.instance.Get("PRocket");
        procketExplPrefab=GameAssets.instance.Get("Plasma");
        cbulletPrefab=GameAssets.instance.Get("CBullet");
        lclawsPrefab=GameAssets.instance.Get("LClaws");
        lclawsPartPrefab=GameAssets.instance.Get("LClawsVFX");
        mPulsePrefab=GameAssets.instance.Get("MPulse");
        hlaserPrefab=GameAssets.instance.Get("HLaser");
        plaserPrefab=GameAssets.instance.Get("PLaser");

        explosionVFX=GameAssets.instance.GetVFX("Explosion");
        flareHitVFX=GameAssets.instance.GetVFX("FlareHit");
        explosionSmallVFX=GameAssets.instance.GetVFX("ExplosionSmall");
    }
    void Update(){
        if (shooting){Shoot();}
        if(flyOff){FlyOff();}
        Die();
        DestroyOutside();
        DispDmgCountUp();
    }
    
    private void Shoot(){
        shotCounter -= Time.deltaTime;
        if(shotCounter<=0f){
            if(DBullets!=true){
                var bt=Instantiate(bullet, transform.position,Quaternion.identity) as GameObject;
                bt.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
            }else{
                var pos1 = new Vector2(transform.position.x+bulletDist,transform.position.y);
                var bt1 = Instantiate(bullet, pos1, Quaternion.identity) as GameObject;
                bt1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                var pos2 = new Vector2(transform.position.x - bulletDist, transform.position.y);
                var bt2 = Instantiate(bullet, pos2, Quaternion.identity) as GameObject;
                bt2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
            }
        }
    }
    private void FlyOff(){
        if(player==null){
            shooting=false;
            rb.velocity=new Vector2(0,3f);
        }
    }
    
    public void Die(){
        if (health <= 0 && health!=-1000){
            int scoreValue = Random.Range(scoreValueStart,scoreValueEnd);
            if(givePts==true){
                gameSession.AddToScore(scoreValue);
                if(enBallchance==1){ Instantiate(enBallPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                if(Coinchance==1){ Instantiate(coinPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                if(powercoreChance==1){ Instantiate(powercorePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                gameSession.AddEnemyCount();
                if(xpAmnt>0){gameSession.AddXP(xpAmnt);}else if(xpAmnt<0){gameSession.SubXP(xpAmnt);}
                givePts=false;
            }
            AudioManager.instance.Play("Explosion");
            if(GetComponent<GoblinDrop>()!=null)GetComponent<GoblinDrop>().DropPowerup();
            if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
            //if(GetComponent<ParticleDelay>()!=null){GetComponent<ParticleDelay>().on=true;health=-1000;Destroy(gameObject,0.05f);}
            /*if(GetComponent<ParticleDelay>()==null){*/GameObject explosion = Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);Destroy(explosion, 0.5f);Destroy(gameObject,0.01f);//}
            shake.CamShake();
        }
    }
    private void OnDestroy() {
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        if(randomizeWaveDeath==true){ gameSession.EVscore = gameSession.EVscoreMax; }
    }
    private void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){if(yeeted==true){givePts=true; health=-1; Die();} else{ Destroy(gameObject,0.001f); if(GetComponent<GoblinDrop>()!=null){Destroy(GetComponent<GoblinDrop>().powerup);}}}
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(!other.CompareTag(tag)){
            DamageDealer damageDealer=other.gameObject.GetComponent<DamageDealer>();
            if(!damageDealer){ return; }
            var dmg = damageDealer.GetDamage();

            var Lname = laserPrefab.name;
            if (other.gameObject.name.Contains(Lname)) { dmg = damageDealer.GetDamageLaser(); Destroy(other.gameObject); AudioManager.instance.Play("EnemyHit"); }
            
            var MLname = mlaserPrefab.name;
            if (other.gameObject.name.Contains(MLname)){ 
                AudioManager.instance.Play("MLaserHit");
                /*var mlaserHitSound = other.GetComponent<RandomSound>().sound;
                if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                AudioSource.PlayClipAtPoint(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
                dmg = damageDealer.GetDamageMiniLaser(); Destroy(other.gameObject);}
            
            var HRname = hrocketPrefab.name;
            if (other.gameObject.name.Contains(HRname)) { dmg = damageDealer.GetDamageHRocket(); Destroy(other.gameObject); AudioManager.instance.Play("HRocketHit");
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            var LSabername = lsaberPrefab.name;
            if (other.gameObject.name.Contains(LSabername)){ dmg = (float)System.Math.Round(damageDealer.GetDamageLSaber()*9f,2); AudioManager.instance.Play("LSaberHit"); }

            //var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            //if (other.gameObject.name != Pname && other.gameObject.name != Pname1){dmg=0;}

            var LClawsname = lclawsPrefab.name;
            if (other.gameObject.name.Contains(LClawsname)){ dmg = (float)System.Math.Round(damageDealer.GetDamageLSaber()/3,2); AudioManager.instance.Play("LClawsHit"); FindObjectOfType<Player>().energy-=1f;}
            var LClawsPartname = lclawsPartPrefab.name;
            if (other.gameObject.name.Contains(LClawsPartname)){ dmg = damageDealer.GetDamageLClaws(); AudioManager.instance.Play("LClawsHit"); }

            var shadowbtName = shadowbtPrefab.name;
            if (other.gameObject.name.Contains(shadowbtName)) { dmg = damageDealer.GetDamageShadowBT(); AudioManager.instance.Play("ShadowHit"); FindObjectOfType<Player>().health-=1; FindObjectOfType<Player>().DMGPopUpHud(1);}
            
            var cBulletname = cbulletPrefab.name;
            if (other.gameObject.name.Contains(cBulletname)) { dmg = damageDealer.GetDamageCBullet(); AudioManager.instance.Play("CStreamHit");}

            var QRname = qrocketPrefab.name;
            if (other.gameObject.name.Contains(QRname)) { dmg = damageDealer.GetDamageQRocket(); Destroy(other.gameObject); AudioManager.instance.Play("QRocketHit");
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }
            var PRname = procketPrefab.name;
            if (other.gameObject.name.Contains(PRname)) { dmg = damageDealer.GetDamagePRocket(); //AudioSource.PlayClipAtPoint(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                //var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }var PLname = plaserPrefab.name;
            if (other.gameObject.name.Contains(PLname)) { dmg = other.GetComponent<DamageOverDist>().dmg; Destroy(other.gameObject); AudioManager.instance.Play("PLaserHit");//AudioSource.PlayClipAtPoint(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                //var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            var PRExplname = procketExplPrefab.name;
            if (other.gameObject.name.Contains(PRExplname)) { dmg = damageDealer.GetDamagePRocketExpl(); GetComponent<Rigidbody2D>().velocity = Vector2.up*6f; yeeted=true; }// AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            
            var mPulsename = mPulsePrefab.name;
            if (other.gameObject.name.Contains(mPulsename)) { dmg = damageDealer.GetDamageMPulse(); AudioManager.instance.Play("MLaserHit");}

            if(GetComponent<VortexWheel>()!=null){
                if(!other.gameObject.name.Contains(HRname)&&!other.gameObject.name.Contains(QRname)){
                    dmg/=3;
                }
            }
            health -= dmg;
            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            var flare = Instantiate(flareHitVFX, new Vector2(transform.position.x,transform.position.y - 0.5f), Quaternion.identity);
            Destroy(flare.gameObject, 0.3f);
            if(gameSession.dmgPopups==true&&dmg!=0){
                if(!other.gameObject.name.Contains(PLname)){
                    GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,other.transform.position);
                    dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,1).ToString();
                }else{
                    dmgCount+=dmg;
                    if(dmgCounted==false)StartCoroutine(DispDmgCount(other.transform.position));
                }
            }
        }else if(other.CompareTag(tag)){
            var hlaserName = hlaserPrefab.name; var hlaserName1 = hlaserPrefab.name + "(Clone)";
            if (other.gameObject.name == hlaserName || other.gameObject.name == hlaserName1) { this.givePts = false; this.health = -1; this.Die(); }
        }
    }
    IEnumerator DispDmgCount(Vector2 pos){
        dmgCounted=true;
        //In Update, DispDmgCountUp
        dmgCountPopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,pos);
        yield return new WaitForSeconds(0.2f);
        dmgCounted=false;
        dmgCount=0;
    }
    void DispDmgCountUp(){
        if(dmgCountPopup!=null)dmgCountPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmgCount,1).ToString();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag(tag))
        {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
            if (!damageDealer) { return; }
            float dmg = damageDealer.GetDamage();

            var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            if (other.gameObject.name == Pname || other.gameObject.name == Pname1) {dmg = damageDealer.GetDamagePhaser(); AudioManager.instance.Play("PhaserHit"); }
            //else { dmg = 0; }

            var LSabername = lsaberPrefab.name; var LSabername1 = lsaberPrefab.name + "(Clone)";
            if (other.gameObject.name == LSabername || other.gameObject.name == LSabername1) { dmg = damageDealer.GetDamageLSaber(); AudioManager.instance.Play("EnemyHit"); }
            //else { dmg = 0; }
            

            var PRExplname = procketExplPrefab.name; var PRExplname1 = procketExplPrefab.name + "(Clone)";
            if (other.gameObject.name == PRExplname || other.gameObject.name == PRExplname1) { dmg = damageDealer.GetDamagePRocketExpl(); }//AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            //else {dmg=0;}

            var cBulletname = cbulletPrefab.name; var cBulletname1 = cbulletPrefab.name + "(Clone)";
            if (other.gameObject.name == cBulletname || other.gameObject.name == cBulletname1) { dmg = damageDealer.GetDamageCBullet(); AudioManager.instance.Play("CStreamHit");}
            //else {dmg=0;}

            var LClawsname = lclawsPrefab.name; var LClawsname1 = lclawsPrefab.name + "(Clone)";
            if (other.gameObject.name == LClawsname || other.gameObject.name == LClawsname1){ dmg = (float)System.Math.Round(damageDealer.GetDamageLSaber()/3,2); FindObjectOfType<Player>().energy-=0.1f;}//AudioSource.PlayClipAtPoint(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y)); }

            var mPulsename = mPulsePrefab.name; var mPulsename1 = mPulsePrefab.name + "(Clone)";
            if (other.gameObject.name == mPulsename || other.gameObject.name == mPulsename1) { dmg = 0; AudioManager.instance.Play("PRocketHit");}

            health -= dmg;
            //Destroy(other.gameObject, 0.05f);

            //AudioSource.PlayClipAtPoint(enemyHitSFX, new Vector2(transform.position.x, transform.position.y));
            var flare = Instantiate(flareHitVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity);
            Destroy(flare.gameObject, 0.3f);
            if(gameSession.dmgPopups==true&&dmg!=0){
                GameObject dmgpopup=CreateOnUI.CreateOnUIFunc(dmgPopupPrefab,transform.position);
                dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,2).ToString();
            }
        }
    }
}
