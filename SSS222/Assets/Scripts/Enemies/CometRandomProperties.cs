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
    [SerializeField] public CometScoreSize[] scoreSizes;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject bflamePart;
    [Header("Lunar")]
    [SerializeField] float sizeMinLunar=0.88f;
    [SerializeField] float sizeMaxLunar=1.55f;
    [SerializeField] int lunarCometChance = 10;
    [SerializeField] float lunarHealthMulti = 2.5f;
    [SerializeField] float lunarSpeedMulti = 0.415f;
    [SerializeField] int lunarScore = -1;
    [SerializeField] int lunarScoreS = 0;
    [SerializeField] int lunarScoreE = 0;
    [SerializeField] GameObject lunarDrop;
    [SerializeField] int lunarDropChance;
    [SerializeField] Sprite[] spritesLunar;
    [SerializeField] GameObject lunarPart;
    public bool isLunar;

    SpriteRenderer spriteRenderer;
    Enemy enemy;
    BackflameEffect bFlame;
    Rigidbody2D rb;
    //float rotationSpeed;
    private void Awake() {
    //Set values
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
        lunarScoreS=e.lunarScoreS;
        lunarScoreE=e.lunarScoreE;
        lunarDrop=e.lunarDrop;
        lunarDropChance=e.lunarDropChance;

        spritesLunar=e.spritesLunar;
        lunarPart=e.lunarPart;
    }
    }
    IEnumerator Start(){
        bFlame=GetComponent<BackflameEffect>();
        bFlame.enabled=false;
        yield return new WaitForSeconds(0.1f);
        bFlame.enabled=true;
        spriteRenderer=GetComponent<SpriteRenderer>();
        rb=GetComponent<Rigidbody2D>();
        var spriteIndex=Random.Range(0, sprites.Length);
        spriteRenderer.sprite=sprites[spriteIndex];
        size=Random.Range(sizeMin, sizeMax);
        transform.localScale=new Vector2(size,size);

        var angle=Random.Range(0f,360f);
        transform.rotation=Quaternion.AngleAxis(angle,Vector3.forward);

        enemy=GetComponent<Enemy>();
        if(healthBySize)enemy.health*=size;

        //Lunar Comets
        //if(GameSession.instance.shopOn){
        //if(GameSession.instance.gameModeSelected!=2){
            if(Random.Range(0,100)<lunarCometChance)isLunar=true;
            if(isLunar==true){
                spriteIndex=Random.Range(0,spritesLunar.Length);spriteRenderer.sprite=spritesLunar[spriteIndex];
                bFlame.part=lunarPart;
                var sizeA=Random.Range(sizeMinLunar, sizeMaxLunar);
                transform.localScale=new Vector2(sizeA, sizeA);

                enemy.health*=lunarHealthMulti;
                rb.velocity*=lunarSpeedMulti;
                if(GameSession.instance.shopOn)enemy.coinChance=1;
            }
        //}
        //}
        //rotationSpeed=Random.Range(2,8);
        //Destroy(this,0.3f);
    }
    public float Size(){
        return size;
    }
    public int LunarScore(){
        if(lunarScore==-1)if(lunarScoreE>0)lunarScore=Random.Range(lunarScoreS,lunarScoreE);
        return lunarScore;
    }
    public bool LunarDrop(){
        if(Random.Range(0,100)<lunarDropChance&&lunarDrop!=null)return true;else return false;
    }public GameObject GetLunarDrop(){
        return lunarDrop;
    }

    /*void Update(){
        transform.Rotate(new Vector3(0,0,rotationSpeed));
    }*/
}
[System.Serializable]public class CometScoreSize{
    public float size;
    public int score;
}