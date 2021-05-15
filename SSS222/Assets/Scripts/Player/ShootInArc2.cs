using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootInArc2 : MonoBehaviour {
	//public Transform shadow;

	public float speed=1;

	public Vector3 p0=new Vector3(0,0,0);
	public Vector3 p1=new Vector3(0,0,0);

	public float h=1;

	[Range(0,1)]
	public float s=0;

	public Vector3 UP = new Vector3(0,0,1);


	public Vector3 pc = new Vector3(0,0,0);
	public Vector3 CameraNormal = new Vector3(0, 1/Mathf.Sqrt(2), -1/Mathf.Sqrt(2));

	void Start(){
		CameraNormal.Normalize();
        if(Player.instance!=null)p0=(Player.instance.transform.position+FindFurthestEnemy().transform.position)/2;
        p0=new Vector3(p0.x+=0.25f,p0.y,p0.z);
        p1=FindFurthestEnemy().transform.position;
        pc=FindFurthestEnemy().transform.position;
	}
	
	void Update(){
		Vector3 fprime = f() - pc;
		Vector3 Vy = proj (UP);
		Vector3 Vx = Vector3.Cross (Vy, CameraNormal);
		float x = Vector3.Dot (fprime, Vx);
		float y = Vector3.Dot (fprime, Vy);
		this.transform.position = new Vector3 (x,y,this.transform.position.z);

		/*if (shadow != null) {
			fprime.z = 0;
			x = Vector3.Dot (fprime, Vx);
			y = Vector3.Dot (fprime, Vy);
			shadow.position = new Vector3 (x,y,shadow.position.z);
		}*/
		s = (s + speed * Time.deltaTime);
		if (s > 1) {
			s=s % 1;
		}
	}


	Vector3 f(){
		return s * p0 + (1 - s) * p1 + UP * 4 * h * s * (1 - s);
	}

	Vector3 proj(Vector3 p){
		return p - Vector3.Dot (p, CameraNormal) * CameraNormal;
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