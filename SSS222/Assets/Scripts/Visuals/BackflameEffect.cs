using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackflameEffect : MonoBehaviour{
    [SerializeField] public GameObject part;
    [SerializeField] bool twice=true;
    [SerializeField] float xx = 0f;
    [SerializeField] float yy = -0.6f;
    [SerializeField] float time = 0.3f;
    /*void Start()
    {
        xx = transform.position.x + xx;
        yy = transform.position.y + yy;
    }*/
    // Update is called once per frame
    void Update(){
        if(Time.timeScale>0.0001f){
            var xxx = transform.position.x + xx;
            var yyy = transform.position.y + yy;
            GameObject BFlame = Instantiate(part, new Vector3(xxx, yyy, transform.position.z - 0.01f), Quaternion.identity);
            Destroy(BFlame, time);
            if (twice == true) { GameObject BFlame2 = Instantiate(part, new Vector3(xxx, yyy, transform.position.z - 0.01f), Quaternion.identity); Destroy(BFlame2, time); }
        }
    }
}
