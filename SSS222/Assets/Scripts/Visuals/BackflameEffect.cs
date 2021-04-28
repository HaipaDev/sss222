using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackflameEffect : MonoBehaviour{
    [SerializeField] public GameObject part;
    //[SerializeField] bool twice=true;
    [SerializeField] public float xx = 0f;
    [SerializeField] public float yy = -0.6f;
    [SerializeField] float time = 0.3f;
    [SerializeField] bool onTop=true;
    [SerializeField] bool stayStill=true;
    [SerializeField] float angle;
    [SerializeField] public GameObject BFlame;
    /*void Start()
    {
        xx = transform.position.x + xx;
        yy = transform.position.y + yy;
    }*/
    // Update is called once per frame
    void Update(){
        //if(Time.timeScale>0.0001f){
            var xxx=transform.position.x+xx;
            var yyy=transform.position.y+yy;
            float zz=0f;
            if(BFlame==null){
            if(onTop==true){zz=transform.position.z-0.01f;}
            if(part!=null){BFlame=Instantiate(part,new Vector3(xxx,yyy,zz),Quaternion.identity,transform);}else{Debug.LogWarning("No particle attached to BackflameEffect of "+gameObject.name);}
            if(stayStill==true){BFlame.transform.eulerAngles=Vector3.zero;}
            else{BFlame.transform.eulerAngles=new Vector3(0,0,angle);}
            BFlame.GetComponent<ParticleSystem>().Play();
            }
            //Destroy(BFlame, time);
            //if (twice == true) { GameObject BFlame2 = Instantiate(part, new Vector3(xxx, yyy, transform.position.z - 0.01f), Quaternion.identity); Destroy(BFlame2, time); }
        //}
    }
    public void ClearBFlame(){Destroy(BFlame);BFlame=null;}
}
