using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnShip : MonoBehaviour{
    Vector2 playerPosX;
    Vector2 playerPos;
    Vector2 posY;
    Vector2 selfPos;
    Transform target;
    [SerializeField] float speedFollow = 2f;
    [SerializeField] float vspeed = 0.1f;
    [SerializeField] float distY = 1.3f;
    [SerializeField] float distX = 0.3f;
    [SerializeField] bool getClose = false;
    void Awake(){
    //Set Values
    var i=GameRules.instance;
    if(i!=null){
        var e=i.enShipSettings;
        speedFollow=e.speedFollow;
        vspeed=e.vspeed;
        distY=e.distY;
        distX=e.distX;
        getClose=e.getClose;
    }
    }
    void Start(){
        posY=new Vector2(transform.position.x,transform.position.y-distY);
    }

    void Update(){
    if(Player.instance!=null){
        float stepY = vspeed * Time.deltaTime;
        float stepX = speedFollow * Time.deltaTime;
        playerPosX = new Vector2(Player.instance.transform.position.x, transform.position.y);
        playerPos = new Vector2(Player.instance.transform.position.x, Player.instance.transform.position.y);
        selfPos = new Vector2(transform.position.x,transform.position.y);
        var dist = Vector2.Distance(playerPosX, selfPos);

        if(transform.position.y>posY.y){transform.position=new Vector2(Player.instance.transform.position.x,transform.position.y-vspeed);}
        else{
            if(getClose!=true){
                var dir=(playerPosX-selfPos).normalized;
                selfPos+=dir*speedFollow*Time.deltaTime;
                transform.position=selfPos;
            }else{
                var dir=(playerPos-selfPos).normalized;
                selfPos+=dir*speedFollow*Time.deltaTime;
                transform.position=selfPos;
            }
        }
    }
    }
}
