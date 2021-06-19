using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometRandomProperties : MonoBehaviour{
    [Header("Basic")]
    [SerializeField] float sizeMin=0.4f;
    [SerializeField] float sizeMax=1.4f;
    [SerializeField] float size;
    [SerializeField] bool healthBySize=true;
    [SerializeField] public bool damageBySpeedSize=true;
    [SerializeField] public bool scoreBySize=false;
    [SerializeField] public bool randomAngle=true;
    [SerializeField] public CometScoreSize[] scoreSizes;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject bflamePart;
    [Header("Lunar")]
    [SerializeField] float sizeMinLunar=0.88f;
    [SerializeField] float sizeMaxLunar=1.55f;
    [SerializeField] int lunarCometChance=10;
    [SerializeField] float lunarHealthMulti=2.5f;
    [SerializeField] float lunarSpeedMulti=0.415f;
    [SerializeField] Vector2 lunarScore;
    [SerializeField] public List<LootTableEntryDrops> lunarDrops;
    public List<float> dropValues;
    [SerializeField] Sprite[] spritesLunar;
    [SerializeField] GameObject lunarPart;
    public int healhitCount;
    public bool isLunar;

    SpriteRenderer spriteRenderer;
    Enemy enemy;
    BackflameEffect bFlame;
    Rigidbody2D rb;
    //float rotationSpeed;
    void Awake(){StartCoroutine(SetValues());}
    IEnumerator SetValues(){
        //lunarDrops=(LootTableDrops)gameObject.AddComponent(typeof(LootTableDrops));
        yield return new WaitForSeconds(0.02f);
        var i=GameRules.instance;
        if(i!=null){
            var e=i.cometSettings;
            sizeMin=e.sizeMin;
            sizeMax=e.sizeMax;
            healthBySize=e.healthBySize;
            damageBySpeedSize=e.damageBySpeedSize;
            scoreBySize=e.scoreBySize;
            scoreSizes=e.scoreSizes;
            sprites=e.sprites;
            bflamePart=e.bflamePart;

            sizeMinLunar=e.sizeMinLunar;
            sizeMaxLunar=e.sizeMaxLunar;
            lunarCometChance=e.lunarCometChance;
            lunarHealthMulti=e.lunarHealthMulti;
            lunarSpeedMulti=e.lunarSpeedMulti;
            lunarScore=e.lunarScore;
            lunarDrops=e.drops;

            spritesLunar=e.spritesLunar;
            lunarPart=e.lunarPart;
        }
        for(var d=0;d<lunarDrops.Count;d++){dropValues.Add(lunarDrops[d].dropChance);}
        for(var d=0;d<dropValues.Count;d++){if(Random.Range(0, 100)<=dropValues[d]){dropValues[d]=101;}}
    }
    IEnumerator Start(){
        bFlame=GetComponent<BackflameEffect>();
        bFlame.enabled=false;
        yield return new WaitForSeconds(0.1f);
        bFlame.enabled=true;
        spriteRenderer=GetComponent<SpriteRenderer>();
        rb=GetComponent<Rigidbody2D>();
        enemy=GetComponent<Enemy>();
        var spriteIndex=Random.Range(0, sprites.Length);
        spriteRenderer.sprite=sprites[spriteIndex];
        size=Random.Range(sizeMin, sizeMax);
        transform.localScale=new Vector2(enemy.size.x*size,enemy.size.y*size);

        var angle=Random.Range(0f,360f);
        if(randomAngle)transform.rotation=Quaternion.AngleAxis(angle,Vector3.forward);

        if(healthBySize)enemy.healthStart=Mathf.RoundToInt(enemy.health*size);enemy.health=enemy.healthStart;

        //Lunar Comets
        //if(GameSession.instance.shopOn){
        //if(GameSession.instance.gameModeSelected!=2){
            if(Random.Range(0,100)<lunarCometChance)isLunar=true;
            if(isLunar==true){
                TransformIntoLunar();
            }
        //}
        //}
        //rotationSpeed=Random.Range(2,8);
        //Destroy(this,0.3f);
    }
    public float Size(){return size;}
    public int LunarScore(){
        return Random.Range((int)lunarScore.x,(int)lunarScore.y);
    }
    //public bool LunarDrop(){if(Random.Range(0,100)<lunarDropChance&&lunarDrop!=null)return true;else return false;}
    //public GameObject GetLunarDrop(){return lunarDrop;}
    void Update(){
        if(healhitCount>=3&&!isLunar){MakeLunar();}
        //transform.Rotate(new Vector3(0,0,rotationSpeed));
    }
    [ContextMenu("MakeLunar")]public void MakeLunar(){
        isLunar=true;
        bFlame.ClearBFlame();
        TransformIntoLunar();
    }
    void TransformIntoLunar(){StartCoroutine(TransformIntoLunarI());}
    IEnumerator TransformIntoLunarI(){
        var spriteIndex=Random.Range(0,spritesLunar.Length);spriteRenderer.sprite=spritesLunar[spriteIndex];
        bFlame.part=lunarPart;
        var sizeA=Random.Range(sizeMinLunar, sizeMaxLunar);
        transform.localScale=new Vector2(enemy.size.x*sizeA, enemy.size.y*sizeA);

        enemy.health*=lunarHealthMulti;
        rb.velocity*=lunarSpeedMulti;
        yield return new WaitForSeconds(0.1f);
        if(GameSession.instance.shopOn)enemy.dropValues[1]=101;
    }

    public void LunarDrop(){
        List<LootTableEntryDrops> ld=lunarDrops;
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
}
[System.Serializable]public class CometScoreSize{
    public float size;
    public int score;
}