using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour{
    [Header("Enemy")]
    [SerializeField] public string Name;
    [SerializeField] public enemyType type;
    [SerializeField] public Vector2 size=Vector2.one;
    [DisableInEditorMode] public float sizeAvg=1;
    [SerializeField] public Sprite spr;
    [SerializeField] public ShaderMatProps sprMatProps;
    public float health=100f;
    public float healthMax=100f;
    public int defense=0;
    public bool defenseOnPhase=true;
    public bool healthBySize=false;
    [SerializeField] public bool shooting=false;
    [SerializeField] public string bulletAssetName;
    [SerializeField] public Vector2 shootTime=new Vector2(1.75f,2.8f);
    [ReadOnly] public float shootTimer;
    [SerializeField] float bulletSpeed=8f;
    [SerializeField] bool DBullets=false;
    [SerializeField] float bulletDist=0.35f;
    [SerializeField] bool randomizeWaveDeath=false;
    [SerializeField] bool flyOff=false;
    [SerializeField] public bool killOnDash=true;
    [Header("Drops & Points")]
    [SerializeField] public bool giveScore=true;
    [SerializeField] Vector2 scoreValue=new Vector2(1,10);
    [SerializeField] float xpAmnt=0f;
    [SerializeField] float xpChance=100f;
    [SerializeField] public List<LootTableEntryDrops> drops;
    public List<float> dropValues;

    [Header("Others")]
    [SerializeField] public bool destroyOut=true;
    public bool yeeted=false;
    public bool dmgCounted;
    public float dmgCount;
    GameObject dmgCountPopup;

    public GameObject bullet;
    Rigidbody2D rb;
    SpriteRenderer sprRender;

    void Awake(){
        if(GetComponent<SpriteRenderer>()!=null){sprRender=GetComponent<SpriteRenderer>();}
        else{if(transform.GetChild(0)!=null){sprRender=transform.GetChild(0).GetComponent<SpriteRenderer>();}}
        if(GameSession.maskMode!=0){
            if(sprRender!=null)sprRender.maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
            else{if(transform.GetChild(0)!=null)transform.GetChild(0).GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;}
        }
        StartCoroutine(SetValues());
    }
    IEnumerator SetValues(){
        //drops=(LootTableDrops)gameObject.AddComponent(typeof(LootTableDrops));
        yield return new WaitForSeconds(0.01f);
        var i=GameRules.instance;
        if(i!=null){
        EnemyClass e=null;
        e=System.Array.Find(i.enemies,x=>x.name==Name);
        if(e!=null){
            type=e.type;
            size=e.size;
            spr=e.spr;sprMatProps=e.sprMatProps;
            health=e.healthStart;healthMax=e.healthMax;
            healthBySize=e.healthBySize;
            defense=e.defense;
            shooting=e.shooting;
            shootTime=e.shootTime;
            bulletAssetName=e.bulletAssetName;
            bulletSpeed=e.bulletSpeed;
            DBullets=e.DBullets;
            bulletDist=e.bulletDist;
            randomizeWaveDeath=e.randomizeWaveDeath;
            flyOff=e.flyOff;
            killOnDash=e.killOnDash;
            
            giveScore=e.giveScore;
            scoreValue=e.scoreValue;
            xpAmnt=e.xpAmnt;
            xpChance=e.xpChance;
            drops=e.drops;
        }
        if(GetComponent<Goblin>()!=null||GetComponent<VortexWheel>()!=null/*||GetComponent<HealingDrone>()!=null*/)shooting=false;
        if(GetComponent<HealingDrone>()!=null){if(!GetComponent<HealingDrone>().enabled)GetComponent<HealingDrone>().enabled=true;}

            yield return new WaitForSeconds(0.04f);
            for(var d=0;d<drops.Count;d++){dropValues.Add(drops[d].dropChance);}
            dropValues[0]*=GameSession.instance.enballDropMulti;
            dropValues[1]*=GameSession.instance.coinDropMulti;
            dropValues[2]*=GameSession.instance.coreDropMulti;

            for(var d=0;d<dropValues.Count;d++){
                if(Random.Range(1,101)<=dropValues[d]&&dropValues[d]!=0){dropValues[d]=101;}
            }
            if(!GameRules.instance.energyOnPlayer)dropValues[0]=0;
            if(!GameRules.instance.crystalsOn)dropValues[1]=0;
            if(!GameRules.instance.coresOn)dropValues[2]=0;
        }

        bullet=GameAssets.instance.GetEnemyBullet(bulletAssetName);
    }
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}

        if(shooting)shootTimer=Random.Range(shootTime.x,shootTime.y);
        if(healthBySize){healthMax=Mathf.RoundToInt(healthMax*sizeAvg);health=Mathf.RoundToInt(health*sizeAvg);}
    }
    void Update(){
        if(shooting){Shoot();}
        if(flyOff){FlyOff();}
        Die();
        if(destroyOut)DestroyOutside();
        DispDmgCountUp();

        health=Mathf.Clamp(health,-1000,healthMax);

        if((Vector2)transform.localScale!=size)transform.localScale=size;
        if(sizeAvg!=(size.x+size.y)/2)sizeAvg=(size.x+size.y)/2;
        if(sprRender.sprite!=spr&&GetComponent<VortexWheel>()==null)sprRender.sprite=spr;
        if(sprMatProps!=null){sprRender.material=GameAssets.instance.UpdateShaderMatProps(sprRender.material,sprMatProps);}
    }
    
    void Shoot(){   if(!GameSession.GlobalTimeIsPaused){
        shootTimer-=Time.deltaTime;
        if(shootTimer<=0f){
        if(GetComponent<LaunchRadialBullets>()==null&&GetComponent<LaunchSwarmBullets>()==null&&GetComponent<HealingDrone>()==null){
            if(bullet!=null){
                if(DBullets!=true){
                    var bt=Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
                    bt.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    if(bt.GetComponent<Tag_PauseVelocity>()==null)bt.AddComponent<Tag_PauseVelocity>();
                }else{
                    var pos1=new Vector2(transform.position.x+bulletDist,transform.position.y);
                    var bt1=Instantiate(bullet,pos1,Quaternion.identity) as GameObject;
                    bt1.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    if(bt1.GetComponent<Tag_PauseVelocity>()==null)bt1.AddComponent<Tag_PauseVelocity>();
                    var pos2=new Vector2(transform.position.x - bulletDist, transform.position.y);
                    var bt2=Instantiate(bullet,pos2,Quaternion.identity) as GameObject;
                    bt2.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    if(bt2.GetComponent<Tag_PauseVelocity>()==null)bt2.AddComponent<Tag_PauseVelocity>();
                }
            }else{Debug.LogWarning("Bullet not asigned");}
        }else if(GetComponent<LaunchRadialBullets>()!=null){GetComponent<LaunchRadialBullets>().Shoot();}
        else if(GetComponent<LaunchSwarmBullets>()!=null){GetComponent<LaunchSwarmBullets>().Shoot();}
        shootTimer=Random.Range(shootTime.x, shootTime.y);
        }
    }}
    void FlyOff(){
        if(Player.instance==null){
            shooting=false;
            rb.velocity=new Vector2(0,3f);
        }
    }
    public void Die(bool explode=true){if(health<=0&&health!=-1000){
        GameSession.instance.AddEnemyCount();
        StatsAchievsManager.instance.AddKills(Name,type);
        int score=Random.Range((int)scoreValue.x,(int)scoreValue.y+1);
        if(GetComponent<CometRandomProperties>()!=null){
            var comet=GetComponent<CometRandomProperties>();
            if(comet.scoreBySize){
                for(var i=0;i<comet.scoreSizes.Length&&(comet.size>comet.scoreSizes[i].size&&(i+1<comet.scoreSizes.Length&&comet.size<comet.scoreSizes[i+1].size));i++){score=comet.scoreSizes[i].score;}
            }
            if(comet.isLunar){
                var lunarScore=comet.LunarScore();
                if(lunarScore!=-1)score=lunarScore;
                comet.LunarDrop();
            }
        }
        if(giveScore==true)GameSession.instance.AddToScore(score);
        if(GameRules.instance.xpOn)if(xpAmnt!=0&&xpChance>=Random.Range(0f,100f)){
            if(xpAmnt/5>=1){for(var i=0;i<(int)(xpAmnt/5);i++){GameAssets.instance.Make("CelestVial",transform.position);}}
            GameSession.instance.DropXP(xpAmnt%5,transform.position);
        }
        else{GameSession.instance.AddXP(xpAmnt);}
        giveScore=false;


        List<LootTableEntryDrops> ld=drops;
        for(var i=0;i<ld.Count;i++){//Drop items
            string st=ld[i].name;
            if(dropValues.Count>=ld.Count){
            if(dropValues[i]==101){
            var amnt=Random.Range((int)ld[i].ammount.x,(int)ld[i].ammount.y);
            if(amnt!=0){
                if(!st.Contains("Coin")){
                    if(amnt==1)GameAssets.instance.Make(st,transform.position);
                    else{GameAssets.instance.MakeSpread(st,transform.position,amnt);}
                }else{//Drop Lunar Crystals
                    if(amnt/GameRules.instance.crystalBigGain>=1){for(var c=0;c<(int)(amnt/GameRules.instance.crystalBigGain);c++){GameAssets.instance.MakeSpread("CoinB",transform.position,1);}}
                    GameAssets.instance.MakeSpread("Coin",transform.position,(amnt%GameRules.instance.crystalBigGain)/GameRules.instance.crystalGain);//CrystalB=6, CrystalS=2
                }
            }}}
        }
        if(GetComponent<Goblin>()!=null){GetComponent<Goblin>().DropPowerup(true);if(GetComponent<Goblin>().bossForm){GetComponent<Goblin>().GoblinBossDrop();}}
        
        if(explode){AudioManager.instance.Play("Explosion");GameObject explosion=GameAssets.instance.VFX("Explosion",transform.position,0.5f);Destroy(gameObject);}
        Shake.instance.CamShake(2,1);
    }}
    void OnDestroy(){
        if(GetComponent<Goblin>()!=null)GetComponent<Goblin>().DropPowerup(false);
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        if(randomizeWaveDeath==true){spawnReqsMono.AddScore(-5,-1);;}
    }
    void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){if(yeeted==true){giveScore=true;health=-1;Die();}else{Destroy(gameObject,0.001f);if(GetComponent<Goblin>()!=null){foreach(GameObject obj in GetComponent<Goblin>().powerups)Destroy(obj);}}}
    }
    public void Kill(bool giveScore=true,bool explode=true){
        this.giveScore=giveScore;
        health=-1;
        Die(explode);
    }
    public void Damage(float dmg){health-=dmg;}
    //Collisions in EnemyCollider
    public void DispDmgCount(Vector2 pos){if(SaveSerial.instance.settingsData.dmgPopups)StartCoroutine(DispDmgCountI(pos));}
    IEnumerator DispDmgCountI(Vector2 pos){
        dmgCounted=true;
        //In Update, DispDmgCountUp
        dmgCountPopup=WorldCanvas.instance.DMGPopup(0,pos,ColorInt32.Int2Color(ColorInt32.dmgColor));
        yield return new WaitForSeconds(0.2f);
        dmgCounted=false;
        dmgCount=0;
    }
    void DispDmgCountUp(){if(dmgCountPopup!=null)dmgCountPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmgCount,1).ToString();}

    public bool _healable(){if(type==enemyType.living||type==enemyType.mecha){return true;}else{return false;}}
}

public enum enemyType{living,mecha,other}