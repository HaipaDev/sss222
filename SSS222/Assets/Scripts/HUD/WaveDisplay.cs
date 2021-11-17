using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI waveText;
    TMPro.TextMeshProUGUI curwaveObjText;
    Waves waves;
    public float timer=2f;
    public float showTime = 5f;
    public bool enableText=false;
    [SerializeField]GameObject curwaveObj;

    void Start(){
        curwaveObjText=curwaveObj.GetComponent<TMPro.TextMeshProUGUI>();
        waveText=GetComponent<TMPro.TextMeshProUGUI>();
        waves=FindObjectOfType<Waves>();
        enableText=true;
    }

    void Update(){
        if(waves==null)waves=FindObjectOfType<Waves>();
        if(timer>0)timer-=Time.deltaTime;
        
        if(waveText.text!=null&&waveText.text!=""){if(timer<=0){waveText.enabled=false;curwaveObjText.enabled=false;}}
        else{waveText.enabled=false;curwaveObjText.enabled=false;}
        if(enableText==true){waveText.enabled=true;curwaveObjText.enabled=true;timer=showTime;enableText=false;}
        if(waves!=null){
            if(waveText!=null&&waves.currentWave!=null){
                waveText.text=
                waves.GetWaveName();
            }
        }
        //Debug.Log(timer);
    }
}
