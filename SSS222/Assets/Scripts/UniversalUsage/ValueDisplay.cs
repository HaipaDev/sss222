using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI txt;
    PlayerSkills pskills;
    [SerializeField] public string value="score";
    //[SerializeField] float valueLimitD=-1;
    [SerializeField] bool changeOnValidate;

    void Start(){
        txt=GetComponent<TMPro.TextMeshProUGUI>();
        pskills=FindObjectOfType<PlayerSkills>();
    }
    void Update(){ChangeText();}

    void ChangeText(){
    if(GameSession.instance!=null&&GameSession.instance!=null){
        if(value=="score") txt.text=GameSession.instance.score.ToString();
        //else if(value=="evscore") txt.text=GameSession.instance.EVscore.ToString();
        else if(value=="coins") txt.text=GameSession.instance.coins.ToString();
        else if(value=="cores") txt.text=GameSession.instance.cores.ToString();
        else if(value.Contains("highscore")) txt.text=GameSession.instance.GetHighscore(/*int.Parse(value.Split('e')[1])*/GameSession.instance.gameModeSelected).ToString();
        else if(value=="gameVersion") txt.text=GameSession.instance.gameVersion;
        else if(value=="timePlayed") txt.text=GameSession.instance.GetGameSessionTimeFormat();
    }
    if(Player.instance!=null){
        if(value=="hpOffMax"){
            if(GameSession.instance.CheckGameModeSelected("Classic")||Player.instance.health<=5){
                txt.text=System.Math.Round(Player.instance.health,1).ToString()+"/"+Player.instance.healthMax.ToString();}//Round to .1
            else txt.text=Mathf.RoundToInt(Player.instance.health).ToString()+"/"+Player.instance.healthMax.ToString();
    }
        else if(value=="energyOffMax") txt.text=Mathf.RoundToInt(Player.instance.energy).ToString()+"/"+Player.instance.energyMax.ToString();
        else if(value=="max_hp") txt.text=Player.instance.healthMax.ToString();
        else if(value=="max_energy") txt.text=Player.instance.energyMax.ToString();
        else if(value=="speed") txt.text=(Player.instance.moveSpeed).ToString();
        else if(value=="hpRegen") if(Player.instance.hpRegenEnabled==true){txt.text=Player.instance.hpRegenAmnt.ToString();}else{txt.text="0";}
        else if(value=="enRegen") if(Player.instance.enRegenEnabled==true){txt.text=Player.instance.enRegenAmnt.ToString();}else{txt.text="0";}
        }
    if(pskills!=null){
        if(value=="cooldownQ") txt.text=System.Math.Round(pskills.cooldownQ,0).ToString();
        else if(value=="cooldownE") txt.text=System.Math.Round(pskills.cooldownE,0).ToString();
        else if(value=="timerTeleport")if(FindObjectOfType<PlayerSkills>()!=null){txt.text=System.Math.Round(pskills.timerTeleport,1).ToString();}else{Destroy(transform.parent.gameObject);}
    }
    if(GameSession.instance!=null){
        if(value=="scoreMulti") txt.text=GameSession.instance.scoreMulti.ToString();
        else if(value=="luck") txt.text=GameSession.instance.luckMulti.ToString();
    }
    if(Shop.instance!=null){
        if(value=="purchases") txt.text="Reputation: "+Shop.instance.purchases.ToString();
        else if(value=="reputation") txt.text="Reputation: "+Shop.instance.reputation.ToString();
    }
    if(UpgradeMenu.instance!=null){
        if(value=="lvl_ship") txt.text="Ship Level: "+UpgradeMenu.instance.total_UpgradesLvl.ToString();
        else if(value=="lvl_hp") txt.text="Lvl. "+UpgradeMenu.instance.healthMax_UpgradesLvl.ToString();
        else if(value=="lvl_energy") txt.text="Lvl. "+UpgradeMenu.instance.energyMax_UpgradesLvl.ToString();
        else if(value=="lvl_speed") txt.text="Lvl. "+UpgradeMenu.instance.speed_UpgradesLvl.ToString();
        else if(value=="lvl_luck") txt.text="Lvl. "+UpgradeMenu.instance.luck_UpgradesLvl.ToString();

        else if(value=="healthMax_upgradeCost") txt.text=UpgradeMenu.instance.healthMax_UpgradeCost.ToString();
        else if(value=="energyMax_upgradeCost") txt.text=UpgradeMenu.instance.energyMax_UpgradeCost.ToString();
        else if(value=="speed_upgradeCost") txt.text=UpgradeMenu.instance.speed_UpgradeCost.ToString();
        else if(value=="luck_upgradeCost") txt.text=UpgradeMenu.instance.luck_UpgradeCost.ToString();
        else if(value=="defaultPowerup_upgradeCost1") txt.text=UpgradeMenu.instance.defaultPowerup_upgradeCost1.ToString();
        else if(value=="defaultPowerup_upgradeCost2") txt.text=UpgradeMenu.instance.defaultPowerup_upgradeCost2.ToString();
        else if(value=="defaultPowerup_upgradeCost3") txt.text=UpgradeMenu.instance.defaultPowerup_upgradeCost3.ToString();
        //else if(value=="energyRefill_upgradeCost") txt.text=UpgradeMenu.instance.energyRefill_upgradeCost.ToString();
        else if(value=="mPulse_upgradeCost") txt.text=UpgradeMenu.instance.mPulse_upgradeCost.ToString();
        else if(value=="postMortem_upgradeCost") txt.text=UpgradeMenu.instance.postMortem_upgradeCost.ToString();
        else if(value=="teleport_upgradeCost") txt.text=UpgradeMenu.instance.teleport_upgradeCost.ToString();
        else if(value=="overhaul_upgradeCost") txt.text=UpgradeMenu.instance.overhaul_upgradeCost.ToString();
        else if(value=="crMend_upgradeCost") txt.text=UpgradeMenu.instance.crMend_upgradeCost.ToString();
        else if(value=="enDiss_upgradeCost") txt.text=UpgradeMenu.instance.enDiss_upgradeCost.ToString();
    }
    if(GameRules.instance!=null){
        if(value=="cfgName")if(GameRules.instance!=null){txt.text=GameRules.instance.cfgName;}else{Debug.LogError("GameRules Not Present");}
        else if(value=="cfgNameCurrent")txt.text=GameCreator.instance.gamerulesetsPrefabs[GameSession.instance.gameModeSelected].cfgName;
        else if(value=="speedPlayerGR") txt.text=GameRules.instance.moveSpeedPlayer.ToString();
        else if(value=="healthPlayerGR") txt.text=GameRules.instance.healthPlayer.ToString();
        else if(value=="energyPlayerGR") txt.text=GameRules.instance.energyPlayer.ToString();
        else if(value=="waveScoreRangeGR") if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;var ss=sr.scoreMaxSetRange;
                                            if(ss.x!=ss.y){txt.text=ss.x.ToString()+"-"+ss.y.ToString();}
                                            else txt.text=ss.x.ToString();}else txt.text="?";
        else if(value=="shopScoreRangeGR") if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;var ss=sr.scoreMaxSetRange;
                                            if(ss.x!=ss.y){txt.text=ss.x.ToString()+"-"+ss.y.ToString();}
                                            else txt.text=ss.x.ToString();}else txt.text="?";
    }
    if(SaveSerial.instance!=null){
        if(value=="inputType"){txt.text=SaveSerial.instance.settingsData.inputType.ToString();}
        else if(value=="joystickType"){txt.text=SaveSerial.instance.settingsData.joystickType.ToString();}
        else if(value=="joystickSize"){txt.text=System.Math.Round(SaveSerial.instance.settingsData.joystickSize,2).ToString();}
        else if(value=="loginUsername"){txt.text=SaveSerial.instance.hyperGamerLoginData.username.ToString();}
    }
    if(DBAccess.instance!=null){
        if(value=="registerMessage"){txt.text=DBAccess.instance.registerMessage;}
        else if(value=="loginMessage"){txt.text=DBAccess.instance.loginMessage;}
        else if(value=="submitMessage"){txt.text=DBAccess.instance.submitMessage;}
    }
        
        /*else if(value=="state"){
            var value=System.Math.Round(Player.instance.GetGCloverTimer(),1);

            if(value <= valueLimitD){ value=0; }
            else { txt.text=value.ToString(); }
        }*/
    }
}
