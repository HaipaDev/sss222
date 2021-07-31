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
    [SerializeField] Vector2 shootTime=new Vector2(1.75f,2.8f);
    [SerializeField] public GameObject bullet;
    [SerializeField] float bulletSpeed=8f;
    [SerializeField] bool DBullets=false;
    [SerializeField] float bulletDist=0.35f;
    [SerializeField] bool randomizeWaveDeath=false;
    [SerializeField] bool flyOff=false;
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

    Rigidbody2D rb;

    private void Awake(){
        StartCoroutine(SetValues());
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;
    }
    IEnumerator SetValues(){
        //drops=(LootTableDrops)gameObject.AddComponent(typeof(LootTableDrops));
        yield return new WaitForSeconds(0.02f);
        var i=GameRules.instance;
        if(i!=null){
        EnemyClass e=null;
        foreach(EnemyClass enemy in i.enemies){if(enemy.name==Name){e=enemy;}}
        if(e!=null){
            size=e.size;
            if(GetComponent<CometRandomProperties>()==null){transform.localScale=size;spr=e.spr;GetComponent<SpriteRenderer>().sprite=spr;}
            healthStart=e.health;
            shooting=e.shooting;
            shootTime=e.shootTime;
            bullet=e.bullet;
            bulletSpeed=e.bulletSpeed;
            DBullets=e.DBullets;
            bulletDist=e.bulletDist;
            randomizeWaveDeath=e.randomizeWaveDeath;
            flyOff=e.flyOff;
            
            giveScore=e.giveScore;
            scoreValue=e.scoreValue;
            xpAmnt=e.xpAmnt;
            xpChance=e.xpChance;
            drops=e.drops;
        }
            health=healthStart;

            //dropValues=drops.dropList;
            yield return new WaitForSeconds(0.08f);
            for(var d=0;d<drops.Count;d++){dropValues.Add(drops[d].dropChance);}
            dropValues[0]*=GameSession.instance.enballDropMulti;
            dropValues[1]*=GameSession.instance.coinDropMulti;
            dropValues[2]*=GameSession.instance.coreDropMulti;

            for(var d=0;d<dropValues.Count;d++){
                if(Random.Range(1,101)<=dropValues[d]&&dropValues[d]!=0){dropValues[d]=101;}
            }
            if(!GameRules.instance.energyOnPlayer)dropValues[0]=0;
            if(!GameSession.instance.shopOn)dropValues[1]=0;
            if(!GameSession.instance.upgradesOn)dropValues[2]=0;
        }
    }
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        shotCounter=Random.Range(shootTime.x,shootTime.y);
    }
    void Update(){
        if(shooting){Shoot();}
        if(flyOff){FlyOff();}
        Die();
        if(destroyOut)DestroyOutside();
        DispDmgCountUp();

        //Rotate if Stinger?
        if(gameObject.name.Contains("Stinger")){
        if(Time.timeScale>0.0001f){
        if(Player.instance!=null){
            if(transform.position.x-Player.instance.transform.position.x<0.2f){
            if(transform.position.y-Player.instance.transform.position.y<3){
                //transform.rotation=new Quaternion(0,0,180,0);
                if(transform.rotation.z<179){transform.rotation=new Quaternion(0,0,transform.rotation.z+30*Time.timeScale,0);}
                if(transform.rotation.z>=179/*||(transform.rotation.z>-150&&transform.rotation.z<0)*/){transform.rotation=new Quaternion(0,0,180,0);}
            }}
        }}}
    }
    
    private void Shoot(){
        shotCounter-=Time.deltaTime;
        if(shotCounter<=0f){
        if(GetComponent<LaunchRadialBullets>()==null&&GetComponent<HealingDrone>()==null){
            if(bullet!=null){
                if(DBullets!=true){
                    var bt=Instantiate(bullet,transform.position,Quaternion.identity) as GameObject;
                    bt.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                }else{
                    var pos1=new Vector2(transform.position.x+bulletDist,transform.position.y);
                    var bt1=Instantiate(bullet,pos1,Quaternion.identity) as GameObject;
                    bt1.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                    var pos2=new Vector2(transform.position.x - bulletDist, transform.position.y);
                    var bt2=Instantiate(bullet,pos2,Quaternion.identity) as GameObject;
                    bt2.GetComponent<Rigidbody2D>().velocity=new Vector2(0,-bulletSpeed);
                }
            }else{Debug.LogWarning("Bullet not asigned");}
        }else if(GetComponent<LaunchRadialBullets>()!=null){GetComponent<LaunchRadialBullets>().Shoot();}
        shotCounter=Random.Range(shootTime.x, shootTime.y);
        }
    }
    private void FlyOff(){
        if(Player.instance==null){
            shooting=false;
            rb.velocity=new Vector2(0,3f);
        }
    }
    public void Die(){if(health<=0&&health!=-1000){
        GameSession.instance.AddEnemyCount();
        int score=Random.Range((int)scoreValue.x,(int)scoreValue.y);
        if(GetComponent<CometRandomProperties>()!=null){
            var comet=GetComponent<CometRandomProperties>();
            if(comet.scoreBySize){
                for(var i=0;i<comet.scoreSizes.Length&&(comet.Size()>comet.scoreSizes[i].size&&(i+1<comet.scoreSizes.Length&&comet.Size()<comet.scoreSizes[i+1].size));i++){score=comet.scoreSizes[i].score;}
            }
            if(comet.isLunar){
                var lunarScore=comet.LunarScore();
                if(lunarScore!=-1)score=lunarScore;
                comet.LunarDrop();
            }
        }
        if(giveScore==true)GameSession.instance.AddToScore(score);
        if(GameRules.instance.xpOn)if(xpAmnt!=0&&xpChance<Random.Range(0,100)){
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
                    if(amnt/GameRules.instance.crystalBGet>=1){for(var c=0;c<(int)(amnt/GameRules.instance.crystalBGet);c++){GameAssets.instance.MakeSpread("CoinB",transform.position,1);}}
                    GameAssets.instance.MakeSpread("Coin",transform.position,(amnt%GameRules.instance.crystalBGet)/GameRules.instance.crystalGet);//CrystalB=6, CrystalS=2
                }
            }}}
        }
        AudioManager.instance.Play("Explosion");
        if(GetComponent<Goblin>()!=null){GetComponent<Goblin>().DropPowerup(true);if(GetComponent<Goblin>().bossForm){GetComponent<Goblin>().GoblinBossDrop();}}
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        GameObject explosion=GameAssets.instance.VFX("Explosion",transform.position,0.5f);Destroy(gameObject,0.01f);//}
        Shake.instance.CamShake(2,1);
    }}
    private void OnDestroy(){
        if(GetComponent<Goblin>()!=null)GetComponent<Goblin>().DropPowerup(false);
        if(GetComponent<EnCombatant>()!=null)Destroy(GetComponent<EnCombatant>().saber.gameObject);
        if(randomizeWaveDeath==true){GameSession.instance.EVscore=GameSession.instance.EVscoreMax;}
    }
    private void DestroyOutside(){
        if((transform.position.x>6.5f || transform.position.x<-6.5f) || (transform.position.y>10f || transform.position.y<-10f)){if(yeeted==true){giveScore=true;health=-1;Die();}else{Destroy(gameObject,0.001f);if(GetComponent<Goblin>()!=null){foreach(GameObject obj in GetComponent<Goblin>().powerups)Destroy(obj);}}}
    }
    //Collisions in EnemyCollider
    public void DispDmgCount(Vector2 pos){if(SaveSerial.instance.settingsData.dmgPopups)StartCoroutine(DispDmgCountI(pos));}
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