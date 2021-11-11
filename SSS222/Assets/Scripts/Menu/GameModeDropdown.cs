using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
using System.Linq;

public class GameModeDropdown : MonoBehaviour{
    [SerializeField]List<string> skip=new List<string>(0);
    TMP_Dropdown dd;
    void Start(){
        dd=GetComponent<TMP_Dropdown>();
        
        List<OptionData> options=new List<OptionData>();
        for(var i=0;i<GameCreator.instance.gamerulesetsPrefabs.Length;i++){
            foreach(string s in skip)if(!GameCreator.instance.gamerulesetsPrefabs[i].cfgName.Contains(s)){
                Debug.Log(GameCreator.instance.gamerulesetsPrefabs[i].cfgName);
                options.Add(new OptionData(GameCreator.instance.gamerulesetsPrefabs[i].cfgName,dd.itemImage.sprite));
            }
        }
        dd.ClearOptions();
        dd.AddOptions(options);
        dd.value=dd.options.FindIndex(d=>d.text.Contains(GameSession.instance.GetCurrentGameModeName()));//GameSession.instance.GetGameModeID(dd.options[dd.value].text);
    }
    public void SetGamemode(){GameSession.instance.SetGameModeSelectedStr(dd.options[dd.value].text);FindObjectOfType<DisplayLeaderboard>().DisplayCurrentUserHighscore();}
}