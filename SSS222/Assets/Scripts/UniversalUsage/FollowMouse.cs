using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour{
    public Vector2 targetPos;
    public Vector2 selfPos;
    //[SerializeField] GameObject target;
    //[SerializeField] string targetTag;
    public float dist;
    //[SerializeField] float distReq = 100f;
    [SerializeField] float speedFollow = 5f;
    [SerializeField] public float vspeed = 0f;
    [SerializeField] public float hspeed = 0f;
    [SerializeField] bool rotateTowards = false;
    [SerializeField] float speedRotate = 15f;
    [SerializeField] float angleAdj = -90f;
    //[SerializeField] bool followAfterOOR;
    //[SerializeField] bool dirYYUp = false;
    //[SerializeField] float OOR_YY = 1.5f;

    //Player player;
    //public GameObject targetObj;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        //if(target!=null){targetObj = GameObject.FindGameObjectWithTag(target.tag); }
        //else{targetObj = GameObject.FindGameObjectWithTag(targetTag); }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (target != null) { targetObj = GameObject.FindGameObjectWithTag(target.tag); }
        //else { targetObj = GameObject.FindGameObjectWithTag(targetTag); }

        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selfPos = new Vector2(transform.position.x, transform.position.y);
        dist=Vector2.Distance(targetPos, selfPos);

        float step = speedFollow * Time.deltaTime;
        //if(followAfterOOR==true){
            //if(dist<distReq){ 
                transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
        //}else{ rb.velocity = new Vector2(hspeed, -vspeed); }
        /*}else{
            if(dirYYUp==true){
                if(transform.position.y < targetPos.y + OOR_YY){// && transform.position.y > targetObj.transform.position.y - OOR_YY+0.5){
                    if (dist < distReq) { transform.position = Vector2.MoveTowards(transform.position, targetPos, step); }
                    else { rb.velocity = new Vector2(hspeed, -vspeed); }
                }else { rb.velocity = new Vector2(hspeed, -vspeed); }
            }else{
                if (transform.position.y > targetPos.y - OOR_YY){// && transform.position.y < targetObj.transform.position.y + OOR_YY + 0.5){
                    if (dist < distReq) { transform.position = Vector2.MoveTowards(transform.position, targetPos, step); }
                    else { rb.velocity = new Vector2(hspeed, -vspeed); }
                }else { rb.velocity = new Vector2(hspeed, -vspeed); }
            }
        }*/

        if(rotateTowards==true){
            //if (followAfterOOR == true){
                float stepR = speedRotate * Time.deltaTime;
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetObj.transform.rotation, stepR);

                Vector3 vectorToTarget = targetPos - new Vector2(transform.position.x, transform.position.y);
                float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) + angleAdj;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * stepR);
            //}
        }
    }
}
