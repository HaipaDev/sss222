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
    int shoot;
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
        //if(closestEnemy!=null){
            if(shoot==0)shoot=1;
            var shootHealBullets=ShootHealBullets();
            //if(shootHealBullets==null)
            if(shoot==1){StartCoroutine(shootHealBullets);shoot=-1;}
        //}else{shoot=0;}
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
