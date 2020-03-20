using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour{
    Vector2 targetPos;
    Vector2 selfPos;
    [SerializeField] GameObject target;
    [SerializeField] string targetTag;
    public float dist;
    [SerializeField] float distReq = 4f;
    [SerializeField] float speedFollow = 5f;
    [SerializeField] float vspeed = 2.4f;
    [SerializeField] bool rotateTowards = false;
    [SerializeField] float speedRotate = 15f;
    [SerializeField] bool followAfterOOR;
    [SerializeField] bool dirYYUp = false;
    [SerializeField] float OOR_YY = 1.5f;

    //Player player;
    GameObject targetObj;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if(target!=null){targetObj = GameObject.FindGameObjectWithTag(target.tag); }
        else{targetObj = GameObject.FindGameObjectWithTag(targetTag); }
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = new Vector2(targetObj.transform.position.x, targetObj.transform.position.y);
        selfPos = new Vector2(transform.position.x, transform.position.y);
        dist=Vector2.Distance(targetPos, selfPos);

        float step = speedFollow * Time.deltaTime;
        if(followAfterOOR==true){
            if(dist<distReq){ transform.position = Vector2.MoveTowards(transform.position, targetPos, step); }
            else{ rb.velocity = new Vector2(0f, -vspeed); }
        }else{
            if(dirYYUp==true){
                if(transform.position.y < targetObj.transform.position.y + OOR_YY){// && transform.position.y > targetObj.transform.position.y - OOR_YY+0.5){
                    if (dist < distReq) { transform.position = Vector2.MoveTowards(transform.position, targetPos, step); }
                    else { rb.velocity = new Vector2(0f, -vspeed); }
                }else { rb.velocity = new Vector2(0f, -vspeed); }
            }else{
                if (transform.position.y > targetObj.transform.position.y - OOR_YY){// && transform.position.y < targetObj.transform.position.y + OOR_YY + 0.5){
                    if (dist < distReq) { transform.position = Vector2.MoveTowards(transform.position, targetPos, step); }
                    else { rb.velocity = new Vector2(0f, -vspeed); }
                }else { rb.velocity = new Vector2(0f, -vspeed); }
            }
        }

        if(rotateTowards==true){
            if (followAfterOOR == true){
                float stepR = speedRotate * Time.deltaTime;
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetObj.transform.rotation, stepR);

                Vector3 vectorToTarget = targetObj.transform.position - transform.position;
                float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * stepR);
            }
        }
    }
}
