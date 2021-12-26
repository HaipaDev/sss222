using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackflameEffect : MonoBehaviour{
    [SerializeField] public GameObject part;
    //[SerializeField] bool twice=true;
    [SerializeField] public float xx = 0f;
    [SerializeField] public float yy = -0.6f;
    //[SerializeField] float time = 0.3f;
    [SerializeField] bool onTop=true;
    [SerializeField] bool stayStill=true;
    [SerializeField] float angle;
    [SerializeField] public GameObject BFlame;
    void Update(){
        var xxx=transform.position.x+xx;
        var yyy=transform.position.y+yy;
        float zz=0f;
        if(onTop==true){zz=transform.position.z-0.01f;}
        if(BFlame==null){
            if(part!=null){BFlame=Instantiate(part,new Vector3(xxx,yyy,zz),Quaternion.identity,transform);}else{Debug.LogWarning("No particle attached to BackflameEffect of "+gameObject.name);}
        }if(BFlame!=null){
            if(stayStill==true){BFlame.transform.eulerAngles=Vector3.zero;}
            else{BFlame.transform.eulerAngles=new Vector3(0,0,angle);}
            if(SaveSerial.instance.settingsData.particles||_exceptions())BFlame.GetComponent<ParticleSystem>().Play();
        }
        if(!SaveSerial.instance.settingsData.particles&&BFlame.GetComponent<ParticleSystem>().isPlaying&&!_exceptions()){BFlame.GetComponent<ParticleSystem>().Stop();}
        if(SaveSerial.instance.settingsData.particles&&BFlame.GetComponent<ParticleSystem>().isStopped){BFlame.GetComponent<ParticleSystem>().Play();}
    }
    public void ClearBFlame(){Destroy(BFlame);BFlame=null;}
    bool _exceptions(){if(BFlame.GetComponent<DamageParticle>()!=null
    ||BFlame.name.Contains(GameAssets.instance.GetVFX("BFlameDMG").name)
    ||BFlame.name.Contains(GameAssets.instance.GetVFX("BFlameDMG_Blue").name)
    )
    return true;
    else if(SaveSerial.instance.settingsData.quality==0)return false;else return false;}
}
