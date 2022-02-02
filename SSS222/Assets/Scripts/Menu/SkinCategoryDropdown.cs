using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static TMPro.TMP_Dropdown;
using System.Linq;

public class SkinCategoryDropdown : MonoBehaviour{
    [SerializeField]List<string> skip=new List<string>(0);
    TMP_Dropdown dd;
    void Start(){
        dd=GetComponent<TMP_Dropdown>();
        
        List<OptionData> options=new List<OptionData>();
        for(var i=0;i<CustomizationInventory.instance._CstmzCategoryNames.Length;i++){
            if(skip.Count==0){
                options.Add(new OptionData(CustomizationInventory.instance._CstmzCategoryNames[i],dd.itemImage.sprite));
            }else{for(var j=0;j<skip.Count;j++){if(CustomizationInventory.instance._CstmzCategoryNames[i].Contains(skip[j])){
                    options.Add(new OptionData(CustomizationInventory.instance._CstmzCategoryNames[i],dd.itemImage.sprite));
            }}}
        }
        dd.ClearOptions();
        dd.AddOptions(options);
        dd.value=dd.options.FindIndex(d=>d.text.Contains(CustomizationInventory.instance._CstmzCategoryNames[(int)CustomizationInventory.instance.categorySelected]));
    }
    public void SetCategory(){CustomizationInventory.instance.ChangeCategory(dd.options[dd.value].text);}
}