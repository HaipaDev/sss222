using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingDrone : MonoBehaviour{
    //[Header("Properties")]
    [SerializeField] GameObject healBallPrefab;
    [SerializeField] float shootFrequency=0.2f;
    [SerializeField] float speedBullet=4f;
    [Header("Values")]
    public Enemy closestEnemy;
    public Tag_LivingEnemy closestLivingEnemy;
    bool shoot=true;
    void Awake(){
    //Set Values
    var i=GameRules.instance;
    if(i!=null){
        var e=i.healingDroneSettings;
        healBallPrefab=e.healBallPrefab;
        shootFrequency=e.shootFrequency;
        speedBullet=e.speedBullet;
    }
    }

    void Update(){
        closestEnemy=FindClosestEnemy();
        closestLivingEnemy=FindClosestLivingEnemy();
        var shootHealBullets=ShootHealBullets();
        if(shoot==true&&GetComponent<Enemy>().shooting){StartCoroutine(shootHealBullets);shoot=false;}
    }

    IEnumerator ShootHealBullets(){
        yield return new WaitForSeconds(shootFrequency);
        var healBall=Instantiate(healBallPrefab,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        float step=speedBullet*Time.deltaTime;
        if(closestLivingEnemy!=null){
            healBall.GetComponent<FollowOneObject>().targetObj=closestLivingEnemy.gameObject;
            healBall.GetComponent<FollowOneObject>().speedFollow=4f;}
        else{healBall.GetComponent<FollowOneObject>().targetObj=this.gameObject;}
        
        shoot=true;
    }


    

    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies = new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr = FindObjectsOfType<Enemy>();
        //List<string> sublist = Enemies.FindAll(isHealingDrone);
        foreach(Enemy enemy in EnemiesArr){
            if(enemy.GetComponent<HealingDrone>()==null&&enemy.health<enemy.healthStart*1.5f)Enemies.Add(enemy);
            //if(enemy.GetComponent<HealingDrone>()!=null){Enemies.RemoveAt(System.Array.IndexOf(EnemiesArr, this));}
            //Debug.Log(System.Array.IndexOf(EnemiesArr, this));
            //Enemies.Find(this.GetComponent<HealingDrone>() healDrone);
            //Enemies.RemoveAll(this.GetComponent<HealingDrone>() healDrone);
        }
        Enemy closest=Enemies.FindClosest(transform.position);
        return closest;
    }
    public Tag_LivingEnemy FindClosestLivingEnemy(){
        KdTree<Tag_LivingEnemy> Enemies = new KdTree<Tag_LivingEnemy>();
        Tag_LivingEnemy[] EnemiesArr;
        EnemiesArr = FindObjectsOfType<Tag_LivingEnemy>();
        //List<string> sublist = Enemies.FindAll(isHealingDrone);
        foreach(Tag_LivingEnemy enemy in EnemiesArr){
            if(enemy.GetComponent<HealingDrone>()==null)Enemies.Add(enemy);
            //if(enemy.GetComponent<HealingDrone>()!=null){Enemies.RemoveAt(System.Array.IndexOf(EnemiesArr, this));}
            //Debug.Log(System.Array.IndexOf(EnemiesArr, this));
            //Enemies.Find(this.GetComponent<HealingDrone>() healDrone);
            //Enemies.RemoveAll(this.GetComponent<HealingDrone>() healDrone);
        }
        Tag_LivingEnemy closest = Enemies.FindClosest(transform.position);
        return closest;
    }

    //static HealingDrone isHealingDrone(){return this;}
}
