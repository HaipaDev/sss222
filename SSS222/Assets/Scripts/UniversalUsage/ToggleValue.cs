using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleValue : MonoBehaviour{
    Toggle toggle;
    Image img;
    GameSession gameSession;
    DataSavable dataSavable;
    SaveSerial saveSerial;
    PlayerSkills pskills;
    UpgradeMenu upgradeMenu;
    Shop shopMenu;
    [SerializeField] public string value="shopOn";
    [SerializeField] bool changeOnValidate;

    void Start(){
        toggle=GetComponent<Toggle>();
        img=GetComponent<Image>();
        gameSession=FindObjectOfType<GameSession>();
        saveSerial=FindObjectOfType<SaveSerial>();
        pskills=FindObjectOfType<PlayerSkills>();
        upgradeMenu=FindObjectOfType<UpgradeMenu>();
        shopMenu=FindObjectOfType<Shop>();
    }

    void Update(){ChangeText();}

    void ChangeText(){
        var i=GameRules.instance;
        if (value=="shopOn") toggle.isOn=i.shopOn;
        if (value=="shopCargoOn") toggle.isOn=i.shopCargoOn;
        if (value=="upgradesOn") toggle.isOn=i.upgradesOn;
        if (value=="xpOn") toggle.isOn=i.xpOn;
        if (value=="barrierOn") toggle.isOn=i.barrierOn;
        if (value=="moveX") toggle.isOn=i.moveX;
        if (value=="moveY") toggle.isOn=i.moveY;
        if (value=="moveAxis") {if(i.moveX&&i.moveY){img.sprite=GetComponent<SpritesLib>().sprites[0];}else if(i.moveX&&!i.moveY){img.sprite=GetComponent<SpritesLib>().sprites[1];}else if(!i.moveX&&i.moveY){img.sprite=GetComponent<SpritesLib>().sprites[2];}}
    }
}
