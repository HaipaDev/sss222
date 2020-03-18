using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour{
    public Rigidbody2D rb;
    [SerializeField] float vspeed = 4f;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, -vspeed);
    }

    // Update is called once per frame
    void Update(){
        //rb.velocity = new Vector2(0f, -10f);
    }
}
