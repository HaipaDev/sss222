using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailVFX : MonoBehaviour{
    
    [Header("Config")]
    [SerializeField] public string part="TrailDef";
    [SerializeField] public Vector2 offset=new Vector2(0f,0.33f);
    [SerializeField] bool onTop=false;
    [SerializeField] public bool cstmzTrail=false;
    [Header("Vars")]
    public GameObject trailObj;
    void Update(){
        var xx=transform.position.x+offset.x;
        var yy=transform.position.y+offset.y;
        float zz=transform.position.z+0.01f;
        if(onTop==true){zz=transform.position.z-0.01f;}
        if(trailObj==null){
            if(!System.String.IsNullOrEmpty(part)){
                if(!cstmzTrail){if(GameAssets.instance.GetVFX(part)!=null){
                    trailObj=Instantiate(GameAssets.instance.GetVFX(part),new Vector3(xx,yy,zz),Quaternion.identity,transform);
                    if(SaveSerial.instance.settingsData.particles||_exceptions()){trailObj.GetComponent<ParticleSystem>().Play();}
                }}else{if(GameAssets.instance.GetTrail(part)!=null){
                    trailObj=Instantiate(GameAssets.instance.GetTrail(part).part,new Vector3(xx,yy,zz),Quaternion.identity,transform);
                    if(SaveSerial.instance.settingsData.particles||_exceptions()){trailObj.GetComponent<ParticleSystem>().Play();}
                }}
            }else{Debug.LogWarning("No particle name set for TrailVFX of "+gameObject.name);}
        }
        
        if(trailObj!=null){
            if(!SaveSerial.instance.settingsData.particles&&!_exceptions()&&trailObj.GetComponent<ParticleSystem>().isPlaying){trailObj.GetComponent<ParticleSystem>().Stop();}
            if((SaveSerial.instance.settingsData.particles||_exceptions())&&trailObj.GetComponent<ParticleSystem>().isStopped){trailObj.GetComponent<ParticleSystem>().Play();}
        }
    }
    public void ClearTrail(){Destroy(trailObj);trailObj=null;}
    public void SetNewTrail(string str,bool _cstmzTrail=false){if(part!=str)ClearTrail();part=str;cstmzTrail=_cstmzTrail;}

    bool _exceptions(){/*if(trailObj.GetComponent<DamageParticle>()!=null
    ||trailObj.name.Contains(GameAssets.instance.GetVFX("trailObjDMG").name)
    ||trailObj.name.Contains(GameAssets.instance.GetVFX("trailObjDMG_Blue").name)
    )return true;*/
    /*else */if(SaveSerial.instance.settingsData.quality==0)return false;else return true;}
}
