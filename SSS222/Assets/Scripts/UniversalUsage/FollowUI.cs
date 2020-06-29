using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour{
    Vector2 targetPos;
    //new Vector2 selfPos;
    //[SerializeField] GameObject targetOne;
    [SerializeField] GameObject target;
    [SerializeField] string targetTag;
    //public float dist;
    [SerializeField] float xx;
    [SerializeField] float yy;
    //[SerializeField] Quaternion rotation;

    //Player player;
    public GameObject targetObj;
    //Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if(targetObj==null){
            if(target!=null){targetObj = GameObject.FindGameObjectWithTag(target.tag); }
            else{if(targetTag!="")targetObj = GameObject.FindGameObjectWithTag(targetTag); }
        }
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetObj!=null){
        targetPos = new Vector2(targetObj.transform.position.x+xx, targetObj.transform.position.y+yy);
        //selfPos = new Vector2(transform.position.x, transform.position.y);
        //dist=Vector2.Distance(targetPos, selfPos);
        transform.position = Camera.main.WorldToScreenPoint(targetPos);
        }
    }
}