using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour{
    [HeaderAttribute("Target")]
    public Vector2 selfPos;
    public Vector2 targetPos;
    [SerializeField] public GameObject target;
    [SerializeField] string targetTag;
    public GameObject targetObj;
    public float dist;
    [HeaderAttribute("Properties")]
    [SerializeField] public float distReq=4f;
    [SerializeField] public float speedFollow=5f;
    [SerializeField] public float vspeed=2.4f;
    [SerializeField] public float hspeed=0f;
    [HeaderAttribute("Rotation")]
    [SerializeField] bool rotateTowards=false;
    [SerializeField] float speedRotate=15f;
    [SerializeField] float angleAdj=-90f;
    [HeaderAttribute("Other")]
    [SerializeField] bool followAfterOOR;
    [SerializeField] bool dirYYUp=false;
    [SerializeField] float OOR_YY=1.5f;
    [SerializeField] bool followClosestEnemy=false;
    Rigidbody2D rb;

    void Start(){
        if(target!=null){targetObj=GameObject.FindGameObjectWithTag(target.tag);}
        else{targetObj=GameObject.FindGameObjectWithTag(targetTag);}
        if(followClosestEnemy==true){if(FindClosestEnemy()!=null)targetObj=FindClosestEnemy().gameObject;}
        rb=GetComponent<Rigidbody2D>();
        if(GetComponent<Tag_PauseVelocity>()!=null){gameObject.AddComponent<Tag_PauseVelocity>();}
   }

    void Update(){
        if(target!=null){targetObj=GameObject.FindGameObjectWithTag(target.tag);}
        else{targetObj=GameObject.FindGameObjectWithTag(targetTag);}

        if(targetObj==null){rb.velocity=new Vector2(hspeed,-vspeed);}
        else{
            targetPos=new Vector2(targetObj.transform.position.x,targetObj.transform.position.y);
            selfPos=new Vector2(transform.position.x,transform.position.y);
            dist=Vector2.Distance(targetPos,selfPos);

            float step=speedFollow*Time.deltaTime;
            if(followAfterOOR==true){
                if(dist<distReq){transform.position=Tag_PauseVelocity.MoveTowards(transform.position,targetPos,step);}
                else{rb.velocity=new Vector2(hspeed,-vspeed);}
            }else{
                if(dirYYUp==true){
                    if(transform.position.y<targetObj.transform.position.y+OOR_YY){// && transform.position.y>targetObj.transform.position.y - OOR_YY+0.5){
                        if(dist<distReq){transform.position=Tag_PauseVelocity.MoveTowards(transform.position,targetPos,step);}
                        else{rb.velocity=new Vector2(hspeed,-vspeed);}
                   }else{rb.velocity=new Vector2(hspeed,-vspeed);}
                }else{
                    if(transform.position.y>targetObj.transform.position.y-OOR_YY){// && transform.position.y<targetObj.transform.position.y + OOR_YY + 0.5){
                        if(dist<distReq){transform.position=Tag_PauseVelocity.MoveTowards(transform.position,targetPos,step);}
                        else{rb.velocity=new Vector2(hspeed,-vspeed);}
                   }else{rb.velocity=new Vector2(hspeed,-vspeed);}
                }
            }

            if(rotateTowards==true){
                if(followAfterOOR==true){
                    float stepR=speedRotate*Time.deltaTime;
                    //transform.rotation=Quaternion.RotateTowards(transform.rotation,targetObj.transform.rotation,stepR);

                    Vector3 vectorToTarget=targetObj.transform.position-transform.position;
                    float angle=(Mathf.Atan2(vectorToTarget.y,vectorToTarget.x)*Mathf.Rad2Deg)+angleAdj;
                    Quaternion q=Quaternion.AngleAxis(angle,Vector3.forward);
                    transform.rotation=Quaternion.Slerp(transform.rotation,q,Time.deltaTime*stepR);
               }
           }
       }
   }
    public Enemy FindClosestEnemy(){
        KdTree<Enemy> Enemies=new KdTree<Enemy>();
        Enemy[] EnemiesArr;
        EnemiesArr=FindObjectsOfType<Enemy>();
        foreach(Enemy enemy in EnemiesArr){
            Enemies.Add(enemy);
        }
        Enemy closest=Enemies.FindClosest(transform.position);
        return closest;
   }
}
