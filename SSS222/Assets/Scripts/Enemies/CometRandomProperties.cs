using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CometRandomProperties : MonoBehaviour{
    [Header("Basic")]
    [SerializeField] Vector2 sizes=new Vector2(0.4f,1.4f);
    [SerializeField] float size;
    [SerializeField] bool healthBySize=true;
    [SerializeField] public bool damageBySpeedSize=true;
    [SerializeField] public bool scoreBySize=false;
    [SerializeField] public CometScoreSize[] scoreSizes;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject bflamePart;
    [Header("Lunar")]
    [SerializeField] Vector2 sizeMultLunar=new Vector2(0.88f,1.55f);
    [SerializeField] int lunarCometChance=10;
    [SerializeField] float lunarHealthMulti=2.5f;
    [SerializeField] float lunarSpeedMulti=0.415f;
    [SerializeField] Vector2 lunarScore;
    [SerializeField] public Vector2 lunarCrystalAmmounts;
    [SerializeField] public List<LootTableEntryDrops> lunarDrops;
    public List<float> dropValues;
    [SerializeField] Sprite[] spritesLunar;
    [SerializeField] GameObject lunarPart;
    public int healhitCount;
    public bool isLunar;

    Enemy en;
    SpriteRenderer sprRender;
    BackflameEffect bFlame;
    Rigidbody2D rb;
    float rotationSpeed=1;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        //lunarDrops=(LootTableDrops)gameObject.AddComponent(typeof(LootTableDrops));
        yield return new WaitForSeconds(0.02f);
        var i=GameRules.instance;
        if(i!=null){
            var e=i.cometSettings;
            sizes=e.sizes;
            healthBySize=e.healthBySize;
            damageBySpeedSize=e.damageBySpeedSize;
            scoreBySize=e.scoreBySize;
            scoreSizes=e.scoreSizes;
            sprites=e.sprites;
            bflamePart=e.bflamePart;

            sizeMultLunar=e.sizeMultLunar;
            lunarCometChance=e.lunarCometChance;
            lunarHealthMulti=e.lunarHealthMulti;
            lunarSpeedMulti=e.lunarSpeedMulti;
            lunarScore=e.lunarScore;
            lunarDrops=e.lunarDrops;

            spritesLunar=e.spritesLunar;
            lunarPart=e.lunarPart;
        }
        for(var d=0;d<lunarDrops.Count;d++){dropValues.Add(lunarDrops[d].dropChance);}
        for(var d=0;d<dropValues.Count;d++){if(Random.Range(1,101)<=dropValues[d]&&dropValues[d]!=0){dropValues[d]=101;}}
    }
    void Start(){
        bFlame=GetComponent<BackflameEffect>();
        if(GetComponent<SpriteRenderer>()!=null)sprRender=GetComponent<SpriteRenderer>();
        else{if(transform.GetChild(0)!=null)sprRender=transform.GetChild(0).GetComponent<SpriteRenderer>();transform.GetChild(0).rotation=Quaternion.AngleAxis(Random.Range(0f,360f),Vector3.forward);}
        rb=GetComponent<Rigidbody2D>();
        en=GetComponent<Enemy>();
        var spriteIndex=Random.Range(0, sprites.Length);
        sprRender.sprite=sprites[spriteIndex];
        size=Random.Range(sizes.x, sizes.y);
        transform.localScale=new Vector2(en.size.x*size,en.size.y*size);

        if(healthBySize)en.healthStart=Mathf.RoundToInt(en.health*size);en.health=en.healthStart;

        if(Random.Range(0,100)<lunarCometChance)MakeLunar();
        rotationSpeed=Random.Range(2.8f,4.7f)*(GetComponent<Rigidbody2D>().velocity.y*-1);
        //Destroy(this,0.3f);
    }
    public float Size(){return size;}
    public int LunarScore(){return Random.Range((int)lunarScore.x,(int)lunarScore.y);}
    void Update(){
        if(healhitCount>=3&&!isLunar){MakeLunar();}
        if(!GameSession.GlobalTimeIsPaused){
        if(transform.GetChild(0)!=null){
            float step=rotationSpeed*Time.deltaTime;
            transform.GetChild(0).Rotate(new Vector3(0,0,step));
        }
        }
    }
    [ContextMenu("MakeLunar")][Button("Make Lunar")]
    public void MakeLunar(){isLunar=true;TransformIntoLunar();}
    void TransformIntoLunar(){
        var spriteIndex=Random.Range(0,spritesLunar.Length);
        sprRender.sprite=spritesLunar[spriteIndex];
        if(bFlame!=null){bFlame.ClearBFlame();bFlame.part=lunarPart;}

        var sizeA=Random.Range(sizeMultLunar.x, sizeMultLunar.y);
        if(en!=null){transform.localScale=new Vector2(en.size.x*sizeA, en.size.y*sizeA);
        en.health*=lunarHealthMulti;}
        rb.velocity*=lunarSpeedMulti;
        StartCoroutine(SetCrystalDropsI());
    }
    IEnumerator SetCrystalDropsI(){
        yield return new WaitForSeconds(0.05f);
        if(!GameRules.instance.crystalsOn)dropValues[0]=102;
    }

    public void LunarDrop(){
        List<LootTableEntryDrops> ld=lunarDrops;
        for(var i=0;i<ld.Count;i++){
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
    }
}
[System.Serializable]public class CometScoreSize{
    public float size;
    public int score;
}