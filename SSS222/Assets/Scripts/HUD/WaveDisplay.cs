using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI waveText;
    TMPro.TextMeshProUGUI curwaveObjText;
    GameSession gameSession;
    Waves waves;
    public float timer=2f;
    public float showTime = 5f;
    public bool enableText=false;
    [SerializeField]GameObject curwaveObj;

    void Start(){
        //curwaveObj = GameObject.Find("CurrentWave");
        curwaveObjText = curwaveObj.GetComponent<TMPro.TextMeshProUGUI>();
        waveText = GetComponent<TMPro.TextMeshProUGUI>();
        waves = FindObjectOfType<Waves>();
        gameSession = FindObjectOfType<GameSession>();
        enableText=true;
    }

    void Update(){
        if(timer>0)timer-=Time.deltaTime;

        
        if(waveText.text!=null&&waveText.text!=""){if(timer<=0){waveText.enabled=false;curwaveObjText.enabled=false;}}
        else{waveText.enabled=false;curwaveObjText.enabled=false;}
        if(enableText==true){waveText.enabled=true;curwaveObjText.enabled=true;timer=showTime;enableText=false;}
        if(waveText!=null&&waves.currentWave!=null){
            waveText.text=
            waves.GetWaveName();
        }
        //Debug.Log(timer);
    }
}
