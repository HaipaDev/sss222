using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometRandomProperties : MonoBehaviour{
    [SerializeField] Sprite[] sprites;
    [SerializeField] Sprite[] spritesLunar;
    [SerializeField] int lunarCometChance = 10;
    [SerializeField] float lunarHealthMulti = 2.5f;
    [SerializeField] float lunarSpeedMulti = 0.3f;
    [SerializeField] GameObject lunarPart;

    SpriteRenderer spriteRenderer;
    Enemy enemy;
    BackflameEffect bFlame;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        bFlame = GetComponent<BackflameEffect>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        var spriteIndex = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[spriteIndex];
        var size= Random.Range(0.4f, 1.4f);
        transform.localScale = new Vector2(size,size);

        var angle = Random.Range(0f,360f);
        transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);

        enemy = GetComponent<Enemy>();
        enemy.health *= size;


        var chance = Random.Range(0,100);
        if(chance < lunarCometChance){
            spriteIndex = Random.Range(0,spritesLunar.Length); spriteRenderer.sprite = spritesLunar[spriteIndex];
            bFlame.part = lunarPart;
            var sizeA = Random.Range(0.88f, 1.55f);
            transform.localScale = new Vector2(sizeA, sizeA);

            enemy.health *= lunarHealthMulti;
            enemy.Coinchance = 1;
            rb.velocity *= lunarSpeedMulti;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
