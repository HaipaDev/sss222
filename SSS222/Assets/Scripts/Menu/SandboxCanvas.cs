using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SandboxCanvas : MonoBehaviour{
    [Header("Panels")]
    [SceneObjectsOnly][SerializeField]GameObject defaultPanel;
    [SceneObjectsOnly][SerializeField]GameObject presetsPanel;
    [SceneObjectsOnly][SerializeField]GameObject globalPanel;
    [SceneObjectsOnly][SerializeField]GameObject playerPanel;
    [Header("Other Objects")]
    [SceneObjectsOnly][SerializeField]GameObject powerupInventory;
    [DisableInEditorMode][SceneObjectsOnly][SerializeField]int powerupToSet;
    [SceneObjectsOnly][SerializeField]GameObject powerupChoices;
    void Start(){
        OpenDefaultPanel();
        SetPowerupChoices();
    }
    void Update(){
        CheckESC();
        SetPowerups();
    }
    public void Back(){
        if(_anyFirstLevelPanelsActive()){OpenDefaultPanel();}
        else{GSceneManager.instance.LoadGameModeChooseScene();}
    }
    public void OpenDefaultPanel(){CloseAllPanels();defaultPanel.SetActive(true);}
    public void OpenPresetsPanel(){CloseAllPanels();presetsPanel.SetActive(true);}
    public void OpenGlobalPanel(){CloseAllPanels();globalPanel.SetActive(true);}
    public void OpenPlayerPanel(){CloseAllPanels();playerPanel.SetActive(true);}
    bool _anyFirstLevelPanelsActive(){bool b=false;
        if(presetsPanel.activeSelf
        ||globalPanel.activeSelf
        ||playerPanel.activeSelf
        ){b=true;}
        return b;}
    void CloseAllPanels(){
        defaultPanel.SetActive(false);
        presetsPanel.SetActive(false);
        globalPanel.SetActive(false);
        playerPanel.SetActive(false);

        powerupChoices.SetActive(false);
    }

    public void SetPreset(string str){StartCoroutine(SetPresetI(str));}
    public IEnumerator SetPresetI(string str){
        OpenDefaultPanel();
        if(GameRules.instance!=null)Destroy(GameRules.instance.gameObject);
        yield return new WaitForSecondsRealtime(0.02f);
        var go=Instantiate(GameCreator.instance.gamerulesetsPrefabs[GameSession.instance.GetGamemodeID(str)]);
        go.name="GRSandbox";
        go.GetComponent<GameRules>().cfgName="Sandbox Mode";
    }

#region//Global
    public void SetCrystalsOn(bool v){GameRules.instance.crystalsOn=v;}
    public void SetXpOn(bool v){GameRules.instance.xpOn=v;}
    public void SetCoresOn(bool v){GameRules.instance.coresOn=v;}
    public void SetLevelingOn(bool v){GameRules.instance.levelingOn=v;}
    public void SetShopOn(bool v){GameRules.instance.shopOn=v;}
    public void SetShopCargoOn(bool v){GameRules.instance.shopCargoOn=v;}
    public void SetModulesOn(bool v){GameRules.instance.modulesOn=v;}
    public void SetBarrierOn(bool v){GameRules.instance.barrierOn=v;}
    public void SetWaveScoreRangeStart(string v){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by score!");GameRules.instance.waveSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}}
    public void SetWaveScoreRangeEnd(string v){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by score!");GameRules.instance.waveSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}}
    public void SetShopScoreRangeStart(string v){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Shop spawns are not set by score!");GameRules.instance.shopSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}}
    public void SetShopScoreRangeEnd(string v){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Shop spawns are not set by score!");GameRules.instance.shopSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}}
#endregion
#region//Player
    public void SetHealth(string v){GameRules.instance.healthPlayer=float.Parse(v);
        if(GameRules.instance.healthPlayer>GameRules.instance.healthMaxPlayer){GameRules.instance.healthMaxPlayer=GameRules.instance.healthPlayer;}}
    public void SetHealthMax(string v){GameRules.instance.healthMaxPlayer=float.Parse(v);}
    public void SetEnergy(string v){GameRules.instance.energyPlayer=float.Parse(v);
        if(GameRules.instance.energyPlayer>GameRules.instance.energyMaxPlayer){GameRules.instance.energyMaxPlayer=GameRules.instance.energyPlayer;}
        if(GameRules.instance.energyMaxPlayer==0){GameRules.instance.energyOnPlayer=false;}else{GameRules.instance.energyOnPlayer=true;}}
    public void SetEnergyMax(string v){GameRules.instance.energyMaxPlayer=float.Parse(v);
        if(GameRules.instance.energyMaxPlayer==0){GameRules.instance.energyOnPlayer=false;}else{GameRules.instance.energyOnPlayer=true;}}
    public void SetMoveAxis(){
        var i=GameRules.instance;
        if(i.moveX&&!i.moveY){i.moveX=false;i.moveY=true;}
        else if(!i.moveX&&i.moveY){i.moveX=true;i.moveY=true;}
        else if(i.moveX&&i.moveY){i.moveX=true;i.moveY=false;}
    }
    public void SetSpeed(string v){GameRules.instance.moveSpeedPlayer=float.Parse(v);}
    public void SetPowerupsCapacity(float v){GameRules.instance.powerupsCapacity=(int)v;}
    public void SetAutoshoot(bool v){GameRules.instance.autoShootPlayer=v;}
    public void OpenPowerupChoices(int id){powerupChoices.SetActive(true);powerupChoices.transform.position=new Vector2(Input.mousePosition.x,Input.mousePosition.y+50f);powerupToSet=id;}
    public void SetPowerupStarting(string v){
        if(GameRules.instance.powerupsStarting.Count<=powerupToSet){for(var i=GameRules.instance.powerupsStarting.Count;i<=powerupToSet;i++){
            GameRules.instance.powerupsStarting.Add(new Powerup());}}
        if(GameRules.instance.powerupsStarting[powerupToSet]==null){GameRules.instance.powerupsStarting[powerupToSet]=new Powerup();}
        else{GameRules.instance.powerupsStarting[powerupToSet].name=v;}
        powerupChoices.SetActive(false);
    }
#endregion

#region//Start & Update functions
    void SetPowerupChoices(){
        GameObject prefab=powerupChoices.transform.GetChild(0).GetChild(0).gameObject;
            prefab.name="null";
            prefab.GetComponent<Image>().sprite=GameAssets.instance.Spr("nullPwrup");
            prefab.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(""));
        foreach(PowerupItem p in GameAssets.instance.powerupItems){if(p.powerupType==powerupType.weapon){
            GameObject go=Instantiate(prefab,powerupChoices.transform.GetChild(0));
            go.name=p.name;
            go.GetComponent<Image>().sprite=GameAssets.instance.Get(p.assetName).GetComponent<SpriteRenderer>().sprite;
            go.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(p.name));
        }}
        //Destroy(powerupChoices.transform.GetChild(0).GetChild(0).gameObject);
        powerupChoices.SetActive(false);
    }

    void CheckESC(){if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button1))Back();}
    void SetPowerups(){
        if(powerupInventory!=null){
            for(var i=0;i<GameRules.instance.powerupsCapacity;i++){
                Sprite _spr;
                if(GameRules.instance.powerupsStarting.Count>i&&!System.String.IsNullOrEmpty(GameRules.instance.powerupsStarting[i].name)){
                    _spr=GameAssets.instance.Get(GameAssets.instance.GetPowerupItem(GameRules.instance.powerupsStarting[i].name).assetName).GetComponent<SpriteRenderer>().sprite;
                }else{_spr=GameAssets.instance.Spr("nullPwrup");}
                powerupInventory.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite=_spr;
            }
            for(var i=9;i>=GameRules.instance.powerupsCapacity;i--){
                if(powerupInventory.transform.GetChild(0).GetChild(i).gameObject.activeSelf)powerupInventory.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
            for(var i=0;i<GameRules.instance.powerupsCapacity;i++){
                if(!powerupInventory.transform.GetChild(0).GetChild(i).gameObject.activeSelf)powerupInventory.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }
        }else{Debug.LogError("PowerupInventory not assigned!");}
    }
#endregion
}