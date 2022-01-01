using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackflameEffect : MonoBehaviour{
    
    [Header("Config")]
    [SerializeField] public string part="BFlame";
    [SerializeField] public Vector2 offset=new Vector2(0f,0.33f);
    [SerializeField] bool onTop=false;
    [Header("Vars")]
    public GameObject BFlame;
    void Update(){
        var xx=transform.position.x+offset.x;
        var yy=transform.position.y+offset.y;
        float zz=transform.position.z+0.01f;
        if(onTop==true){zz=transform.position.z-0.01f;}
        if(BFlame==null){
            if(!System.String.IsNullOrEmpty(part)){
                if(GameAssets.instance.GetVFX(part)!=null){
                    BFlame=Instantiate(GameAssets.instance.GetVFX(part),new Vector3(xx,yy,zz),Quaternion.identity,transform);
                    if(SaveSerial.instance.settingsData.particles||_exceptions()){BFlame.GetComponent<ParticleSystem>().Play();}
                }
            }else{Debug.LogWarning("No particle attached to BackflameEffect of "+gameObject.name);}
        }
        
        if(BFlame!=null){
            if(!SaveSerial.instance.settingsData.particles&&!_exceptions()&&BFlame.GetComponent<ParticleSystem>().isPlaying){BFlame.GetComponent<ParticleSystem>().Stop();}
            if((SaveSerial.instance.settingsData.particles||_exceptions())&&BFlame.GetComponent<ParticleSystem>().isStopped){BFlame.GetComponent<ParticleSystem>().Play();}
        }
    }
    public void ClearBFlame(){Destroy(BFlame);BFlame=null;}

    bool _exceptions(){if(BFlame.GetComponent<DamageParticle>()!=null
    ||BFlame.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)
    ||BFlame.name.Contains(GameAssets.instance.GetVFX("BFlameDMG_Blue").name)
    )
    return true;
    else if(SaveSerial.instance.settingsData.quality==0)return false;else return false;}
}
