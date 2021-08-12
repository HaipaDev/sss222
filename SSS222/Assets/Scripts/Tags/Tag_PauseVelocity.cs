using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PauseVelocity : MonoBehaviour{
    public Vector2 velPaused;
    Rigidbody2D rb;
    void Start() {
        rb=GetComponent<Rigidbody2D>();
    }
    void Update(){
        if(rb.velocity!=Vector2.zero){velPaused=rb.velocity;}
        if(GameSession.GlobalTimeIsPaused){if(rb.velocity!=Vector2.zero){velPaused=rb.velocity;rb.velocity=Vector2.zero;}}else{if(velPaused!=Vector2.zero)rb.velocity=velPaused;}
    }
    public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta){if(!GameSession.GlobalTimeIsPaused){return Vector2.MoveTowards(current,target,maxDistanceDelta);}else{return current;}}
}
