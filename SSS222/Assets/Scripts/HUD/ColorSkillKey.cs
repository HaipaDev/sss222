using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSkillKey : MonoBehaviour{
    [SerializeField] public int ID;
    [SerializeField] public skillKeyBind key;
    [SerializeField]Skill[] skills;
    [SerializeField]skillKeyBind[] skillsBinds;
    Color color;
    Image img;
    TMPro.TextMeshProUGUI txt;
    public bool on;
    void Start(){
        img=GetComponent<Image>();
        txt=GetComponentInChildren<TMPro.TextMeshProUGUI>();
        StartCoroutine(SetSkills());
    }
    private void OnEnable() {
        StartCoroutine(SetSkills());
    }
    IEnumerator SetSkills(){
        yield return new WaitForSecondsRealtime(0.21f);
        skills=FindObjectOfType<PlayerSkills>().skills;
        skillsBinds=FindObjectOfType<PlayerSkills>().skillsBinds;
    }

    void Update(){
        if(ID>0&&ID<skillsBinds.Length)if(skillsBinds[ID]==key){color=Color.green;on=true;}
        else{color=Color.white;on=false;}
        img.color=color;
        txt.color=color;
    }
}
