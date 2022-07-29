using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloomyScythe : MonoBehaviour{
    [SerializeField] float rotationSpeed=12f;
    [SerializeField] float waitTime=0.65f;
    public float waitTimer;
    //public Vector2 savedSpeed;
    float vSpeed;
    [SerializeField] AudioClip scytheFlySFX;
    public float vSpeedMax=9;
    Rigidbody2D rb;
    void Start(){
        rb=GetComponent<Rigidbody2D>();
        //savedSpeed=rb.velocity;
        rb.velocity=Vector2.zero;
        waitTimer=waitTime;
        if(GetComponent<Follow>()!=null)GetComponent<Follow>().enabled=false;
    }
    void Update()
    {
        var rotation=transform.eulerAngles;
        var rotStep=rotationSpeed;
        var angle=rotation.z+=rotStep;
        transform.Rotate(new Vector3(0,0,rotStep*Time.deltaTime));
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle)*Time.deltaTime;

        if(vSpeed<vSpeedMax){vSpeed+=0.5f;}

        if(waitTimer>0){waitTimer-=Time.deltaTime;}
        if(waitTimer<=0&&waitTimer!=-4){rb.velocity=new Vector2(0,vSpeed);waitTimer=-4;AudioSource.PlayClipAtPoint(scytheFlySFX,transform.position);if(GetComponent<Follow>()!=null)GetComponent<Follow>().enabled=true;}
    }
}
