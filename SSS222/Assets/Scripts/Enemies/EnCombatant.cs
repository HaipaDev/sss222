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
    //[SerializeField] float speedFollow = 2f;
    [SerializeField] float speedFollowX = 3.5f;
    [SerializeField] float speedFollowY = 4f;
    //[SerializeField] float speedRush = 10f;
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
    Rigidbody2D rb;
    //GameSession gameSession;
    void Awake(){
    //Set Values
    var i=GameRules.instance;
    if(i!=null){
        var e=i.enCombatantSettings;
        speedFollowX=e.speedFollowX;
        speedFollowY=e.speedFollowY;
        vspeed=e.vspeed;
        distY=e.distY;
        distX=e.distX;
        distYPlayer=e.distYPlayer;
        saberPrefab=e.saberPrefab;
    }
    }
    void Start(){
        //if(FindObjectOfType<Tag_EnSaberWeapon>()==null){saber = Instantiate(saberPrefab,transform.position,Quaternion.identity);}//saber.GetComponent<FollowStrict>().targetObj=this.gameObject;}//saber.GetComponent<FollowOneObject>().targetObj=this.gameObject;}
        saber=gameObject.transform.GetChild(0).gameObject;
        player = FindObjectOfType<Player>();
        //enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
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
            if(dist>3.5f){
                if(distX>0.6f){
                    var dir = (playerPosX - selfPos).normalized;
                    selfPos += dir * speedFollowX * Time.deltaTime;
                    transform.position = selfPos;
                }else{
                    var dir = (playerPos - selfPos).normalized;
                    selfPos += dir * speedFollowX * Time.deltaTime;
                    transform.position = selfPos;
                }
            }else{
                /*var dir = (playerPosYDist - selfPos).normalized;
                selfPos += dir * speedFollow * Time.deltaTime;
                transform.position = selfPos;*/
                if(dist<2f){
                    var attack=Attack(stepY);
                    if(attack==null)StartCoroutine(attack);
                    //transform.position=Vector2.MoveTowards(selfPos,playerPosYDist,stepY*5);
                }else{
                    transform.position=Vector2.MoveTowards(selfPos,playerPosYDist,stepY);
                    //Debug.Log(playerPosYDist);
                }
            }
        }
        if(selfPos.y>playerPos.y){transform.localRotation=new Quaternion(0,0,0,0);saber.GetComponent<FollowStrict>().yy=-1.12f;dir=-1;}
        else if(selfPos.y<playerPos.y){transform.localRotation=new Quaternion(0,0,180,0);saber.GetComponent<FollowStrict>().yy=1.12f;dir=1;}
        //Debug.Log(stepY);
        //Debug.Log(dist);
    }
    IEnumerator Attack(float stepY){
        rb.velocity=Vector2.MoveTowards(selfPos,playerPos,stepY*5);
        yield return new WaitForSeconds(1f);
        rb.velocity=Vector2.MoveTowards(selfPos,playerPosYDist,stepY);
    }
}
