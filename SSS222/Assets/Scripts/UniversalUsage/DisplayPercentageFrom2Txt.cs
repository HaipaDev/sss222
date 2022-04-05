using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayPercentageFrom2Txt : MonoBehaviour{
    [SerializeField]GameObject go1;
    [SerializeField]GameObject go2;
    [SerializeField]bool percentSymbol=true;
    [Sirenix.OdinInspector.DisableInPlayMode][SerializeField] bool onlyOnEnable=true;
    
    void Start(){if(onlyOnEnable)ChangeText();}
    void OnEnable(){if(onlyOnEnable)ChangeText();}
    void Update(){if(!onlyOnEnable)ChangeText();}
    void ChangeText(){
        float val1=0,val2=0,percent=0;
        if(go1.GetComponent<TextMeshProUGUI>()!=null){val1=float.Parse(go1.GetComponent<TextMeshProUGUI>().text);}
        if(go1.GetComponent<TMP_InputField>()!=null){val1=float.Parse(go1.GetComponent<TMP_InputField>().text);}
        if(go2.GetComponent<TextMeshProUGUI>()!=null){val2=float.Parse(go2.GetComponent<TextMeshProUGUI>().text);}
        if(go2.GetComponent<TMP_InputField>()!=null){val2=float.Parse(go2.GetComponent<TMP_InputField>().text);}
        percent=(float)System.Math.Round((val1/val2*100),2);
        if(!percentSymbol){GetComponent<TextMeshProUGUI>().text=percent.ToString();}
        else{GetComponent<TextMeshProUGUI>().text=percent.ToString()+"%";}
    }
}
