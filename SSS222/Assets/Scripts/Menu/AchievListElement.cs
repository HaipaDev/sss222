using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AchievListElement : MonoBehaviour{
    [SerializeField] new TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Image icon;
    [SerializeField] bool epic;
    [DisableInEditorMode] public bool completed;
    void Update(){
        var mngr=StatsAchievsManager.instance;
        if(!completed&&!epic){GetComponent<Image>().color=mngr.uncompletedColor;}
        else if(completed&&!epic){GetComponent<Image>().color=mngr.completedColor;}
        else if(!completed&&epic){GetComponent<Image>().color=mngr.epicUncompletedColor;}
        else if(completed&&epic){GetComponent<Image>().color=mngr.epicCompletedColor;}
    }

    public void SetName(string str){name.text=str;gameObject.name=str;}
    public void SetDesc(string str){desc.text=str;}
    public void SetIcon(Sprite spr){icon.sprite=spr;}
    public void SetEpic(bool b){epic=b;}
}
