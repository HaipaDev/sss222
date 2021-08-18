using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDir : MonoBehaviour{
    [SerializeField] dir dir=dir.down;
    [SerializeField] float speed=4f;
    void Start(){
        if(dir==dir.up)GetComponent<Rigidbody2D>().velocity=Vector2.up*speed;
        else if(dir==dir.left)GetComponent<Rigidbody2D>().velocity=Vector2.left*speed;
        else if(dir==dir.right)GetComponent<Rigidbody2D>().velocity=Vector2.right*speed;
        else GetComponent<Rigidbody2D>().velocity=Vector2.down*speed;
        if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}
    }
    public float GetSpeed(){return speed;}
}
