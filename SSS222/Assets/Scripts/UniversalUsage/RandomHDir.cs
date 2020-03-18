using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHDir : MonoBehaviour{
    [SerializeField] float chanceForR=50;
    [SerializeField] float hspeed = 1f;

    Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        var dir = 1;
        if(Random.Range(0,100)<chanceForR) { dir = -1; }
        rb.velocity = new Vector2(dir * hspeed, rb.velocity.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
