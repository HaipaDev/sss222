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
    bool shoot=true;
    void Awake(){
        var i=GameRules.instance;
        if(i!=null){
            var e=i.healingDroneSettings;
            healBallPrefab=e.healBallPrefab;
            shootFrequency=e.shootFrequency;
            speedBullet=e.speedBullet;
        }
    }
    void Update(){
        closestEnemy=FindClosestHealableEnemy();
        var shootHealBullets=ShootHealBullets();
        if(shoot==true&&GetComponent<Enemy>().shooting){StartCoroutine(shootHealBullets);shoot=false;}
    }

    IEnumerator ShootHealBullets(){
        yield return new WaitForSeconds(shootFrequency);
        var healBall=Instantiate(healBallPrefab,new Vector2(transform.position.x,transform.position.y),Quaternion.identity);
        float step=speedBullet*Time.deltaTime;
        if(closestEnemy!=null){
            healBall.GetComponent<FollowOneObject>().targetObj=closestEnemy.gameObject;
            healBall.GetComponent<FollowOneObject>().speedFollow=4f;}
        else{healBall.GetComponent<FollowOneObject>().targetObj=this.gameObject;}
        
        shoot=true;
    }

    public Enemy FindClosestHealableEnemy(){
        KdTree<Enemy> EnemiesHealable=new KdTree<Enemy>();
        Enemy[] EnemiesArr=System.Array.FindAll(FindObjectsOfType<Enemy>(),x=>x._healable());
        foreach(Enemy enemy in EnemiesArr){
            if(enemy.gameObject!=this.gameObject&&enemy.GetComponent<HealingDrone>()==null&&enemy.health<enemy.healthMax)EnemiesHealable.Add(enemy);
            
        }
        Enemy closest=EnemiesHealable.FindClosest(transform.position);
        return closest;
    }
}
