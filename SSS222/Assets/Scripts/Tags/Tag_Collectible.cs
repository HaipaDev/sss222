using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_Collectible : MonoBehaviour{
    [SerializeField] public dir dir=dir.down;
    [SerializeField] public float speed=4f;
    [SerializeField] public bool isPowerup=false;
    [SerializeField] public bool isDestructible=true;
    void Start(){
        if(GameSession.maskMode!=0)GetComponent<SpriteRenderer>().maskInteraction=(SpriteMaskInteraction)GameSession.maskMode;

        SetDirSpeed();
        if(GetComponent<Tag_PauseVelocity>()==null){gameObject.AddComponent<Tag_PauseVelocity>();}

        if(isDestructible&&GetComponent<HLaserKillThis>()==null)gameObject.AddComponent<HLaserKillThis>();
    }
    public void SetDirSpeed(){
        if(dir==dir.up)GetComponent<Rigidbody2D>().velocity=Vector2.up*speed;
        else if(dir==dir.left)GetComponent<Rigidbody2D>().velocity=Vector2.left*speed;
        else if(dir==dir.right)GetComponent<Rigidbody2D>().velocity=Vector2.right*speed;
        else GetComponent<Rigidbody2D>().velocity=Vector2.down*speed;
    }
}