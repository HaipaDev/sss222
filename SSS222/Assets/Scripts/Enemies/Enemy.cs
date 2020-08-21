using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour{
    [Header("Enemy")]
    [SerializeField] public string Name;
    [SerializeField] public Vector2 size = Vector2.one;
    [SerializeField] public Sprite spr;
    [SerializeField] public float health = 100f;
    float shotCounter;
    [SerializeField] public bool shooting = false;
    [SerializeField] float minTimeBtwnShots=0.2f;
    [SerializeField] float maxTimeBtwnShots=1f;
    [SerializeField] public GameObject bullet;
    [SerializeField] float bulletSpeed = 8f;
    [SerializeField] bool DBullets = false;
    [SerializeField] float bulletDist=0.35f;
    [SerializeField] bool randomizeWaveDeath = false;
    [SerializeField] bool flyOff = false;
    [Header("Drops & Points")]
    //[SerializeField] int scoreValue = 1;
    [SerializeField] public bool givePts = true;
    [SerializeField] int scoreValueStart = 1;
    [SerializeField] int scoreValueEnd = 10;
    [SerializeField] float enBallChanceInit = 30f;
    [SerializeField] float coinChanceInit = 3f;
    [SerializeField] float powercoreChanceInit = 0f;
    [SerializeField] float xpAmnt = 0f;
    [HideInInspector] public float enBallChance;
    [HideInInspector] public float coinChance;
    [HideInInspector] public float powercoreChance;
    [SerializeField] public GameObject specialDrop;
    
    [Header("Effects")]
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
    [Header("Damage Dealers")]
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
    [Header("Drops")]
    GameObject enBallPrefab;
    GameObject coinPrefab;
    GameObject powercorePrefab;
    #endregion
    [Header("Others")]
    //[SerializeField] public bool cTagged=false;
    //[SerializeField] public float curSpeed;
    [SerializeField] public bool yeeted=false;
    [HideInInspector] public GameObject dmgPopupPrefab;
    public bool dmgCounted;
    public float dmgCount;
    GameObject dmgCountPopup;

    Rigidbody2D rb;
    GameSession gameSession;
    Player player;
    Shake shake;

    private void Awake() {
        StartCoroutine(SetValues());
    }
    IEnumerator SetValues(){
        yield return new WaitForSeconds(0.02f);
        //Set values
        var i=GameRules.instance;
        if(i!=null){
            EnemyClass e=null;
            foreach(EnemyClass enemy in i.enemies){if(enemy.name==Name){e=enemy;}}
            if(e!=null){
            if(GetComponent<CometRandomProperties>()==null){size=e.size;transform.localScale=size;spr=e.spr;GetComponent<SpriteRenderer>().sprite=spr;}
            health=e.health;
            shooting=e.shooting;
            minTimeBtwnShots=e.minTimeBtwnShots;
            maxTimeBtwnShots=e.maxTimeBtwnShots;
            bullet=e.bullet;
            bulletSpeed=e.bulletSpeed;
            DBullets=e.DBullets;
            bulletDist=e.bulletDist;
            randomizeWaveDeath=e.randomizeWaveDeath;
            flyOff=e.flyOff;
            givePts=e.givePts;
            scoreValueStart=e.scoreValueStart;
            scoreValueEnd=e.scoreValueEnd;
            enBallChanceInit=e.enBallChanceInit;
            coinChanceInit=e.coinChanceInit;
            powercoreChanceInit=e.powercoreChanceInit;
            xpAmnt=e.xpAmnt;
            specialDrop=e.specialDrop;
            }
        }
    }
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
        shake = GameObject.FindObjectOfType<Shake>();

        enBallChanceInit*=gameSession.enballDropMulti;
        coinChanceInit*=gameSession.coinDropMulti;
        powercoreChanceInit*=gameSession.coreDropMulti;

        enBallChance = Random.Range(0f, 100f);
        coinChance = Random.Range(0f, 100f);
        powercoreChance = Random.Range(0f, 100f);
        if(enBallChance <= enBallChanceInit && enBallChanceInit>0){ enBallChance = 1; }
        if(coinChance <= coinChanceInit && coinChanceInit>0) { coinChance = 1; }
        if(powercoreChance <= powercoreChanceInit && powercoreChanceInit>0) { powercoreChance = 1; }
        if(!GameRules.instance.energyOnPlayer)enBallChance=0;
        if(!GameSession.instance.shopOn)coinChance=0;
        if(!GameSession.instance.upgradesOn)powercoreChance=0;
        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);

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

        dmgPopupPrefab=GameAssets.instance.GetVFX("DMGPopup");
    }
    void Update(){
        if (shooting){Shoot();}
        if(flyOff){FlyOff();}
        Die();
        DestroyOutside();
        DispDmgCountUp();
        //Rotate if Stinger
        if(gameObject.name.Contains("Stinger")){
        if(Time.timeScale>0.0001f){
        if(FindObjectOfType<Player>()!=null){
            if(transform.position.x-FindObjectOfType<Player>().transform.position.x<0.2f){
            if(transform.position.y-FindObjectOfType<Player>().transform.position.y<3){
                //transform.rotation=new Quaternion(0,0,180,0);
                if(transform.rotation.z<179){transform.rotation=new Quaternion(0,0,transform.rotation.z+30*Time.timeScale,0);}
                if(transform.rotation.z>=179/*||(transform.rotation.z>-150&&transform.rotation.z<0)*/){transform.rotation=new Quaternion(0,0,180,0);}
            }}
        }}}
    }
    
    private void Shoot(){
        shotCounter -= Time.deltaTime;
        if(shotCounter<=0f){
        if(GetComponent<LaunchRadialBullets>()==null){
            if(DBullets!=true){
                var bt=Instantiate(bullet, transform.position,Quaternion.identity) as GameObject;
                bt.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
            }else{
                var pos1 = new Vector2(transform.position.x+bulletDist,transform.position.y);
                var bt1 = Instantiate(bullet, pos1, Quaternion.identity) as GameObject;
                bt1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
                var pos2 = new Vector2(transform.position.x - bulletDist, transform.position.y);
                var bt2 = Instantiate(bullet, pos2, Quaternion.identity) as GameObject;
                bt2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -bulletSpeed);
            }
        }else{
            GetComponent<LaunchRadialBullets>().Shoot();
        }
        shotCounter = Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
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
                if(enBallChance==1){ Instantiate(enBallPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                if(coinChance==1){ Instantiate(coinPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                if(powercoreChance==1){ Instantiate(powercorePrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity); }
                gameSession.AddEnemyCount();
                if(xpAmnt!=0)gameSession.AddXP(xpAmnt);
                givePts=false;
            }
            AudioManager.instance.Play("Explosion");
            if(GetComponent<GoblinDrop>()!=null)GetComponent<GoblinDrop>().DropPowerup(true);
            if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
            if(specialDrop!=null){Instantiate(specialDrop,transform.position,Quaternion.identity);}
            //if(GetComponent<HealingDrone>()!=null)GameAssets.instance.Make("ArmorPwrup",transform.position);
            //if(GetComponent<ParticleDelay>()!=null){GetComponent<ParticleDelay>().on=true;health=-1000;Destroy(gameObject,0.05f);}
            /*if(GetComponent<ParticleDelay>()==null){*/GameObject explosion = Instantiate(explosionVFX, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);Destroy(explosion, 0.5f);Destroy(gameObject,0.01f);//}
            shake.CamShake(2,1);
        }
    }
    private void OnDestroy() {
        if(GetComponent<GoblinDrop>()!=null)GetComponent<GoblinDrop>().DropPowerup(false);
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        if(randomizeWaveDeath==true){ gameSession.EVscore = gameSession.EVscoreMax; }
    }
    private void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){if(yeeted==true){givePts=true; health=-1; Die();} else{ Destroy(gameObject,0.001f); if(GetComponent<GoblinDrop>()!=null){foreach(GameObject obj in GetComponent<GoblinDrop>().powerup)Destroy(obj);/*obj.SetActive(true);*/}}}
    }
    private void OnTriggerEnter2D(Collider2D other){
        //if(FindObjectOfType<Player>().shadowRaycast[FindObjectOfType<Player>().shadowRaycast.FindIndex(FindObjectOfType<Player>().shadowRaycast.Count,(x) => x == this)]==this){Die();}
        if(!other.CompareTag(tag)&&!other.CompareTag("EnemyBullet")&&other.GetComponent<Tag_OutsideZone>()==null){
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            if(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}
            var dmg = damageValues.GetDmg();

            if(other.GetComponent<Player>()!=null){if(other.GetComponent<Player>().dashing==true){Die();}}

            var Lname = laserPrefab.name;
            if (other.gameObject.name.Contains(Lname)) { dmg = damageValues.GetDmgLaser(); Destroy(other.gameObject); AudioManager.instance.Play("EnemyHit"); }
            
            var MLname = mlaserPrefab.name;
            if (other.gameObject.name.Contains(MLname)){ 
                AudioManager.instance.Play("MLaserHit");
                /*var mlaserHitSound = other.GetComponent<RandomSound>().sound;
                if(other.GetComponent<RandomSound>().playLimitForThis==true){mlaserHitSound=other.GetComponent<RandomSound>().sound2;}
                AudioSource.PlayClipAtPoint(mlaserHitSound, new Vector2(transform.position.x, transform.position.y));*/
                dmg = damageValues.GetDmgMiniLaser(); Destroy(other.gameObject);}
            
            var HRname = hrocketPrefab.name;
            if (other.gameObject.name.Contains(HRname)) { dmg = damageValues.GetDmgHRocket(); Destroy(other.gameObject); AudioManager.instance.Play("HRocketHit");
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            var LSabername = lsaberPrefab.name;
            if (other.gameObject.name.Contains(LSabername)){ dmg = (float)System.Math.Round(damageValues.GetDmgLSaber()*9f,2); AudioManager.instance.Play("LSaberHit"); }

            //var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            //if (other.gameObject.name != Pname && other.gameObject.name != Pname1){dmg=0;}

            var LClawsname = lclawsPrefab.name;
            if (other.gameObject.name.Contains(LClawsname)){ dmg = (float)System.Math.Round(damageValues.GetDmgLSaber()/3,2); AudioManager.instance.Play("LClawsHit"); FindObjectOfType<Player>().energy-=1f;}
            var LClawsPartname = lclawsPartPrefab.name;
            if (other.gameObject.name.Contains(LClawsPartname)){ dmg = damageValues.GetDmgLClaws(); AudioManager.instance.Play("LClawsHit"); }

            var shadowbtName = shadowbtPrefab.name;
            if (other.gameObject.name.Contains(shadowbtName)) { dmg = damageValues.GetDmgShadowBT(); FindObjectOfType<Player>().Damage(1,dmgType.shadow);}
            
            var cBulletname = cbulletPrefab.name;
            if (other.gameObject.name.Contains(cBulletname)) { dmg = damageValues.GetDmgCBullet(); AudioManager.instance.Play("CStreamHit");}

            var QRname = qrocketPrefab.name;
            if (other.gameObject.name.Contains(QRname)) { dmg = damageValues.GetDmgQRocket(); Destroy(other.gameObject); AudioManager.instance.Play("QRocketHit");
                var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }
            var PRname = procketPrefab.name;
            if (other.gameObject.name.Contains(PRname)) { dmg = damageValues.GetDmgPRocket(); //AudioSource.PlayClipAtPoint(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                //var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }var PLname = plaserPrefab.name;
            if (other.gameObject.name.Contains(PLname)) { dmg = other.GetComponent<DamageOverDist>().dmg; Destroy(other.gameObject); AudioManager.instance.Play("PLaserHit");//AudioSource.PlayClipAtPoint(hrocketHitSFX, new Vector2(transform.position.x, transform.position.y));
                //var explosionSmall = Instantiate(explosionSmallVFX, new Vector2(transform.position.x, transform.position.y - 0.5f), Quaternion.identity); Destroy(explosionSmall.gameObject, 0.3f);
            }

            var PRExplname = procketExplPrefab.name;
            if (other.gameObject.name.Contains(PRExplname)) { dmg = damageValues.GetDmgPRocketExpl(); GetComponent<Rigidbody2D>().velocity = Vector2.up*6f; yeeted=true; }// AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            
            var mPulsename = mPulsePrefab.name;
            if (other.gameObject.name.Contains(mPulsename)) { dmg = damageValues.GetDmgMPulse(); AudioManager.instance.Play("MLaserHit");}

            if(GetComponent<VortexWheel>()!=null){
                if(!other.gameObject.name.Contains(HRname)&&!other.gameObject.name.Contains(QRname)){
                    dmg/=3;
                }
            }
            dmg*=player.dmgMulti;
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
        if (!other.CompareTag(tag)&&other.GetComponent<Tag_OutsideZone>()==null)
        {
            DamageDealer damageDealer=other.GetComponent<DamageDealer>();
            DamageValues damageValues=DamageValues.instance;
            if(!damageDealer||!damageValues){Debug.LogWarning("No DamageDealer component or DamageValues instance");return;}
            float dmg = damageValues.GetDmg();

            var Pname = phaserPrefab.name; var Pname1 = phaserPrefab.name + "(Clone)";
            if (other.gameObject.name == Pname || other.gameObject.name == Pname1) {dmg = damageValues.GetDmgPhaser(); AudioManager.instance.Play("PhaserHit"); }
            //else { dmg = 0; }

            var LSabername = lsaberPrefab.name; var LSabername1 = lsaberPrefab.name + "(Clone)";
            if (other.gameObject.name == LSabername || other.gameObject.name == LSabername1) { dmg = damageValues.GetDmgLSaber(); AudioManager.instance.Play("EnemyHit"); }
            //else { dmg = 0; }
            

            var PRExplname = procketExplPrefab.name; var PRExplname1 = procketExplPrefab.name + "(Clone)";
            if (other.gameObject.name == PRExplname || other.gameObject.name == PRExplname1) { dmg = damageValues.GetDmgPRocketExpl(); }//AudioSource.PlayClipAtPoint(procketHitSFX, new Vector2(transform.position.x, transform.position.y));}
            //else {dmg=0;}

            var cBulletname = cbulletPrefab.name; var cBulletname1 = cbulletPrefab.name + "(Clone)";
            if (other.gameObject.name == cBulletname || other.gameObject.name == cBulletname1) { dmg = damageValues.GetDmgCBullet(); AudioManager.instance.Play("CStreamHit");}
            //else {dmg=0;}

            var LClawsname = lclawsPrefab.name; var LClawsname1 = lclawsPrefab.name + "(Clone)";
            if (other.gameObject.name == LClawsname || other.gameObject.name == LClawsname1){ dmg = (float)System.Math.Round(damageValues.GetDmgLSaber()/3,2); FindObjectOfType<Player>().energy-=0.1f;}//AudioSource.PlayClipAtPoint(lclawsHitSFX, new Vector2(transform.position.x, transform.position.y)); }

            var mPulsename = mPulsePrefab.name; var mPulsename1 = mPulsePrefab.name + "(Clone)";
            if (other.gameObject.name == mPulsename || other.gameObject.name == mPulsename1) { dmg = 0; AudioManager.instance.Play("PRocketHit");}

            dmg*=player.dmgMulti;
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
