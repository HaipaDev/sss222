using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI scoreText;
    GameSession gameSession;
    DataSavable dataSavable;
    SaveSerial saveSerial;
    Player player;
    PlayerSkills pskills;
    UpgradeMenu upgradeMenu;
    Shop shopMenu;
    [SerializeField] public string value = "score";
    [SerializeField] float valueLimitD=-1;
    [SerializeField] bool changeOnValidate;

    // Start is called before the first frame update
    void Start(){
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
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
        if (value == "score") scoreText.text = gameSession.GetScore().ToString();
        else if (value == "evscore") scoreText.text = gameSession.GetEVScore().ToString();
        else if (value == "coins") scoreText.text = gameSession.GetCoins().ToString();
        else if (value == "cores") scoreText.text = gameSession.GetCores().ToString();
        else if (value.Contains("highscore")) scoreText.text = gameSession.GetHighscore(/*int.Parse(value.Split('e')[1])*/GameSession.instance.gameModeSelected).ToString();
        else if (value == "version") scoreText.text = gameSession.GetVersion();
        else if (value == "hpOffMax") scoreText.text = Mathf.RoundToInt(player.health).ToString()+"/"+player.maxHP.ToString();
        else if (value == "energyOffMax") scoreText.text = Mathf.RoundToInt(player.energy).ToString()+"/"+player.maxEnergy.ToString();
        else if (value == "max_hp") scoreText.text = player.maxHP.ToString();
        else if (value == "max_energy") scoreText.text = player.maxEnergy.ToString();
        else if (value == "speed") scoreText.text = (player.moveSpeed-(player.moveSpeedInit-1)).ToString();
        else if (value == "hpRegen") if(player.hpRegenEnabled==true){scoreText.text = player.hpRegenAmnt.ToString();}else{scoreText.text="0";}
        else if (value == "enRegen") if(player.enRegenEnabled==true){scoreText.text = player.enRegenAmnt.ToString();}else{scoreText.text="0";}
        else if (value == "scoreMulti") scoreText.text = gameSession.scoreMulti.ToString();
        else if (value == "luck") scoreText.text = gameSession.luckMulti.ToString();

        else if (value == "purchases") scoreText.text = "Reputation: "+shopMenu.purchases.ToString();
        else if (value == "reputation") scoreText.text = "Reputation: "+shopMenu.reputation.ToString();
        else if (value == "lvl_ship") scoreText.text = "Ship Level: "+upgradeMenu.total_UpgradesLvl.ToString();
        else if (value == "lvl_hp") scoreText.text = "Lvl. "+upgradeMenu.maxHealth_UpgradesLvl.ToString();
        else if (value == "lvl_energy") scoreText.text = "Lvl. "+upgradeMenu.maxEnergy_UpgradesLvl.ToString();
        else if (value == "lvl_speed") scoreText.text = "Lvl. "+upgradeMenu.speed_UpgradesLvl.ToString();
        else if (value == "lvl_hpRegen") scoreText.text = "Lvl. "+upgradeMenu.hpRegen_UpgradesLvl.ToString();
        else if (value == "lvl_enRegen") scoreText.text = "Lvl. "+upgradeMenu.enRegen_UpgradesLvl.ToString();
        else if (value == "lvl_luck") scoreText.text = "Lvl. "+upgradeMenu.luck_UpgradesLvl.ToString();

        else if (value == "maxHealth_upgradeCost") scoreText.text = upgradeMenu.maxHealth_UpgradeCost.ToString();
        else if (value == "maxEnergy_upgradeCost") scoreText.text = upgradeMenu.maxEnergy_UpgradeCost.ToString();
        else if (value == "speed_upgradeCost") scoreText.text = upgradeMenu.speed_UpgradeCost.ToString();
        else if (value == "hpRegen_upgradeCost") scoreText.text = upgradeMenu.hpRegen_UpgradeCost.ToString();
        else if (value == "enRegen_upgradeCost") scoreText.text = upgradeMenu.enRegen_UpgradeCost.ToString();
        else if (value == "luck_upgradeCost") scoreText.text = upgradeMenu.luck_UpgradeCost.ToString();
        else if (value == "defaultPowerup_upgradeCost1") scoreText.text = upgradeMenu.defaultPowerup_upgradeCost1.ToString();
        else if (value == "defaultPowerup_upgradeCost2") scoreText.text = upgradeMenu.defaultPowerup_upgradeCost2.ToString();
        else if (value == "defaultPowerup_upgradeCost3") scoreText.text = upgradeMenu.defaultPowerup_upgradeCost3.ToString();
        else if (value == "energyRefill_upgradeCost") scoreText.text = upgradeMenu.energyRefill_upgradeCost.ToString();
        else if (value == "mPulse_upgradeCost") scoreText.text = upgradeMenu.mPulse_upgradeCost.ToString();
        else if (value == "postMortem_upgradeCost") scoreText.text = upgradeMenu.postMortem_upgradeCost.ToString();
        else if (value == "teleport_upgradeCost") scoreText.text = upgradeMenu.teleport_upgradeCost.ToString();
        else if (value == "overhaul_upgradeCost") scoreText.text = upgradeMenu.overhaul_upgradeCost.ToString();

        else if (value == "cooldownQ") scoreText.text = System.Math.Round(pskills.cooldownQ,0).ToString();
        else if (value == "cooldownE") scoreText.text = System.Math.Round(pskills.cooldownE,0).ToString();
        else if (value == "timerTeleport")if(FindObjectOfType<PlayerSkills>()!=null){ scoreText.text = System.Math.Round(pskills.timerTeleport,1).ToString();}else{Destroy(transform.parent.gameObject);}

        else if (value == "cfgName")if(GameRules.instance!=null){scoreText.text = GameRules.instance.cfgName;}else{Debug.LogError("GameRules Not Present");}
        else if (value == "speedGRPlayer") scoreText.text = GameRules.instance.moveSpeedPlayer.ToString();
        else if (value == "healthGRPlayer") scoreText.text = GameRules.instance.healthPlayer.ToString();
        else if (value == "energyGRPlayer") scoreText.text = GameRules.instance.energyPlayer.ToString();


        else if (value == "joystickType"){scoreText.text = SaveSerial.instance.joystickType.ToString();}
        else if (value == "joystickSize"){scoreText.text = System.Math.Round(SaveSerial.instance.joystickSize,2).ToString();}
        
        /*else if (value == "state"){
            var value = System.Math.Round(player.GetGCloverTimer(),1);

            if (value <= valueLimitD){ value = 0; }
            else { scoreText.text = value.ToString(); }
        }*/
    }
}
