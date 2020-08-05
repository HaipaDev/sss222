using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromFollow : MonoBehaviour{
    [HeaderAttribute("Properties")]
    [SerializeField] float catch_distance=1.5f;
    [SerializeField] public float shake_distance = 0.05f;
    [SerializeField] public int count_max = 3;
    [SerializeField] float fallSpeed = 6f;
    [SerializeField] AudioClip leechAttachSFX;
    [HeaderAttribute("Current")]
    public bool attached;
    public bool detached;
    public int stage = 0;
    public int count = 0;
    public float dist;

    Follow follow;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        follow = GetComponent<Follow>();
        //if(follow==null)follow=GetComponent<FollowOneObject>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector2.Distance(follow.targetPos, follow.selfPos);
        if (dist<catch_distance && detached==false){
            if(attached==false){
                AudioSource.PlayClipAtPoint(leechAttachSFX, new Vector2(transform.position.x, transform.position.y));
                attached = true;
            }
        }else{attached=false;}
        /*else{
            if(attached==false){ }
        }*/

        if(attached==true){
            if (count < count_max){
                if (follow.selfPos.x > follow.targetPos.x + shake_distance)
                {
                    if(stage==0)stage = 1;
                }
                else if (follow.selfPos.x < follow.targetPos.x - shake_distance)
                {
                    if (stage == 1) stage = 2;
                }

                if (stage == 2)
                {
                    count += 1;
                    stage = 0;
                }
            }else{
                if(follow.selfPos.x<follow.targetPos.x-shake_distance){
                    rb.velocity = new Vector2(fallSpeed,-fallSpeed);
                    follow.enabled = false;
                    detached = true;
                    attached = false;
                    //Destroy(gameObject);
                }
            }
        }
    }
}
