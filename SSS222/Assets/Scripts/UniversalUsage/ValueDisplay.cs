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
    [HideInPlayMode][SerializeField] bool onValidate=false;
    TextMeshProUGUI txt;
    TMP_InputField tmpInput;
    void Start(){
        if(GetComponent<TextMeshProUGUI>()!=null)txt=GetComponent<TextMeshProUGUI>();
        if(GetComponent<TMP_InputField>()!=null)tmpInput=GetComponent<TMP_InputField>();
        if(onlyOnEnable)ChangeText();
    }
    void OnEnable(){if(onlyOnEnable)ChangeText();}
    void OnValidate(){if(onValidate)ChangeText();}
    void Update(){if(!onlyOnEnable)ChangeText();}

    void ChangeText(){      string _txt="";
    #region//GameSession
        if(GameSession.instance!=null){     var gs=GameSession.instance;
            if(value=="score") _txt=gs.score.ToString();
            else if(value=="coins") _txt=gs.coins.ToString();
            else if(value=="cores") _txt=gs.cores.ToString();
            else if(value.Contains("highscore")) _txt=gs.GetHighscoreCurrent().ToString();
            else if(value=="gameSpeed") _txt="v"+gs.gameSpeed;
            else if(value=="gameVersion") _txt=gs.gameVersion;
            else if(value=="timePlayed") _txt=gs.GetGameSessionTimeFormat();
            else if(value=="scoreMulti") _txt=gs.scoreMulti.ToString();
            else if(value=="luck") _txt=gs.luckMulti.ToString();

            else if(value=="cfgNameCurrent")_txt=gs.GetGameRulesCurrent().cfgName;
        }
    #endregion
    #region//Player
        if(Player.instance!=null){      var p=Player.instance;
            if(value=="hpOffMax"){
                if(GameSession.instance.CheckGamemodeSelected("Classic")||p.health<=5){
                    _txt=System.Math.Round(p.health,1).ToString()+"/"+p.healthMax.ToString();}//Round to .1
                else _txt=Mathf.RoundToInt(p.health).ToString()+"/"+p.healthMax.ToString();
            }
            else if(value=="energyOffMax") _txt=Mathf.RoundToInt(p.energy).ToString()+"/"+p.energyMax.ToString();
            else if(value=="max_hp") _txt=p.healthMax.ToString();
            else if(value=="max_energy") _txt=p.energyMax.ToString();
            else if(value=="speed") _txt=(p.moveSpeed).ToString();
            else if(value=="hpRegen") if(p.hpRegenEnabled==true){_txt=p.hpRegenAmnt.ToString();}else{_txt="0";}
            else if(value=="enRegen") if(p.enRegenEnabled==true){_txt=p.enRegenAmnt.ToString();}else{_txt="0";}


            PlayerSkills pskills=p.GetComponent<PlayerSkills>();
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
    #region//Shop & UpgradeMenu
        if(Shop.instance!=null){    var sh=Shop.instance;
            if(value=="purchases") _txt="Reputation: "+sh.purchases.ToString();
            else if(value=="reputation") _txt="Reputation: "+sh.reputation.ToString();
        }
        if(UpgradeMenu.instance!=null){     var u=UpgradeMenu.instance;
            if(value=="lvl_ship") _txt="Ship Level: "+u.total_UpgradesLvl.ToString();
            else if(value=="lvlPopup") _txt="Lvl up! ("+u.total_UpgradesLvl.ToString()+")";

            else if(value=="lvl_hp") _txt="Lvl. "+u.healthMax_UpgradesLvl.ToString();
            else if(value=="lvl_energy") _txt="Lvl. "+u.energyMax_UpgradesLvl.ToString();
            else if(value=="lvl_speed") _txt="Lvl. "+u.speed_UpgradesLvl.ToString();
            else if(value=="lvl_luck") _txt="Lvl. "+u.luck_UpgradesLvl.ToString();

            else if(value=="healthMax_upgradeCost") _txt=u.healthMax_UpgradeCost.ToString();
            else if(value=="energyMax_upgradeCost") _txt=u.energyMax_UpgradeCost.ToString();
            else if(value=="speed_upgradeCost") _txt=u.speed_UpgradeCost.ToString();
            else if(value=="luck_upgradeCost") _txt=u.luck_UpgradeCost.ToString();
            else if(value=="defaultPowerup_upgradeCost1") _txt=u.defaultPowerup_upgradeCost1.ToString();
            else if(value=="defaultPowerup_upgradeCost2") _txt=u.defaultPowerup_upgradeCost2.ToString();
            else if(value=="defaultPowerup_upgradeCost3") _txt=u.defaultPowerup_upgradeCost3.ToString();
            //else if(value=="energyRefill_upgradeCost") _txt=u.energyRefill_upgradeCost.ToString();
            else if(value=="mPulse_upgradeCost") _txt=u.mPulse_upgradeCost.ToString();
            else if(value=="postMortem_upgradeCost") _txt=u.postMortem_upgradeCost.ToString();
            else if(value=="teleport_upgradeCost") _txt=u.teleport_upgradeCost.ToString();
            else if(value=="overhaul_upgradeCost") _txt=u.overhaul_upgradeCost.ToString();
            else if(value=="crMend_upgradeCost") _txt=u.crMend_upgradeCost.ToString();
            else if(value=="enDiss_upgradeCost") _txt=u.enDiss_upgradeCost.ToString();
        }
    #endregion
    #region//SaveSerial
        if(SaveSerial.instance!=null){      var s=SaveSerial.instance;//var ss=s.settingsData;
            if(value=="inputType"){_txt=s.settingsData.inputType.ToString();}
            else if(value=="joystickType"){_txt=s.settingsData.joystickType.ToString();}
            else if(value=="joystickSize"){_txt=System.Math.Round(s.settingsData.joystickSize,2).ToString();}
            else if(value=="loginUsername"){_txt=s.hyperGamerLoginData.username.ToString();}
        }
    #endregion
    #region//DBAccess
        if(DBAccess.instance!=null){    var db=DBAccess.instance;
            if(value=="loginMessage"){_txt=db.loginMessage;}
            else if(value=="submitMessage"){_txt=db.submitMessage;}
        }
    #endregion
    #region//GameRules
        if(GameRules.instance!=null){   var gr=GameRules.instance;
            if(value=="cfgName") _txt=gr.cfgName;
            else if(value=="cfgDesc") _txt=gr.cfgDesc;
            else if(value=="scoreDisplay"){if(GameSession.instance!=null){
                    if(gr.scoreDisplay==scoreDisplay.score)_txt=GameSession.instance.score.ToString();
                    else if(gr.scoreDisplay==scoreDisplay.sessionTime)_txt=GameSession.instance.GetGameSessionTimeFormat().ToString();
                }
            }
            else if(value=="gameSpeedGR") _txt=gr.defaultGameSpeed.ToString();

            else if(value=="shopScoreRangeGR"){if(gr.shopSpawnReqs is spawnScore){var sr=(spawnScore)gr.shopSpawnReqs;var ss=sr.scoreMaxSetRange;
                                                if(ss.x!=ss.y){_txt=ss.x.ToString()+"-"+ss.y.ToString();}
                                                else _txt=ss.x.ToString();}else _txt="?";
            }
            else if(value=="shopScoreRangeStartGR") if(gr.shopSpawnReqs is spawnScore){var sr=(spawnScore)gr.shopSpawnReqs;var ss=sr.scoreMaxSetRange;_txt=ss.x.ToString();}else _txt="?";
            else if(value=="shopScoreRangeEndGR") if(gr.shopSpawnReqs is spawnScore){var sr=(spawnScore)gr.shopSpawnReqs;var ss=sr.scoreMaxSetRange;_txt=ss.y.ToString();}else _txt="?";

            else if(value=="waveScoreRangeGR"){if(gr.waveSpawnReqs is spawnScore){var sr=(spawnScore)gr.waveSpawnReqs;var ss=sr.scoreMaxSetRange;
                                                if(ss.x!=ss.y){_txt=ss.x.ToString()+"-"+ss.y.ToString();}
                                                else _txt=ss.x.ToString();}else _txt="?";
            }
            else if(value=="waveScoreRangeStartGR") if(gr.waveSpawnReqs is spawnScore){var sr=(spawnScore)gr.waveSpawnReqs;_txt=sr.scoreMaxSetRange.x.ToString();}else _txt="?";
            else if(value=="waveScoreRangeEndGR") if(gr.waveSpawnReqs is spawnScore){var sr=(spawnScore)gr.waveSpawnReqs;_txt=sr.scoreMaxSetRange.y.ToString();}else _txt="?";
            else if(value=="waveTimeRangeStartGR") if(gr.waveSpawnReqs is spawnReqs&&!gr.waveSpawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=gr.waveSpawnReqs;_txt=sr.time.x.ToString();}else _txt="?";
            else if(value=="waveTimeRangeEndGR") if(gr.waveSpawnReqs is spawnReqs&&!gr.waveSpawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=gr.waveSpawnReqs;_txt=sr.time.y.ToString();}else _txt="?";
            else if(value=="waveKillsNeededGR") if(gr.waveSpawnReqs is spawnKills){var sr=(spawnKills)gr.waveSpawnReqs;_txt=sr.killsNeeded.ToString();}else _txt="?";

            else if(value=="waveWeightsTotalSumGR") _txt=GameRules.instance.wavesWeightsSumTotal.ToString();

            else if(value=="healthStartingPlayerGR") _txt=gr.healthPlayer.ToString();
            else if(value=="healthMaxPlayerGR") _txt=gr.healthMaxPlayer.ToString();
            else if(value=="defensePlayerGR") _txt=gr.defensePlayer.ToString();
            else if(value=="energyPlayerGR") _txt=gr.energyPlayer.ToString()+"/"+gr.energyMaxPlayer.ToString();
            else if(value=="energyStartingPlayerGR") _txt=gr.energyPlayer.ToString();
            else if(value=="energyMaxPlayerGR") _txt=gr.energyMaxPlayer.ToString();
            else if(value=="speedPlayerGR") _txt=gr.moveSpeedPlayer.ToString();

            else if(value=="powerupsCapacity") _txt=gr.powerupsCapacity.ToString();

            else if(value=="energyGain_EnergyBallGR") _txt=gr.energyBall_energyGain.ToString();
            else if(value=="energyGain_BatteryGR") _txt=gr.battery_energyGain.ToString();
            else if(value=="benergyGain_BlackBallGR") _txt=gr.benergyBallGain.ToString();
            else if(value=="benergyGain_BlackVialGR") _txt=gr.benergyVialGain.ToString();
            else if(value=="crystalGainGR") _txt=gr.crystalGain.ToString();
            else if(value=="crystalBigGainGR") _txt=gr.crystalBigGain.ToString();
            else if(value=="hpGain_MedkitGR") _txt=gr.medkit_hpGain.ToString();
            else if(value=="energyGain_MedkitGR") _txt=gr.medkit_energyGain.ToString();
            else if(value=="absorpGain_LunarGelGR") _txt=gr.lunarGel_hpGain.ToString();
            else if(value=="energyGain_PowerupGR") _txt=gr.powerups_energyGain.ToString();
            else if(value=="energyNeeded_PowerupGR") _txt=gr.powerups_energyNeeded.ToString();

            //Sandbox Speciffic
            if(SandboxCanvas.instance!=null){   var sb=SandboxCanvas.instance;
                if(value=="saveSelected") _txt=sb.saveSelected.Replace(".json","");
                else if(value=="buildVersionSB") _txt="Build "+sb.buildVersion;
                else if(value=="presetNameSB") _txt="PRESET FROM: "+sb.defPresetGameruleset.cfgName;
                else if(value=="bgHueGR") _txt=gr.bgMaterial.hue.ToString();
                else if(value=="bgSaturGR") _txt=gr.bgMaterial.saturation.ToString();
                else if(value=="bgValueGR") _txt=gr.bgMaterial.value.ToString();
                else if(value=="bgNegativeGR") _txt=gr.bgMaterial.negative.ToString();
                else if(value=="bgPixelateGR") _txt=gr.bgMaterial.pixelate.ToString();
                else if(value=="bgBlueGR") _txt=gr.bgMaterial.blur.ToString();

                if(sb._enMod()!=null){
                    if(value=="name_EnemySB") _txt=sb._enMod().name;
                    else if(value=="health_EnemySB") _txt=sb._enMod().healthStart.ToString();
                    else if(value=="healthMax_EnemySB") _txt=sb._enMod().healthMax.ToString();
                    else if(value=="defense_EnemySB") _txt=sb._enMod().defense.ToString();
                    else if(value=="scoreStart_EnemySB") _txt=sb._enMod().scoreValue.x.ToString();
                    else if(value=="scoreEnd_EnemySB") _txt=sb._enMod().scoreValue.y.ToString();
                    
                    else if(value=="sprMatHue_EnemySB") _txt=sb._enModSprMat().hue.ToString();
                    else if(value=="sprMatSatur_EnemySB") _txt=sb._enModSprMat().saturation.ToString();
                    else if(value=="sprMatValue_EnemySB") _txt=sb._enModSprMat().value.ToString();
                    else if(value=="sprMatNegative_EnemySB") _txt=sb._enModSprMat().negative.ToString();
                    else if(value=="sprMatPixelate_EnemySB") _txt=sb._enModSprMat().pixelate.ToString();
                    else if(value=="sprMatBlur_EnemySB") _txt=sb._enModSprMat().blur.ToString();
                }

                PowerupsSpawnerGR _pwSp=null;
                if(value.Contains("_PwrupSpawnerSB")){if(_pwSp==null){if(!String.IsNullOrEmpty(sb.powerupSpawnerToModify))_pwSp=gr.powerupSpawners.Find(x=>x.name==sb.powerupSpawnerToModify);}}
                if(_pwSp!=null){
                    if(value=="name_PwrupSpawnerSB") _txt=_pwSp.name;
                    if(value=="scoreRangeStart_PwrupSpawnerSB") if(_pwSp.spawnReqs is spawnScore){var sr=(spawnScore)_pwSp.spawnReqs;_txt=sr.scoreMaxSetRange.x.ToString();}else _txt="?";
                    if(value=="scoreRangeEnd_PwrupSpawnerSB") if(_pwSp.spawnReqs is spawnScore){var sr=(spawnScore)_pwSp.spawnReqs;_txt=sr.scoreMaxSetRange.y.ToString();}else _txt="?";
                    if(value=="timeRangeStart_PwrupSpawnerSB") if(_pwSp.spawnReqs is spawnReqs&&!_pwSp.spawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=_pwSp.spawnReqs;_txt=sr.time.x.ToString();}else _txt="?";
                    if(value=="timeRangeEnd_PwrupSpawnerSB") if(_pwSp.spawnReqs is spawnReqs&&!_pwSp.spawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=_pwSp.spawnReqs;_txt=sr.time.y.ToString();}else _txt="?";
                    if(value=="killsNeeded_PwrupSpawnerSB") if(_pwSp.spawnReqs is spawnKills){var sr=(spawnKills)_pwSp.spawnReqs;_txt=sr.killsNeeded.ToString();}else _txt="?";
                    else if(value=="weightsTotalSum_PwrupSpawnerSB") _txt=_pwSp.sum.ToString();
                }
            }
        }
    #endregion
        
        if(txt!=null)txt.text=_txt;
        else{if(tmpInput!=null){if(UIInputSystem.instance!=null)if(UIInputSystem.instance.currentSelected!=tmpInput.gameObject){tmpInput.text=_txt;}
        foreach(TextMeshProUGUI t in GetComponentsInChildren<TextMeshProUGUI>()){t.text=_txt;}}}
    }
}
