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
    string str;
    GameObject curwaveObj;

    // Start is called before the first frame update
    void Start()
    {
        curwaveObj = GameObject.Find("CurrentWave");
        curwaveObjText = curwaveObj.GetComponent<TMPro.TextMeshProUGUI>();
        waveText = GetComponent<TMPro.TextMeshProUGUI>();
        waves = FindObjectOfType<Waves>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waves!=null)waveText.text = waves.GetWaveName().ToString();
        str = waveText.text;
        if(str!=null && str!=""){ if (timer <= 0) { waveText.enabled = false; curwaveObjText.enabled = false; } }
        else{ waveText.enabled = false; curwaveObjText.enabled = false; }

        if(enableText == true){ waveText.enabled = true; curwaveObjText.enabled = true; timer = showTime; enableText = false; }
        
        if(timer>0)timer -= Time.deltaTime;
        //Debug.Log(timer);
        //if (timer <= 0){ waveText.enabled = false;  timer = 2f; }
    }
}
