using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour{
    [Header("Enemy")]
    [SerializeField] public string Name;
    [SerializeField] public Vector2 size=Vector2.one;
    [SerializeField] public Sprite spr;
    [SerializeField] public float healthStart=100f;
    public float health=100f;
    float shotCounter;
    [SerializeField] public bool shooting=false;
    [SerializeField] float minTimeBtwnShots=0.2f;
    [SerializeField] float maxTimeBtwnShots=1f;
    [SerializeField] public GameObject bullet;
    [SerializeField] float bulletSpeed=8f;
    [SerializeField] bool DBullets=false;
    [SerializeField] float bulletDist=0.35f;
    [SerializeField] bool randomizeWaveDeath=false;
    [SerializeField] bool flyOff=false;
    [SerializeField] float freezefxTime=0.05f;
    [Header("Drops & Points")]
    //[SerializeField] int scoreValue=1;
    [SerializeField] public bool givePts=true;
    [SerializeField] int scoreValueStart=1;
    [SerializeField] int scoreValueEnd=10;
    [SerializeField] float enBallChanceInit=30f;
    [SerializeField] float coinChanceInit=3f;
    [SerializeField] float powercoreChanceInit=0f;
    [SerializeField] float xpAmnt=0f;
    [HideInInspector] public float enBallChance;
    [HideInInspector] public float coinChance;
    [HideInInspector] public float powercoreChance;
    [SerializeField] public GameObject specialDrop;
    [Header("Others")]
    //[SerializeField] public bool cTagged=false;
    //[SerializeField] public float curSpeed;
    [SerializeField] public bool yeeted=false;
    public bool dmgCounted;
    public float dmgCount;
    GameObject dmgCountPopup;

    Rigidbody2D rb;
    Shake shake;

    private void Awake(){
        StartCoroutine(SetValues());
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
    }
    IEnumerator SetValues(){
        yield return new WaitForSeconds(0.02f);
        //Set values
        var i=GameRules.instance;
        if(i!=null){
            EnemyClass e=null;
            foreach(EnemyClass enemy in i.enemies){if(enemy.name==Name){e=enemy;}}
            if(e!=null){
            size=e.size;
            if(GetComponent<CometRandomProperties>()==null){transform.localScale=size;spr=e.spr;GetComponent<SpriteRenderer>().sprite=spr;}
            healthStart=e.health;
            shooting=e.shooting;
            minTimeBtwnShots=e.minTimeBtwnShots;
            maxTimeBtwnShots=e.maxTimeBtwnShots;
            bullet=e.bullet;
            bulletSpeed=e.bulletSpeed;
            DBullets=e.DBullets;
            bulletDist=e.bulletDist;
            randomizeWaveDeath=e.randomizeWaveDeath;
            flyOff=e.flyOff;
            freezefxTime=e.freezefxTime;
            
            givePts=e.givePts;
            scoreValueStart=e.scoreValueStart;
            scoreValueEnd=e.scoreValueEnd;
            enBallChanceInit=e.enBallChanceInit;
            coinChanceInit=e.coinChanceInit;
            powercoreChanceInit=e.powercoreChanceInit;
            xpAmnt=e.xpAmnt;
            specialDrop=e.specialDrop;
            }
            health=healthStart;
        }
    }
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        shake=GameObject.FindObjectOfType<Shake>();

        enBallChanceInit*=GameSession.instance.enballDropMulti;
        coinChanceInit*=GameSession.instance.coinDropMulti;
        powercoreChanceInit*=GameSession.instance.coreDropMulti;

        enBallChance=Random.Range(0f, 100f);
        coinChance=Random.Range(0f, 100f);
        powercoreChance=Random.Range(0f, 100f);
        if(enBallChance<=enBallChanceInit&&enBallChanceInit>0){enBallChance=1;}
        if(coinChance<=coinChanceInit&&coinChanceInit>0){coinChance=1;}
        if(powercoreChance<=powercoreChanceInit&&powercoreChanceInit>0){powercoreChance=1;}
        if(!GameRules.instance.energyOnPlayer)enBallChance=0;
        if(!GameSession.instance.shopOn)coinChance=0;
        if(!GameSession.instance.upgradesOn)powercoreChance=0;
        shotCounter=Random.Range(minTimeBtwnShots,maxTimeBtwnShots);
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
        if(GetComponent<LaunchRadialBullets>()==null&&GetComponent<HealingDrone>()==null){
            if(bullet!=null){
                if(DBullets!=true){
                    var bt=Instantiate(bullet, transform.position,Quaternion.identity) as GameObject;
                    bt.GetComponent<Rigidbody2D>().velocity=new Vector2(0, -bulletSpeed);
                }else{
                    var pos1=new Vector2(transform.position.x+bulletDist,transform.position.y);
                    var bt1=Instantiate(bullet, pos1, Quaternion.identity) as GameObject;
                    bt1.GetComponent<Rigidbody2D>().velocity=new Vector2(0, -bulletSpeed);
                    var pos2=new Vector2(transform.position.x - bulletDist, transform.position.y);
                    var bt2=Instantiate(bullet, pos2, Quaternion.identity) as GameObject;
                    bt2.GetComponent<Rigidbody2D>().velocity=new Vector2(0, -bulletSpeed);
                }
            }else{Debug.LogWarning("Bullet not asigned");}
        }else if(GetComponent<LaunchRadialBullets>()!=null){GetComponent<LaunchRadialBullets>().Shoot();}
        shotCounter=Random.Range(minTimeBtwnShots, maxTimeBtwnShots);
        }
    }
    private void FlyOff(){
        if(FindObjectOfType<Player>()==null){
            shooting=false;
            rb.velocity=new Vector2(0,3f);
        }
    }
    public void Die(){
        if(health<=0&&health!=-1000){
            int scoreValue=Random.Range(scoreValueStart,scoreValueEnd);
            if(GetComponent<CometRandomProperties>()!=null){
                var comet=GetComponent<CometRandomProperties>();
                if(comet.scoreBySize){
                    for(var i=0;i<comet.scoreSizes.Length&&(comet.Size()>comet.scoreSizes[i].size&&(i+1<comet.scoreSizes.Length&&comet.Size()<comet.scoreSizes[i+1].size));i++){/*&&comet.scoreSizes[i].size-System.Math.Truncate(comet.scoreSizes[i].size)>0.5*/ //){i++;}
                    scoreValue=comet.scoreSizes[i].score;}
                }
                if(comet.isLunar){
                    var lunarScore=comet.LunarScore();
                    if(lunarScore!=-1)scoreValue=lunarScore;
                    if(comet.LunarDrop()){Instantiate(comet.GetLunarDrop(),new Vector2(transform.position.x,transform.position.y),Quaternion.identity);}
                }
            }
            if(givePts==true){
                GameSession.instance.AddToScore(scoreValue);
                if(enBallChance==1){GameAssets.instance.Make("EnBall",transform.position);}
                if(coinChance==1){GameAssets.instance.Make("Coin",transform.position);}
                if(powercoreChance==1){GameAssets.instance.Make("PowerCore",transform.position);}
                GameSession.instance.AddEnemyCount();
                if(xpAmnt!=0)if(GameRules.instance.xpOn){GameSession.instance.DropXP(xpAmnt,transform.position);}else{GameSession.instance.AddXP(xpAmnt);}
                givePts=false;
            }
            AudioManager.instance.Play("Explosion");
            if(GetComponent<GoblinDrop>()!=null)GetComponent<GoblinDrop>().DropPowerup(true);
            if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
            if(specialDrop!=null){Instantiate(specialDrop,transform.position,Quaternion.identity);}
            //if(GetComponent<HealingDrone>()!=null)GameAssets.instance.Make("ArmorPwrup",transform.position);
            //if(GetComponent<ParticleDelay>()!=null){GetComponent<ParticleDelay>().on=true;health=-1000;Destroy(gameObject,0.05f);}
            /*if(GetComponent<ParticleDelay>()==null){*/GameObject explosion=GameAssets.instance.VFX("Explosion",transform.position,0.5f);Destroy(gameObject,0.01f);//}
            shake.CamShake(2,1);
            if(TimeFreezer.instance!=null&&freezefxTime>0)TimeFreezer.instance.Freeze(freezefxTime);
        }
    }
    private void OnDestroy(){
        if(GetComponent<GoblinDrop>()!=null)GetComponent<GoblinDrop>().DropPowerup(false);
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        if(randomizeWaveDeath==true){GameSession.instance.EVscore=GameSession.instance.EVscoreMax;}
    }
    private void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){if(yeeted==true){givePts=true; health=-1; Die();} else{Destroy(gameObject,0.001f); if(GetComponent<GoblinDrop>()!=null){foreach(GameObject obj in GetComponent<GoblinDrop>().powerup)Destroy(obj);/*obj.SetActive(true);*/}}}
    }
    //Collisions in EnemyCollider
    public void DispDmgCount(Vector2 pos){StartCoroutine(DispDmgCountI(pos));}
    IEnumerator DispDmgCountI(Vector2 pos){
        dmgCounted=true;
        //In Update, DispDmgCountUp
        dmgCountPopup=GameCanvas.instance.DMGPopupReturn(0,pos,Color.yellow);
        yield return new WaitForSeconds(0.2f);
        dmgCounted=false;
        dmgCount=0;
    }
    void DispDmgCountUp(){if(dmgCountPopup!=null)dmgCountPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmgCount,1).ToString();}
}