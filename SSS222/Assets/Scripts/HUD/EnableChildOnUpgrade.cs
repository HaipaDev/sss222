using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableChildOnUpgrade : MonoBehaviour{
    [SerializeField] public string valueName;
    [SerializeField] public int valueReq=1;
    public int value;
    Image image;
    UpgradeMenu upgradeMenu;
    void Start(){
        image=GetComponent<Image>();
        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        //valueReq=
    }

    void Update(){
        value=(int)upgradeMenu.GetType().GetField(valueName).GetValue(upgradeMenu);
        if(value>=valueReq){
            SetActiveAllChildren(transform,true);
        }else{SetActiveAllChildren(transform,false);}
    }

    private void SetActiveAllChildren(Transform transform, bool value)
     {
         foreach (Transform child in transform)
         {
             child.gameObject.SetActive(value);
 
             SetActiveAllChildren(child, value);
         }
     }
}
