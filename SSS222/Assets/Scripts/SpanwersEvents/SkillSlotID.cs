using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum skillKeyBind{
    Disabled, 
    Q, 
    E
    };
[CreateAssetMenu(menuName = "Skill Slot ID")]
public class SkillSlotID : ScriptableObject{
    [HeaderAttribute("Properties")]
    [SerializeField] public string skillName = "";
    [SerializeField] public Sprite sprite;
    //[SerializeField] public string pwrupName = "";
    [SerializeField] public int ID;
    [SerializeField] public float enCost;
    [SerializeField] public float cooldown;
    //public skillKeyBind skillKeyBind;
    //[SerializeField] public skillKeyBind keySet=skillKeyBind.Disabled;
    //[SerializeField] public Sprite sprite;
}
