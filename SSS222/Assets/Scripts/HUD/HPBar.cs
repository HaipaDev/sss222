using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour{
    Player player;
    float playerHP;
    float playerMaxHP;
    public bool gclover = false;
    Sprite HPBarNormal;
    [SerializeField] Sprite HPBarGold;
    // Start is called before the first frame update
    void Start(){
        player = FindObjectOfType<Player>();
        playerMaxHP = player.GetComponent<Player>().maxHP;
        HPBarNormal = GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update(){
        playerHP = player.GetComponent<Player>().health;
        playerMaxHP = player.GetComponent<Player>().maxHP;
        transform.localScale = new Vector2(playerHP / playerMaxHP, 1);
        if(gclover==true){ GetComponent<SpriteRenderer>().sprite = HPBarGold; }
        else{ GetComponent<SpriteRenderer>().sprite = HPBarNormal; }
    }
}
