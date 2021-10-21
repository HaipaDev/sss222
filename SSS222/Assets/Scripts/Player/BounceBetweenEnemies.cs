using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBetweenEnemies : MonoBehaviour{
    [SerializeField] public float speed=9f;
    [SerializeField] Enemy enemyHit;
    public Vector2 savedVel;
    [SerializeField] int timesHitMax=5;
    public int timesHit;
    void Start(){
        savedVel=GetComponent<Rigidbody2D>().velocity;
    }
    void Update(){
        float step=speed*Time.deltaTime;
        var target=FindClosestEnemy();
        if(target!=null&&(timesHit<timesHitMax&&timesHitMax>0)){GetComponent<Rigidbody2D>().velocity=Vector2.zero;transform.position=Vector2.MoveTowards(transform.position,target.transform.position,step);}
        else if(target==null||(timesHit>=timesHitMax&&timesHitMax>0)){GetComponent<Rigidbody2D>().velocity=savedVel;}
    }
    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies=new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr=FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in EnemiesArr){
            //if(enemy.GetComponent<Enemy>().cTagged==false)Enemies.Add(enemy);
            if(enemy!=enemyHit)Enemies.Add(enemy);
        }
        Enemy closest=Enemies.FindClosest(transform.position);
        return closest;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!CompareTag(other.tag)){
            if(other.CompareTag("Enemy")){
                enemyHit=null;
                enemyHit=other.GetComponent<Enemy>();
                timesHit++;
            }
        }
    }
}
