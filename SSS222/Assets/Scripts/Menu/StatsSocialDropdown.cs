using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
using System.Linq;

public class StatsSocialDropdown : MonoBehaviour{
    public string value="Total";
    [SerializeField]List<string> skip=new List<string>(0);
    TMP_Dropdown dd;
    IEnumerator Start(){
        dd=GetComponent<TMP_Dropdown>();
        
        yield return new WaitForSeconds(0.1f);
        List<OptionData> options=new List<OptionData>();
        for(var i=0;i<StatsAchievsManager.instance.statsGamemodesList.Count;i++){
            if(skip.Count==0){
                options.Add(new OptionData(StatsAchievsManager.instance.statsGamemodesList[i].gmName,dd.itemImage.sprite));
            }else{for(var j=0;j<skip.Count;j++){if(!StatsAchievsManager.instance.statsGamemodesList[i].gmName.Contains(skip[j])){
                    options.Add(new OptionData(StatsAchievsManager.instance.statsGamemodesList[i].gmName,dd.itemImage.sprite));
            }}}
        }
        //dd.ClearOptions();
        dd.AddOptions(options);
        dd.value=dd.options.FindIndex(d=>d.text==value);
    }
    public void SetValue(){value=dd.options[dd.value].text;}
}