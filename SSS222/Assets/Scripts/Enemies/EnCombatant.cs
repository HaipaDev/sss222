using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnCombatant : MonoBehaviour{
    Vector2 playerPos;
    Vector2 playerPosX;
    Vector2 playerPosYDist;
    Vector2 posY;
    Vector2 selfPos;
    Vector2 selfPosX;
    Transform target;
    [SerializeField] float speedFollow = 2f;
    [SerializeField] float speedFollowX = 3.5f;
    [SerializeField] float speedFollowY = 4f;
    [SerializeField] float speedRush = 10f;
    [SerializeField] float vspeed = 0.1f;
    [SerializeField] float distY = 1.3f;
    [SerializeField] float distX = 0.3f;
    [SerializeField] float distYPlayer = 1.5f;
    [SerializeField] GameObject saberPrefab;
    public GameObject saber;
    public float dist;
    public float distXX;
    int dir=-1;

    Player player;
    //Enemy enemy;
    //Rigidbody2D rb;
    //GameSession gameSession;
    void Start(){
        if(FindObjectOfType<Tag_EnSaberWeapon>()==null){saber = Instantiate(saberPrefab);}//saber.GetComponent<FollowOneObject>().targetObj=this.gameObject;}
        player = FindObjectOfType<Player>();
        //enemy = GetComponent<Enemy>();
        //rb = GetComponent<Rigidbody2D>();
        //gameSession = FindObjectOfType<GameSession>();

        posY = new Vector2(transform.position.x, transform.position.y - distY);
    }

    void Update(){
        //float stepY = vspeed * Time.deltaTime;
        float stepX = speedFollowX * Time.deltaTime;
        float stepY = speedFollowY * Time.deltaTime;
        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        playerPosX = new Vector2(player.transform.position.x, transform.position.y);
        playerPosYDist = new Vector2(player.transform.position.x, player.transform.position.y+(distYPlayer*dir));
        selfPos = new Vector2(transform.position.x,transform.position.y);
        selfPosX = new Vector2(transform.position.x,player.transform.position.y);
        dist = Vector2.Distance(playerPos, selfPos);
        distXX = Vector2.Distance(playerPosX, selfPos);

        if (transform.position.y>posY.y){ transform.position = new Vector2(player.transform.position.x, transform.position.y - vspeed); }
        else {
            /*if(transform.position.x < player.transform.position.x+ distX)
            {
                transform.position = new Vector2(transform.position.x + speedFollow, transform.position.y);
            }else if(transform.position.x > player.transform.position.x+distX)
            {
                transform.position = new Vector2(transform.position.x - speedFollow, transform.position.y);
            }
            else{}*/
            //if (!target) { return; }
            //if(selfPos.x>playerPos.x || selfPos.x<playerPos.x){
            if(distX>0.6f){
                var dir = (playerPosX - selfPos).normalized;
                selfPos += dir * speedFollowX * Time.deltaTime;
                transform.position = selfPos;
            }else{
                /*var dir = (playerPosYDist - selfPos).normalized;
                selfPos += dir * speedFollow * Time.deltaTime;
                transform.position = selfPos;*/
                transform.position=Vector2.MoveTowards(selfPos,playerPosYDist,stepY);
            }
        }
        if(selfPos.y>playerPos.y){transform.localRotation=new Quaternion(0,0,0,0);saber.GetComponent<FollowStrict>().yy=-1.12f;dir=-1;}
        else if(selfPos.y<playerPos.y){transform.localRotation=new Quaternion(0,0,180,0);saber.GetComponent<FollowStrict>().yy=1.12f;dir=1;}
        //Debug.Log(stepY);
    }
}
