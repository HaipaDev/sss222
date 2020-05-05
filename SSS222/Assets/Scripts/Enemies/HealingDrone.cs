using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingDrone : MonoBehaviour{
    [SerializeField] GameObject healBallPrefab;
    [SerializeField] float shootFrequency=0.2f;
    [SerializeField] float speedBullet=4f;
    [HeaderAttribute("Dodge")]
    public float distMin=1.6f;
    public float dodgeSpeed=2f;
    public float dodgeTime=0.5f;
    public float dodgeTimer;
    public int dodgeDir=1;
    [HeaderAttribute("Detecting Params")]
    public Vector2 selfPos;
    public Vector2 targetPos;
    public float dist;
    public Tag_PlayerWeapon closestWeapon;
    public Enemy closestEnemy;
    Rigidbody2D rb;
    float xMin;
    float xMax;
    float yMin;
    float yMax;
    float padding=0.5f;
    int shoot;
    //HealingDrone healDrone;
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        SetUpMoveBoundaries();
        //healDrone=this;
    }

    void Update(){
        selfPos = new Vector2(transform.position.x, transform.position.y);

        //closestWeapon=Weapons.FindClosest(transform.position);
        if(FindClosestWeapon()!=null){closestWeapon=FindClosestWeapon();
            targetPos = new Vector2(closestWeapon.transform.position.x, closestWeapon.transform.position.y);
            dist=Vector2.Distance(targetPos, selfPos);
        }else{
            dist=0f;
        }
        ChangeDir();

        Dodge();

        if(GetComponent<EnemyPathing>().waypointIndex>0){var clamp=true;if(clamp==true)ClampPosition();}

        closestEnemy=FindClosestEnemy();
        //if(closestEnemy!=null){
            if(shoot==0)shoot=1;
            var shootHealBullets=ShootHealBullets();
            //if(shootHealBullets==null)
            if(shoot==1){StartCoroutine(shootHealBullets);shoot=-1;}
        //}else{shoot=0;}
    }

    void Dodge(){
        if(dist<=distMin && dist>0){
            rb.velocity=new Vector2(dodgeSpeed*dodgeDir,0f);
            dodgeTimer=dodgeTime;
        }
        if(dodgeTimer>0f)dodgeTimer-=Time.deltaTime;
        if(dodgeTimer<=0)rb.velocity=Vector2.zero;
    }

    void ChangeDir(){
        if(targetPos.x>selfPos.x){dodgeDir=-1;}
        if(targetPos.x<selfPos.x){dodgeDir=1;}
    }
    private void SetUpMoveBoundaries(){
        xMin = -3.87f + padding;
        xMax = 3.87f - padding;
        yMin = -6.95f + padding;
        yMax = 7f - padding;
    }
    void ClampPosition(){
        var newXpos = Mathf.Clamp(transform.position.x, xMin, xMax);
        var newYpos = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector2(newXpos, newYpos);
    }

    IEnumerator ShootHealBullets(){
        yield return new WaitForSeconds(shootFrequency);
        var healBall = Instantiate(healBallPrefab, new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        float step = speedBullet * Time.deltaTime;
        //healBall.transform.position=Vector2.MoveTowards(transform.position, closestEnemy.transform.position, step);
        if(closestEnemy!=null){
            healBall.GetComponent<FollowOneObject>().targetObj=closestEnemy.gameObject;
            healBall.GetComponent<FollowOneObject>().speedFollow=4f;}
        else{healBall.GetComponent<FollowOneObject>().targetObj=this.gameObject;}
        
        StartCoroutine(ShootHealBullets());
    }


    public Tag_PlayerWeapon FindClosestWeapon(){
        KdTree<Tag_PlayerWeapon> Weapons = new KdTree<Tag_PlayerWeapon>();
        Tag_PlayerWeapon[] WeaponsArr;
        WeaponsArr = FindObjectsOfType<Tag_PlayerWeapon>();
        foreach(Tag_PlayerWeapon weapon in WeaponsArr){
            Weapons.Add(weapon);
        }
        Tag_PlayerWeapon closest = Weapons.FindClosest(transform.position);
        return closest;
    }

    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies = new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr = FindObjectsOfType<Enemy>();
        //List<string> sublist = Enemies.FindAll(isHealingDrone);
        foreach(Enemy enemy in EnemiesArr){
            if(enemy.GetComponent<HealingDrone>()==null)Enemies.Add(enemy);
            //if(enemy.GetComponent<HealingDrone>()!=null){Enemies.RemoveAt(System.Array.IndexOf(EnemiesArr, this));}
            //Debug.Log(System.Array.IndexOf(EnemiesArr, this));
            //Enemies.Find(this.GetComponent<HealingDrone>() healDrone);
            //Enemies.RemoveAll(this.GetComponent<HealingDrone>() healDrone);
        }
        Enemy closest = Enemies.FindClosest(transform.position);
        return closest;
    }

    //static HealingDrone isHealingDrone(){return this;}
}
