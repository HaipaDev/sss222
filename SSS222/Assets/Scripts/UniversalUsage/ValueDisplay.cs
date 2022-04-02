using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class ValueDisplay : MonoBehaviour{
    [SerializeField] public string value="score";
    [DisableInPlayMode][SerializeField] bool onlyOnEnable=false;
    TextMeshProUGUI txt;
    TMP_InputField tmpInput;
    void Start(){
        if(GetComponent<TextMeshProUGUI>()!=null)txt=GetComponent<TextMeshProUGUI>();
        if(GetComponent<TMP_InputField>()!=null)tmpInput=GetComponent<TMP_InputField>();
        if(onlyOnEnable)ChangeText();
    }
    void OnEnable(){if(onlyOnEnable)ChangeText();}
    void Update(){if(!onlyOnEnable)ChangeText();}

    void ChangeText(){      string _txt="";
    #region//GameSession
        if(GameSession.instance!=null){
            if(value=="score") _txt=GameSession.instance.score.ToString();
            else if(value=="coins") _txt=GameSession.instance.coins.ToString();
            else if(value=="cores") _txt=GameSession.instance.cores.ToString();
            else if(value.Contains("highscore")) _txt=GameSession.instance.GetHighscoreCurrent().ToString();
            else if(value=="gameSpeed") _txt="v"+GameSession.instance.gameSpeed;
            else if(value=="gameVersion") _txt=GameSession.instance.gameVersion;
            else if(value=="timePlayed") _txt=GameSession.instance.GetGameSessionTimeFormat();
            else if(value=="scoreMulti") _txt=GameSession.instance.scoreMulti.ToString();
            else if(value=="luck") _txt=GameSession.instance.luckMulti.ToString();

            else if(value=="cfgNameCurrent")_txt=GameSession.instance.GetGameRulesCurrent().cfgName;
        }
    #endregion
    #region//Player
        if(Player.instance!=null){
            if(value=="hpOffMax"){
                if(GameSession.instance.CheckGamemodeSelected("Classic")||Player.instance.health<=5){
                    _txt=System.Math.Round(Player.instance.health,1).ToString()+"/"+Player.instance.healthMax.ToString();}//Round to .1
                else _txt=Mathf.RoundToInt(Player.instance.health).ToString()+"/"+Player.instance.healthMax.ToString();
            }
            else if(value=="energyOffMax") _txt=Mathf.RoundToInt(Player.instance.energy).ToString()+"/"+Player.instance.energyMax.ToString();
            else if(value=="max_hp") _txt=Player.instance.healthMax.ToString();
            else if(value=="max_energy") _txt=Player.instance.energyMax.ToString();
            else if(value=="speed") _txt=(Player.instance.moveSpeed).ToString();
            else if(value=="hpRegen") if(Player.instance.hpRegenEnabled==true){_txt=Player.instance.hpRegenAmnt.ToString();}else{_txt="0";}
            else if(value=="enRegen") if(Player.instance.enRegenEnabled==true){_txt=Player.instance.enRegenAmnt.ToString();}else{_txt="0";}


            PlayerSkills pskills=Player.instance.GetComponent<PlayerSkills>();
            if(pskills!=null){
                if(value=="cooldownQ") _txt=System.Math.Round(pskills.cooldownQ,0).ToString();
                else if(value=="cooldownE") _txt=System.Math.Round(pskills.cooldownE,0).ToString();
                else if(value=="timerTeleport"){
                    if(FindObjectOfType<PlayerSkills>()!=null){_txt=System.Math.Round(pskills.timerTeleport,1).ToString();}else{Destroy(transform.parent.gameObject);}
                    if(_txt=="-4"){var tempColor=txt.color;tempColor.a=0;txt.color=tempColor;}
                }
            }
        }
    #endregion
    #region//Shop
        if(Shop.instance!=null){
            if(value=="purchases") _txt="Reputation: "+Shop.instance.purchases.ToString();
            else if(value=="reputation") _txt="Reputation: "+Shop.instance.reputation.ToString();
        }
        if(UpgradeMenu.instance!=null){
            if(value=="lvl_ship") _txt="Ship Level: "+UpgradeMenu.instance.total_UpgradesLvl.ToString();
            else if(value=="lvlPopup") _txt="Lvl up! ("+UpgradeMenu.instance.total_UpgradesLvl.ToString()+")";

            else if(value=="lvl_hp") _txt="Lvl. "+UpgradeMenu.instance.healthMax_UpgradesLvl.ToString();
            else if(value=="lvl_energy") _txt="Lvl. "+UpgradeMenu.instance.energyMax_UpgradesLvl.ToString();
            else if(value=="lvl_speed") _txt="Lvl. "+UpgradeMenu.instance.speed_UpgradesLvl.ToString();
            else if(value=="lvl_luck") _txt="Lvl. "+UpgradeMenu.instance.luck_UpgradesLvl.ToString();

            else if(value=="healthMax_upgradeCost") _txt=UpgradeMenu.instance.healthMax_UpgradeCost.ToString();
            else if(value=="energyMax_upgradeCost") _txt=UpgradeMenu.instance.energyMax_UpgradeCost.ToString();
            else if(value=="speed_upgradeCost") _txt=UpgradeMenu.instance.speed_UpgradeCost.ToString();
            else if(value=="luck_upgradeCost") _txt=UpgradeMenu.instance.luck_UpgradeCost.ToString();
            else if(value=="defaultPowerup_upgradeCost1") _txt=UpgradeMenu.instance.defaultPowerup_upgradeCost1.ToString();
            else if(value=="defaultPowerup_upgradeCost2") _txt=UpgradeMenu.instance.defaultPowerup_upgradeCost2.ToString();
            else if(value=="defaultPowerup_upgradeCost3") _txt=UpgradeMenu.instance.defaultPowerup_upgradeCost3.ToString();
            //else if(value=="energyRefill_upgradeCost") _txt=UpgradeMenu.instance.energyRefill_upgradeCost.ToString();
            else if(value=="mPulse_upgradeCost") _txt=UpgradeMenu.instance.mPulse_upgradeCost.ToString();
            else if(value=="postMortem_upgradeCost") _txt=UpgradeMenu.instance.postMortem_upgradeCost.ToString();
            else if(value=="teleport_upgradeCost") _txt=UpgradeMenu.instance.teleport_upgradeCost.ToString();
            else if(value=="overhaul_upgradeCost") _txt=UpgradeMenu.instance.overhaul_upgradeCost.ToString();
            else if(value=="crMend_upgradeCost") _txt=UpgradeMenu.instance.crMend_upgradeCost.ToString();
            else if(value=="enDiss_upgradeCost") _txt=UpgradeMenu.instance.enDiss_upgradeCost.ToString();
        }
    #endregion
    #region//SaveSerial
        if(SaveSerial.instance!=null){
            if(value=="inputType"){_txt=SaveSerial.instance.settingsData.inputType.ToString();}
            else if(value=="joystickType"){_txt=SaveSerial.instance.settingsData.joystickType.ToString();}
            else if(value=="joystickSize"){_txt=System.Math.Round(SaveSerial.instance.settingsData.joystickSize,2).ToString();}
            else if(value=="loginUsername"){_txt=SaveSerial.instance.hyperGamerLoginData.username.ToString();}
        }
    #endregion
    #region//DBAccess
        if(DBAccess.instance!=null){
            if(value=="loginMessage"){_txt=DBAccess.instance.loginMessage;}
            else if(value=="submitMessage"){_txt=DBAccess.instance.submitMessage;}
        }
    #endregion
    #region//GameRules
        if(GameRules.instance!=null){
            EnemyClass _en=null;if(value.Contains("EnemySB")&&SandboxCanvas.instance!=null){if(!String.IsNullOrEmpty(SandboxCanvas.instance.enemyToModify))_en=Array.Find(GameRules.instance.enemies,x=>x.name==SandboxCanvas.instance.enemyToModify);}
            if(value=="cfgName") _txt=GameRules.instance.cfgName;
            else if(value=="gameSpeedGR") _txt=GameRules.instance.defaultGameSpeed.ToString();
            else if(value=="presetName"&&SandboxCanvas.instance!=null) _txt="PRESET FROM: "+SandboxCanvas.instance.presetGameruleset.cfgName;


            else if(value=="waveScoreRangeGR"){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;var ss=sr.scoreMaxSetRange;
                                                if(ss.x!=ss.y){_txt=ss.x.ToString()+"-"+ss.y.ToString();}
                                                else _txt=ss.x.ToString();}else _txt="?";
            }
            else if(value=="waveScoreRangeStartGR") if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;var ss=sr.scoreMaxSetRange;_txt=ss.x.ToString();}else _txt="?";
            else if(value=="waveScoreRangeEndGR") if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;var ss=sr.scoreMaxSetRange;_txt=ss.y.ToString();}else _txt="?";
            else if(value=="shopScoreRangeGR"){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;var ss=sr.scoreMaxSetRange;
                                                if(ss.x!=ss.y){_txt=ss.x.ToString()+"-"+ss.y.ToString();}
                                                else _txt=ss.x.ToString();}else _txt="?";
            }
            else if(value=="shopScoreRangeStartGR") if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;var ss=sr.scoreMaxSetRange;_txt=ss.x.ToString();}else _txt="?";
            else if(value=="shopScoreRangeEndGR") if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;var ss=sr.scoreMaxSetRange;_txt=ss.y.ToString();}else _txt="?";

            else if(value=="healthStartingPlayerGR") _txt=GameRules.instance.healthPlayer.ToString();
            else if(value=="healthMaxPlayerGR") _txt=GameRules.instance.healthMaxPlayer.ToString();
            else if(value=="energyPlayerGR") _txt=GameRules.instance.energyPlayer.ToString()+"/"+GameRules.instance.energyMaxPlayer.ToString();
            else if(value=="energyStartingPlayerGR") _txt=GameRules.instance.energyPlayer.ToString();
            else if(value=="energyMaxPlayerGR") _txt=GameRules.instance.energyMaxPlayer.ToString();
            else if(value=="speedPlayerGR") _txt=GameRules.instance.moveSpeedPlayer.ToString();

            else if(value=="powerupsCapacity") _txt=GameRules.instance.powerupsCapacity.ToString();


            else if(value=="nameEnemySB") _txt=_en.name.ToString();
            else if(value=="healthEnemySB") _txt=_en.healthStart.ToString();
            else if(value=="healthMaxEnemySB") _txt=_en.healthMax.ToString();
            else if(value=="scoreStartEnemySB") _txt=_en.scoreValue.x.ToString();
            else if(value=="scoreEndEnemySB") _txt=_en.scoreValue.y.ToString();
        }
    #endregion
        
        if(txt!=null)txt.text=_txt;
        else{if(tmpInput!=null){if(UIInputSystem.instance!=null)if(UIInputSystem.instance.currentSelected!=tmpInput.gameObject){tmpInput.text=_txt;}
        foreach(TextMeshProUGUI t in GetComponentsInChildren<TextMeshProUGUI>()){t.text=_txt;}}}
    }
}
