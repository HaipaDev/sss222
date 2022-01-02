using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnShip : MonoBehaviour{
    Vector2 playerPosX;
    Vector2 playerPos;
    Vector2 posY;
    Vector2 selfPos;
    Transform target;
    [SerializeField] float speedFollow=3.75f;
    [SerializeField] float vspeed=2f;
    [SerializeField] float distY=1.3f;
    void Awake(){
        var i=GameRules.instance;
        if(i!=null){
            var e=i.enShipSettings;
            speedFollow=e.speedFollow;
            vspeed=e.vspeed;
            distY=e.distY;
        }
    }
    void Start(){
        posY=new Vector2(transform.position.x,transform.position.y-distY);
    }

    void Update(){  if(!GameSession.GlobalTimeIsPaused&&Player.instance!=null){
        float stepY=vspeed*Time.deltaTime;
        float stepX=speedFollow*Time.deltaTime;
        playerPosX=new Vector2(Player.instance.transform.position.x,transform.position.y);
        selfPos=new Vector2(transform.position.x,transform.position.y);
        var distX=Vector2.Distance(playerPosX,selfPos);

        if(transform.position.y>posY.y){selfPos=new Vector2(playerPosX.x,selfPos.y-stepY);}//Fly in
        else{
            if(distX>0.1f){
                var dir=(playerPosX-selfPos).normalized;
                selfPos+=dir*stepX;
            }
        }

        transform.position=selfPos;
    }}
}
