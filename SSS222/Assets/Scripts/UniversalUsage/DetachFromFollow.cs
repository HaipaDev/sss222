using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetachFromFollow : MonoBehaviour{
    [SerializeField] float catch_distance=5f;
    [SerializeField] public float shake_distance = 0.2f;
    [SerializeField] public int count_max = 3;
    [SerializeField] float fallSpeed = 14f;
    [SerializeField] GameObject shakeNotif;
    public bool attached = false;
    public bool detached = false;
    public int stage = 0;
    public int count = 0;
    public float dist;

    Follow follow;
    Rigidbody2D rb;
    TMPro.TextMeshProUGUI shakeNotifText;
    // Start is called before the first frame update
    void Start()
    {
        shakeNotif = GameObject.Find("ShakeQuickly");
        shakeNotifText = shakeNotif.GetComponent<TMPro.TextMeshProUGUI>();
        follow = GetComponent<Follow>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector2.Distance(follow.targetPos, follow.selfPos);
        if (dist<catch_distance && detached==false){
            attached = true;
        }/*else{
            if(attached==false){ }
        }*/

        if(attached==true){
            if (count < count_max)
            {
                if (follow.selfPos.x > follow.targetPos.x + shake_distance)
                {
                    stage = 1;
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
                    attached = false;
                    detached = true;
                    follow.enabled = false;
                    rb.velocity = new Vector2(fallSpeed,-fallSpeed);
                }
            }
            shakeNotifText.enabled = true;
        }else{
            shakeNotifText.enabled = false;
        }
    }
}
