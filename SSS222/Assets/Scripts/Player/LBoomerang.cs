using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBoomerang : MonoBehaviour{
    [SerializeField] float speed=9f;
    [SerializeField] List<Enemy> enemiesHit;
    void Update(){
        float step = speed * Time.deltaTime;
        if((FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().ammo==-1)||FindObjectOfType<Player>()==null){
            var target=FindClosestEnemy();
            if(target!=null)transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }else if(FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().ammo!=-1){
            transform.position = Vector2.MoveTowards(transform.position, FindObjectOfType<Player>().transform.position, step);
        }
    }
    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies = new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in EnemiesArr){
            //if(enemy.GetComponent<Enemy>().cTagged==false)Enemies.Add(enemy);
            if(!enemiesHit.Contains(enemy))Enemies.Add(enemy);
        }
        Enemy closest = Enemies.FindClosest(transform.position);
        return closest;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!CompareTag(other.tag)){
            if(other.CompareTag("Enemy")){
            //other.GetComponent<Enemy>().cTagged=true;
            enemiesHit.Add(other.GetComponent<Enemy>());
            }
        }
    }
}
