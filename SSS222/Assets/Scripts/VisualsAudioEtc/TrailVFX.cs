using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TrailVFX : MonoBehaviour{
    
    [Header("Config")]
    [SerializeField] public string part="TrailDef";
    [SerializeField] public Vector2 offset=new Vector2(0f,0.33f);
    [SerializeField] bool onTop=false;
    [DisableInEditorMode][SerializeField] public bool cstmzTrail=false;
    [Header("Vars")]
    public GameObject trailObj;
    void Update(){
        var xx=transform.position.x+offset.x;
        var yy=transform.position.y+offset.y;
        float zz=transform.position.z+0.01f;
        if(onTop==true){zz=transform.position.z-0.01f;}
        if(trailObj==null){
            if(!System.String.IsNullOrEmpty(part)){
                if(!cstmzTrail){if(AssetsManager.instance.GetVFX(part)!=null){
                    if(trailObj==null){trailObj=Instantiate(AssetsManager.instance.GetVFX(part),new Vector3(xx,yy,zz),Quaternion.identity,transform);}//Debug.Log("Creating particle for: "+gameObject.name);}
                    if(SaveSerial.instance.settingsData.particles){trailObj.GetComponent<ParticleSystem>().Play();}//Debug.Log("Playing particle first time for: "+gameObject.name);}
                }}else{if(AssetsManager.instance.GetTrail(part)!=null){
                    if(trailObj==null){trailObj=Instantiate(AssetsManager.instance.GetTrail(part).part,new Vector3(xx,yy,zz),Quaternion.identity,transform);}//Debug.Log("Creating particle for: "+gameObject.name);}
                    if(SaveSerial.instance.settingsData.particles){trailObj.GetComponent<ParticleSystem>().Play();}//Debug.Log("Playing particle first time for: "+gameObject.name);}
                }}
            }else{Debug.LogWarning("No particle name set for TrailVFX of "+gameObject.name);}
        }
        
        if(trailObj!=null){
            if(!SaveSerial.instance.settingsData.particles&&!_exceptions()&&trailObj.GetComponent<ParticleSystem>().isPlaying){trailObj.GetComponent<ParticleSystem>().Stop();Debug.Log("Stopping particle for: "+gameObject.name);}
            if((SaveSerial.instance.settingsData.particles)&&trailObj.GetComponent<ParticleSystem>().isStopped){trailObj.GetComponent<ParticleSystem>().Play();Debug.Log("Playing particle for: "+gameObject.name);}
        }
    }
    public void ClearTrail(){Destroy(trailObj);trailObj=null;}
    public void SetNewTrail(string str,bool _cstmzTrail=false){if(part!=str){ClearTrail();part=str;cstmzTrail=_cstmzTrail;}}//Debug.Log("Setting new Trail for: "+gameObject.name);}}

    bool _exceptions(){/*if(trailObj.GetComponent<DamageParticle>()!=null
    ||trailObj.name.Contains(AssetsManager.instance.GetVFX("trailObjDMG").name)
    ||trailObj.name.Contains(AssetsManager.instance.GetVFX("trailObjDMG_Blue").name)
    )return true;*/
    /*else */if(SaveSerial.instance.settingsData.quality==0)return false;else return true;}
}
