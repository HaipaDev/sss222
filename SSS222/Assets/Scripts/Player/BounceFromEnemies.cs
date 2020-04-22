using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceFromEnemies : MonoBehaviour{
    void Start(){
        
    }

    void Update(){
        
    }
    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies = new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr = FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in EnemiesArr){
            Enemies.Add(enemy);
        }
        Enemy closest = Enemies.FindClosest(transform.position);
        return closest;
    }
}
