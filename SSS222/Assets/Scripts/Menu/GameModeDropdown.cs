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
    bool valueChanged;
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
    }
    public void SetGamemode(){
        //if(!valueChanged){dd.value=GameSession.instance.gameModeSelected+startAt;dd.RefreshShownValue();valueChanged=true;return;}
        //else{
            GameSession.instance.gameModeSelected=dd.value;
            //}
    }
}