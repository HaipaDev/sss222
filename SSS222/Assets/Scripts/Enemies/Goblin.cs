using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;

public class Goblin : MonoBehaviour{
    [SerializeField] Sprite bossSprite;
    [SerializeField] float bossHealth;
    [SerializeField] public List<LootTableEntryDrops> bossDrops;
    public List<float> dropValues;
    public List<GameObject> powerups;
    public bool bossForm=false;
    public bool confused=false;
    float yMax=6.6f;
    float yMin=-4.1f;
    float vspeed=0.09f;
    bool moveDown;
    Vector2 pos;

    Rigidbody2D rb;
    GameObject questionMarkObj;

    void Start(){
        var i=GameRules.instance;if(i!=null){
            var e=i.goblinBossSettings;
            bossSprite=e.goblinBossSprite;
            bossHealth=e.goblinbossHealth;
            bossDrops=e.goblinBossDrops;
        }

        for(var d=0;d<bossDrops.Count;d++){dropValues.Add(bossDrops[d].dropChance);}
        for(var d=0;d<dropValues.Count;d++){if(Random.Range(0, 100)<=dropValues[d]){dropValues[d]=101;}}

        rb=GetComponent<Rigidbody2D>();
        questionMarkObj=transform.GetChild(0).gameObject;
        questionMarkObj.SetActive(false);
    }

    void Update(){
        if(powerups!=null&&!bossForm){rb.velocity=new Vector2(Random.Range(2.5f,3f),Random.Range(2.5f,3f));}//Fly off after getting powerup
        if(bossForm)BossAI();
    }
    
    public void DropPowerup(bool sound){
        if(powerups.Count>0){
        foreach(GameObject pwrup in powerups){
            if(pwrup!=null){
                //Instantiate(powerup,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
                pwrup.SetActive(true);
                pwrup.transform.position=transform.position;
                if(pwrup.GetComponent<Tag_Collectible>()!=null)pwrup.GetComponent<Tag_Collectible>().SetDirSpeed();
            }
        }
        }
        if(sound){
        if(bossForm!=true)AudioManager.instance.Play("GoblinDeath");
        else AudioManager.instance.Play("GoblinDeathTransf");
        }
    }
    public void GoblinBossDrop(){
        List<LootTableEntryDrops> ld=bossDrops;
        for(var i=0;i<ld.Count;i++){
            string st=ld[i].name;
            if(dropValues.Count>=ld.Count){
            if(dropValues[i]>=101){
                var amnt=Random.Range((int)ld[i].ammount.x,(int)ld[i].ammount.y);
                if(amnt==1)GameAssets.instance.Make(st,transform.position);
                else{GameAssets.instance.MakeSpread(st,transform.position,amnt);}
            }}
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(bossForm!=true){
            if(other.CompareTag("Powerups")&&(!other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)&&!other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name))){
                AudioManager.instance.Play("GoblinSteal");
                powerups.Add(other.gameObject);
                other.gameObject.SetActive(false);
                questionMarkObj.SetActive(false);
                confused=false;
            }else if(other.gameObject.name.Contains(GameAssets.instance.Get("EnBall").name)||other.gameObject.name.Contains(GameAssets.instance.Get("Coin").name)){
                if(confused==false){
                    AudioManager.instance.Play("GoblinConfused");
                    questionMarkObj.SetActive(true);
                    confused=true;
                }
            }else if(other.gameObject.name.Contains(GameAssets.instance.Get("PowerCore").name)){//Transform
                powerups.Add(other.gameObject);
                other.gameObject.SetActive(false);
                GoblinTransform();
            }
        }
    }
    [ContextMenu("BossTransform")][Button("Boss Transform")]
    void GoblinTransform(){
        bossForm=true;
        GetComponent<Follow>().enabled=false;
        if(GetComponent<BackflameEffect>().BFlame!=null){Destroy(GetComponent<BackflameEffect>().BFlame);}GetComponent<BackflameEffect>().enabled=false;
        confused=false;Destroy(transform.GetChild(0));
        GetComponent<SpriteRenderer>().sprite=bossSprite;
        GetComponent<Enemy>().health=bossHealth;GetComponent<Enemy>().healthMax=bossHealth;
        GetComponent<Enemy>().shooting=true;
        pos.x=transform.position.x;transform.rotation=new Quaternion(0,0,0,0);
        rb.velocity=Vector2.zero;GetComponent<Tag_PauseVelocity>().velPaused=Vector2.zero;
        AudioManager.instance.Play("GoblinTransform");
    }
    void BossAI(){
        if(!GameSession.GlobalTimeIsPaused){
            float step=vspeed*Time.timeScale;
            if(!moveDown&&pos.y<yMax)pos.y+=step;
            if(pos.y>=yMax)moveDown=true;
            if(moveDown)pos.y-=step;
            if(pos.y<=yMin)moveDown=false;
            transform.position=pos;
        }
    }
}
