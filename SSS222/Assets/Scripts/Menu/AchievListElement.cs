using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AchievListElement : MonoBehaviour{
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Image icon;
    [DisableInEditorMode] public bool completed;
    void Update(){if(completed){GetComponent<Image>().color=Color.green;}}

    public void SetName(string str){name.text=str;gameObject.name=str;}
    public void SetDesc(string str){desc.text=str;}
    public void SetIcon(Sprite spr){icon.sprite=spr;}
}
