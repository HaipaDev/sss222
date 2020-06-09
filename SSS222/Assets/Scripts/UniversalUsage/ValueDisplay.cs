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
    [SerializeField] string value = "score";
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
        else if (value == "highscore") scoreText.text = gameSession.GetHighscore().ToString();
        else if (value == "version") scoreText.text = gameSession.GetVersion();
        else if (value == "hpOffMax") scoreText.text = Mathf.RoundToInt(player.health).ToString()+"/"+player.maxHP.ToString();
        else if (value == "energyOffMax") scoreText.text = Mathf.RoundToInt(player.energy).ToString()+"/"+player.maxEnergy.ToString();
        else if (value == "max_hp") scoreText.text = player.maxHP.ToString();
        else if (value == "max_energy") scoreText.text = player.maxEnergy.ToString();
        else if (value == "speed") scoreText.text = (player.moveSpeed-(player.moveSpeedInit-1)).ToString();
        else if (value == "hpRegen") if(player.hpRegenEnabled==true){scoreText.text = player.hpRegenAmnt.ToString();}else{scoreText.text="0";}
        else if (value == "enRegen") if(player.enRegenEnabled==true){scoreText.text = player.enRegenAmnt.ToString();}else{scoreText.text="0";}

        else if (value == "lvl_ship") scoreText.text = "Ship Level: "+upgradeMenu.total_UpgradesLvl.ToString();
        else if (value == "lvl_hp") scoreText.text = "Lvl. "+upgradeMenu.maxHealth_UpgradesLvl.ToString();
        else if (value == "lvl_energy") scoreText.text = "Lvl. "+upgradeMenu.maxEnergy_UpgradesLvl.ToString();
        else if (value == "lvl_speed") scoreText.text = "Lvl. "+upgradeMenu.speed_UpgradesLvl.ToString();
        else if (value == "lvl_hpRegen") scoreText.text = "Lvl. "+upgradeMenu.hpRegen_UpgradesLvl.ToString();
        else if (value == "lvl_enRegen") scoreText.text = "Lvl. "+upgradeMenu.enRegen_UpgradesLvl.ToString();

        else if (value == "maxHealth_upgradeCost") scoreText.text = upgradeMenu.maxHealth_UpgradeCost.ToString();
        else if (value == "maxEnergy_upgradeCost") scoreText.text = upgradeMenu.maxEnergy_UpgradeCost.ToString();
        else if (value == "speed_upgradeCost") scoreText.text = upgradeMenu.speed_UpgradeCost.ToString();
        else if (value == "hpRegen_upgradeCost") scoreText.text = upgradeMenu.hpRegen_UpgradeCost.ToString();
        else if (value == "enRegen_upgradeCost") scoreText.text = upgradeMenu.enRegen_UpgradeCost.ToString();
        else if (value == "defaultPowerup_upgradeCost1") scoreText.text = upgradeMenu.defaultPowerup_upgradeCost1.ToString();
        else if (value == "defaultPowerup_upgradeCost2") scoreText.text = upgradeMenu.defaultPowerup_upgradeCost2.ToString();
        else if (value == "defaultPowerup_upgradeCost3") scoreText.text = upgradeMenu.defaultPowerup_upgradeCost3.ToString();
        else if (value == "energyRefill_upgradeCost") scoreText.text = upgradeMenu.energyRefill_upgradeCost.ToString();
        else if (value == "mPulse_upgradeCost") scoreText.text = upgradeMenu.mPulse_upgradeCost.ToString();
        else if (value == "postMortem_upgradeCost") scoreText.text = upgradeMenu.postMortem_upgradeCost.ToString();

        else if (value == "cooldownQ") scoreText.text = System.Math.Round(pskills.cooldownQ,0).ToString();
        else if (value == "cooldownE") scoreText.text = System.Math.Round(pskills.cooldownE,0).ToString();
        else if (value == "timerTeleport") scoreText.text = System.Math.Round(pskills.timerTeleport,1).ToString();
        
        /*else if (value == "state"){
            var value = System.Math.Round(player.GetGCloverTimer(),1);

            if (value <= valueLimitD){ value = 0; }
            else { scoreText.text = value.ToString(); }
        }*/
    }
}
