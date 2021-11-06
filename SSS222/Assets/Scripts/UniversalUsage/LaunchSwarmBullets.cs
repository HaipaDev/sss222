﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchSwarmBullets : MonoBehaviour {
	[SerializeField] GameObject projectile;
	[SerializeField] int numberOfProjectiles=4;
	[SerializeField] float radius=2;
	[SerializeField] float moveSpeed=5;

	Vector2 startPoint;
	void Awake(){
		//Set Values
		var i=GameRules.instance;
		if(GetComponent<Goblin>()!=null&&i!=null){
		
		}
    }
	void Update () {
        //startPoint=transform.position;
		/*if (Input.GetButtonDown ("Fire1")) {
			startPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			SpawnProjectiles (numberOfProjectiles);
		}*/
	}
    public void Shoot(){
		startPoint=transform.position;
        SpawnProjectiles(numberOfProjectiles);
    }

	public void SpawnProjectiles(int numberOfProjectiles)
	{
		float angleStep = 160f / numberOfProjectiles;
		float angle = 100f;

		for (int i = 0; i <= numberOfProjectiles - 1; i++) {
			
			float projectileDirXposition = startPoint.x + Mathf.Sin ((angle * Mathf.PI) / 180) * radius;
			float projectileDirYposition = startPoint.y + Mathf.Cos ((angle * Mathf.PI) / 180)* radius;

			Vector2 projectileVector = new Vector2 (projectileDirXposition, projectileDirYposition);
			Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * moveSpeed;

			var proj = Instantiate (projectile, startPoint, Quaternion.identity);
			proj.GetComponent<Rigidbody2D> ().velocity = 
				new Vector2 (projectileMoveDirection.x, projectileMoveDirection.y);

			angle += angleStep;
		}
		//if(rotate==true){transform.Rotate(0.0f, 0.0f, angle, Space.Self);}
	}
	public void SetProjectile(GameObject proj){
		projectile=proj;
	}
}