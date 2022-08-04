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
            else if(value=="highscore") _txt=gs.GetHighscoreCurrent().score.ToString();
            else if(value=="gameSpeed") _txt=gs.gameSpeed.ToString();
            else if(value=="gameVersion") _txt=gs.gameVersion;
            else if(value=="buildVersion") _txt=gs.buildVersion.ToString();
            else if(value=="buildVersion-Menu") _txt="build "+gs.buildVersion.ToString();
            else if(value=="timePlayed"){
                if(gs.gamemodeSelected!=-1){_txt=gs.GetGameSessionTimeFormat();}
                else{_txt=(Mathf.RoundToInt(gs.currentPlaytime)*GameRules.instance.secondToDistanceRatio).ToString()+"m";}
            }else if(value=="timePlayed_txt"){
                if(gs.gamemodeSelected!=-1){_txt="Time Played :";}
                else{_txt="Distance traveled :";}
            }
            else if(value=="scoreMulti") _txt=gs.scoreMulti.ToString();
            else if(value=="luck") _txt=gs.luckMulti.ToString();

            else if(value=="cfgNameCurrent")_txt=gs.GetGameRulesCurrent().cfgName;
            else if(value=="adventureDeath")if(FindObjectOfType<PlayerHolobody>()!=null){
                var phb=FindObjectOfType<PlayerHolobody>();
                if(phb.GetTimeLeft()!=-4){_txt="Your Crystals are still lost at "+(SaveSerial.instance.advD.holo_timeAt*GameRules.instance.secondToDistanceRatio).ToString()+"m";}
                else{_txt="Your Crystals have been lost at "+(Mathf.RoundToInt(gs.currentPlaytime)*GameRules.instance.secondToDistanceRatio).ToString()+"m";}
            }else{_txt="";}
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
            else if(value=="speedBase") _txt=(p.moveSpeedBase).ToString();
            else if(value=="hpRegen") if(p.hpRegenEnabled==true){_txt=p.hpRegenAmnt.ToString();}else{_txt="0";}
            else if(value=="enRegen") if(p.enRegenEnabled==true){_txt=p.enRegenAmnt.ToString();}else{_txt="0";}
            else if(value=="defenseBase") _txt=(p.defenseBase).ToString();
            else if(value=="shootSpeedBase") _txt=(p.shootMultiBase).ToString();
            else if(value=="critChanceBase") _txt=(p.critChanceBase).ToString()+"%";


            ///Player Modules
            PlayerModules pmodules=p.GetComponent<PlayerModules>();
            if(pmodules!=null){
                if(value.Contains("cooldownSkill_")){_txt=System.Math.Round(pmodules.GetSkillFromID(int.Parse(value.Split('_')[1])).cooldown,0).ToString();}
                else if(value=="timerTeleport"){
                    if(FindObjectOfType<PlayerModules>()!=null){_txt=System.Math.Round(pmodules.timerTeleport,1).ToString();}else{Destroy(transform.parent.gameObject);}
                    if(_txt=="-4"){var tempColor=txt.color;tempColor.a=0;txt.color=tempColor;}
                }
                if(value.Contains("moduleEquippedSlot_")){var m=pmodules.moduleSlots.FindIndex(x=>x==value.Split('_')[1]);if(m!=-1){_txt="Slot: "+(m+1).ToString();}else _txt="Not-Eq";}
                if(value.Contains("skillEquippedSlot_")){var s=pmodules.skillsSlots.FindIndex(x=>x==value.Split('_')[1]);if(s!=-1){_txt="Slot: "+(s+1).ToString();}else _txt="Not-Eq";}

                if(value.Contains("moduleLvl_")){var m=pmodules.modulesList.Find(x=>x.name==value.Split('_')[1]);if(m!=null){_txt="Lvl "+m.lvl.ToString();}else _txt="";}
                if(value.Contains("skillLvl_")){var s=pmodules.skillsList.Find(x=>x.name==value.Split('_')[1]);if(s!=null){_txt="Lvl "+s.lvl.ToString();}else _txt="";}

                if(value=="lvl_ship"){_txt="Ship Level: "+pmodules.shipLvl.ToString();}
                if(value=="lvlPopup"){
                    _txt="Lvl up! ("+pmodules.shipLvl.ToString()+")";
                    /*if(GameRules.instance.autoleveling){_txt="Lvl up! ("+pmodules.shipLvl.ToString()+")";}
                    else{_txt="Lvl up available! (->"+(pmodules.shipLvl+1).ToString()+")<br> [L to LvlUp]";}*/
                }
                if(value=="celestPointPopup"){
                    if(Player.instance.GetComponent<PlayerModules>()._isAutoAscend()){_txt="Celestial Point! ("+pmodules.shipLvlFraction.ToString()+"/"+pmodules.lvlFractionsMax.ToString()+")";}
                    else{
                        if(pmodules.shipLvlFraction+1<pmodules.lvlFractionsMax){_txt="Celestial Point available! ("+(pmodules.shipLvlFraction+1).ToString()+"/"+pmodules.lvlFractionsMax.ToString()+")<br> (L to climb)";}
                        else{_txt="Lvl up available! ("+(pmodules.shipLvl+1).ToString()+")<br> (L to Lvlup)";}
                    }
                }

                if(value=="accumulatedCelestPoints"){
                    if(!GameSession.instance.CheckGamemodeSelected("Adventure")){_txt=pmodules.accumulatedCelestPoints.ToString();}
                    else{_txt=pmodules.shipLvlFraction.ToString();}
                }
                if(value=="bodyUpgraded"){_txt="Lvl "+pmodules.bodyUpgraded.ToString();}
                if(value=="engineUpgraded"){_txt="Lvl "+pmodules.engineUpgraded.ToString();}
                if(value=="blastersUpgraded"){_txt="Lvl "+pmodules.blastersUpgraded.ToString();}
            }
            ///Other player related
            if(value=="holodeath_popup"&&FindObjectOfType<PlayerHolobody>()!=null){
                _txt="Distance to Hologram: "+FindObjectOfType<PlayerHolobody>().GetDistanceLeft()+"m";
            }
        }
    #endregion
    #region//Shop & UpgradeMenu
        if(Shop.instance!=null){    var sh=Shop.instance;
            if(value=="purchases") _txt="Reputation: "+sh.purchases.ToString();
            else if(value=="reputation") _txt="Reputation: "+sh.reputation.ToString();
        }
    #endregion
    #region//SaveSerial
        if(SaveSerial.instance!=null){      var s=SaveSerial.instance;var ss=s.settingsData;var sh=s.hyperGamerLoginData;
            if(value=="inputType"){_txt=ss.inputType.ToString();}
            else if(value=="joystickType"){_txt=ss.joystickType.ToString();}
            else if(value=="joystickSize"){_txt=System.Math.Round(ss.joystickSize,2).ToString();}
            else if(value=="loginUsername"){if(Login.developerNicknames.Contains(sh.username)){_txt="<color="+Login.developerNicknameColor+">"+sh.username+"</color>";}else{_txt=sh.username;}}
            else if(value=="masterVolume"){_txt=ss.masterVolume.ToString();}
            else if(value=="soundVolume"){_txt=ss.soundVolume.ToString();}
            else if(value=="ambienceVolume"){_txt=ss.ambienceVolume.ToString();}
            else if(value=="musicVolume"){_txt=ss.musicVolume.ToString();}
        }
    #endregion
    #region//DBAccess
        if(DBAccess.instance!=null){    var db=DBAccess.instance;
            if(value=="loginMessage"){_txt=db.loginMessage;}
            else if(value=="loggedInMessage"){_txt=db.loggedInMessage;}
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
                    else if(gr.scoreDisplay==scoreDisplay.timeLeft)_txt=GameSession.instance.GetGameTimeLeftFormat().ToString();
                    else if(gr.scoreDisplay==scoreDisplay.sessionTimeAsDistance)_txt=(Mathf.RoundToInt(GameSession.instance.currentPlaytime)*GameRules.instance.secondToDistanceRatio).ToString()+"m";
                    
                    else if(gr.scoreDisplay==scoreDisplay.bossHealth){
                        if(FindObjectOfType<BossAI>()!=null){
                            var hp=Mathf.Clamp(FindObjectOfType<BossAI>().GetComponent<Enemy>().health,0,99999f);
                            var hpmax=FindObjectOfType<BossAI>().GetComponent<Enemy>().healthMax;
                            if(hp>=10){hp=Mathf.RoundToInt(hp);}else{hp=(float)System.Math.Round(hp,2);}
                            _txt=hp.ToString()+"/"+hpmax;
                        }else{_txt="0";}
                    }
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

            else if(value=="bodyUpgrade_price") _txt=gr.bodyUpgrade_price.ToString();
            else if(value=="engineUpgrade_price") _txt=gr.engineUpgrade_price.ToString();
            else if(value=="blastersUpgrade_price") _txt=gr.blastersUpgrade_price.ToString();

            else if(value=="bodyUpgrade_changes"){
                _txt=
                "+ "+gr.bodyUpgrade_defense.ToString()+" DEFENSE"+"<br>"+
                "+ "+gr.bodyUpgrade_powerupCapacity.ToString()+" POWERUP SLOTS"
                ;
            }
            else if(value=="engineUpgrade_changes"){
                if(GameRules.instance._isAdventure()){
                    if(Player.instance.GetComponent<PlayerModules>().engineUpgraded<1){
                        _txt=
                        "+ "+gr.engineUpgrade_moveSpeed.ToString()+" MOVE SPEED"+"<br>"+
                        "+ "+gr.engineUpgrade_energyRegen.ToString()+" ENERGY REGEN"
                        ;
                    }else{
                        _txt=
                        "+ "+gr.engineUpgrade_moveSpeed.ToString()+" MOVE SPEED"+"<br>"+
                        "- "+gr.engineUpgrade_energyRegenFreqMinus.ToString()+" ENERGY REGEN FREQUENCY"
                        ;
                    }
                }else{
                    if(Player.instance.GetComponent<PlayerModules>().engineUpgraded<1){
                        _txt=
                        "+ "+gr.engineUpgrade_moveSpeed.ToString()+" MOVE SPEED"+"<br>";
                        ;
                    }else{
                        _txt=
                        "+ "+gr.engineUpgrade_moveSpeed.ToString()+" MOVE SPEED"+"<br>"+
                        "- "+gr.engineUpgrade_energyRegenFreqMinus.ToString()+" ENERGY REGEN FREQUENCY"
                        ;
                    }
                }
            }
            else if(value=="blastersUpgrade_changes"){
                _txt=
                "+ "+gr.blastersUpgrade_shootMulti.ToString()+" SHOOT SPEED"+"<br>"+
                "+ "+gr.blastersUpgrade_critChance.ToString()+"% CRIT CHANCE"
                ;
            }

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
