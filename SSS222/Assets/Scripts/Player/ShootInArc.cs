using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInArc : MonoBehaviour{
    [SerializeField] public float speed=9f;
    [SerializeField] public float gravity=9f;
    [SerializeField] public float firingAngle=45f;
    [SerializeField] Transform Target;
    [SerializeField] float target_Distance;
    private void Start() {
        Shoot();
    }private void Update() {
        //if(Target!=null)target_Distance=Vector2.Distance(transform.position, Target.position);
    }
    public void Shoot(){
        StartCoroutine(ShootI());
    }
    IEnumerator ShootI(){
        yield return new WaitForSeconds(0.01f);
        Target=FindFurthestEnemy().transform;

        if(Target!=null){
            // Calculate distance to target
            target_Distance = Vector2.Distance(transform.position, Target.position);
    
            // Calculate the velocity needed to throw the object to the target at specified angle.
            float transform_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
    
            // Extract the X  Y componenent of the velocity
            float Vx = Mathf.Sqrt(transform_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(transform_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
    
            // Calculate flight time.
            float flightDuration = target_Distance / Vx;
    
            // Rotate transform to face the target.
            //transform.rotation = Quaternion.LookRotation(Target.position - transform.position);
        
            float elapse_time = 0;
    
            while (elapse_time < flightDuration)
            {
                transform.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
            
                elapse_time += Time.deltaTime;
    
                yield return null;
            }
        }
    }
    Enemy FindFurthestEnemy(){
        Enemy[] EnemyList;
        EnemyList=FindObjectsOfType<Enemy>();
        float FurthestDistance = 0;
        Enemy FurthestObject = null;
        foreach(Enemy Enemy in EnemyList){
            float ObjectDistance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (ObjectDistance > FurthestDistance){
                FurthestObject = Enemy;
                FurthestDistance = ObjectDistance;
            }
        }
        return FurthestObject;
    }
    Enemy FindClosestEnemy(){
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
