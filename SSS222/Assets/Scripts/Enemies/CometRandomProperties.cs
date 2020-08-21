using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometRandomProperties : MonoBehaviour{
    [Header("Basic")]
    [SerializeField] float sizeMin=0.4f;
    [SerializeField] float sizeMax=1.4f;
    [SerializeField] bool healthBySize=true;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject bflamePart;
    [Header("Lunar")]
    [SerializeField] float sizeMinLunar=0.88f;
    [SerializeField] float sizeMaxLunar=1.55f;
    [SerializeField] int lunarCometChance = 10;
    [SerializeField] float lunarHealthMulti = 2.5f;
    [SerializeField] float lunarSpeedMulti = 0.415f;
    [SerializeField] Sprite[] spritesLunar;
    [SerializeField] GameObject lunarPart;

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
        sprites=e.sprites;
        bflamePart=e.bflamePart;

        sizeMinLunar=e.sizeMinLunar;
        sizeMaxLunar=e.sizeMaxLunar;
        lunarCometChance=e.lunarCometChance;
        lunarHealthMulti=e.lunarHealthMulti;
        lunarSpeedMulti=e.lunarSpeedMulti;
        spritesLunar=e.spritesLunar;
        lunarPart=e.lunarPart;
    }
    }
    void Start()
    {
        if(!GameSession.instance.shopOn){Destroy(this);}
        bFlame = GetComponent<BackflameEffect>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        var spriteIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[spriteIndex];
        var size= Random.Range(sizeMin, sizeMax);
        transform.localScale = new Vector2(size,size);

        var angle = Random.Range(0f,360f);
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);

        enemy = GetComponent<Enemy>();
        if(healthBySize)enemy.health *= size;

        //Lunar Comets
        if(GameSession.instance.shopOn){
            var chanceLunar = Random.Range(0,100);
            if(chanceLunar < lunarCometChance){
                spriteIndex = Random.Range(0,spritesLunar.Length); spriteRenderer.sprite = spritesLunar[spriteIndex];
                bFlame.part = lunarPart;
                var sizeA = Random.Range(sizeMinLunar, sizeMaxLunar);
                transform.localScale = new Vector2(sizeA, sizeA);

                enemy.health *= lunarHealthMulti;
                enemy.coinChance = 1;
                rb.velocity *= lunarSpeedMulti;
            }
        }
        //rotationSpeed=Random.Range(2,8);
        Destroy(this,0.5f);
    }

    void Update(){
        //transform.Rotate(new Vector3(0,0,rotationSpeed));
    }
}
