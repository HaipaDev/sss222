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
    Player player;
    PlayerSkills pskills;
    UpgradeMenu upgradeMenu;
    Shop shopMenu;
    [SerializeField] public string value = "shopOn";
    [SerializeField] bool changeOnValidate;

    // Start is called before the first frame update
    void Start(){
        toggle = GetComponent<Toggle>();
        img = GetComponent<Image>();
        gameSession = FindObjectOfType<GameSession>();
        saveSerial = FindObjectOfType<SaveSerial>();
        //dataSavable = FindObjectOfType<DataSavable>();
        player = FindObjectOfType<Player>();
        pskills = FindObjectOfType<PlayerSkills>();
        upgradeMenu = FindObjectOfType<UpgradeMenu>();
        shopMenu = FindObjectOfType<Shop>();
    }

    // Update is called once per frame
    void Update(){
        ChangeText();
    }
    /*private void OnValidate() {
        player = FindObjectOfType<Player>();
        if(changeOnValidate)ChangeText();
    }*/

    void ChangeText(){
        if (value == "shopOn") toggle.isOn = GameRules.instance.shopOn;
        if (value == "shopCargoOn") toggle.isOn = GameRules.instance.shopCargoOn;
        if (value == "upgradesOn") toggle.isOn = GameRules.instance.upgradesOn;
        if (value == "xpOn") toggle.isOn = GameRules.instance.xpOn;
        if (value == "barrierOn") toggle.isOn = GameRules.instance.barrierOn;
        if (value == "moveX") toggle.isOn = GameRules.instance.moveX;
        if (value == "moveY") toggle.isOn = GameRules.instance.moveY;
        if (value == "moveAxis") {if(GameRules.instance.moveX&&GameRules.instance.moveY){img.sprite=GetComponent<SpritesLib>().sprites[0];}else if(GameRules.instance.moveX&&!GameRules.instance.moveY){img.sprite=GetComponent<SpritesLib>().sprites[1];}else if(!GameRules.instance.moveX&&GameRules.instance.moveY){img.sprite=GetComponent<SpritesLib>().sprites[2];}}
    }
}
