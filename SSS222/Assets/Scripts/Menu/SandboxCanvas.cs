using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SandboxCanvas : MonoBehaviour{     public static SandboxCanvas instance;
    [Title("Panels", titleAlignment: TitleAlignments.Centered)]
    [SceneObjectsOnly][SerializeField]GameObject defaultPanel;
    [SceneObjectsOnly][SerializeField]GameObject presetsPanel;
    [SceneObjectsOnly][SerializeField]GameObject globalPanel;
    [SceneObjectsOnly][SerializeField]GameObject playerPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemiesPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemyPanel;
    [SceneObjectsOnly][SerializeField]GameObject wavesPanel;
    [SceneObjectsOnly][SerializeField]GameObject collectiblesPanel;
    [Header("Enemy Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject enemyMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemySpritePanel;
    [SceneObjectsOnly][SerializeField]GameObject enemySpritesLibPanel;
    [Header("Collectibles Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject collectiblesMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject basicCollectiblesPanel;
    [SceneObjectsOnly][SerializeField]GameObject powerupsPanel;
    [Title("Variables & Other obj", titleAlignment: TitleAlignments.Centered)]
    [DisableInEditorMode][SerializeField] public GameRules presetGameruleset;
    [DisableInEditorMode][SerializeField][Range(0,360)]public int bgHue;
    [DisableInEditorMode][SerializeField][Range(0,2)]public float bgSatur=1;
    [DisableInEditorMode][SerializeField][Range(0,2)]public float bgValue=1;
    [Header("")]
    [SceneObjectsOnly][SerializeField]GameObject powerupInventory;
    [DisableInEditorMode][SerializeField]int powerupToSet;
    [SceneObjectsOnly][SerializeField]GameObject startingPowerupChoices;
    [Header("")]
    [DisableInEditorMode][SerializeField]public string enemyToModify;
    [DisableInEditorMode][SerializeField]List<GSprite> enemySprites;
    [Header("")]
    [SceneObjectsOnly][SerializeField]TMP_Dropdown wavesSpawnTypeDropdown;
    [SerializeField]string[] wavesSpawnReqsTypes;
    [SceneObjectsOnly][SerializeField]GameObject wavesScoreInput;
    [SceneObjectsOnly][SerializeField]GameObject wavesTimeInput;
    [SceneObjectsOnly][SerializeField]GameObject wavesKillsInput;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI wavesWeightsSumTotal;
    [Header("")]
    [DisableInEditorMode][SerializeField]public string powerupSpawnerToModify;
    [SceneObjectsOnly][SerializeField]TMP_Dropdown pwrupsSpawnTypeDropdown;
    [SerializeField]string[] pwrupsSpawnReqsTypes;
    [SceneObjectsOnly][SerializeField]GameObject pwrupsScoreInput;
    [SceneObjectsOnly][SerializeField]GameObject pwrupsTimeInput;
    [SceneObjectsOnly][SerializeField]GameObject pwrupsKillsInput;
    [SceneObjectsOnly][SerializeField]TextMeshProUGUI powerupsWeightsSumTotal;
    void Start(){
        instance=this;
        presetGameruleset=GameCreator.instance.gamerulesetsPrefabs[0];
        OpenDefaultPanel();
        SetupEverything();
        SetWaveSpawnReqsInputs();
        SetStartingPowerupChoices();
    }
    void Update(){
        CheckESC();
        SetPowerups();
        GameSession.instance.gameSpeed=GameRules.instance.defaultGameSpeed;
    }
    public void Back(){
        if(_anyFirstLevelPanelsActive()){OpenDefaultPanel();}
            else if(enemyMainPanel.activeSelf){OpenEnemiesPanel();}
                else if(enemySpritePanel.activeSelf){OpenEnemyPanel(enemyToModify);}
                    else if(enemySpritesLibPanel.activeSelf){OpenEnemySpritePanel();}
            else if(collectiblesMainPanel.activeSelf){OpenDefaultPanel();}
                else if(basicCollectiblesPanel.activeSelf||powerupsPanel.activeSelf){OpenCollectiblesPanel();}
        else{GSceneManager.instance.LoadGameModeChooseScene();}
    }
    public void OpenDefaultPanel(){CloseAllPanels();defaultPanel.SetActive(true);}
    public void OpenPresetsPanel(){CloseAllPanels();presetsPanel.SetActive(true);}
    public void OpenGlobalPanel(){CloseAllPanels();globalPanel.SetActive(true);}
    public void OpenPlayerPanel(){CloseAllPanels();playerPanel.SetActive(true);}

    public void OpenEnemiesPanel(){CloseAllPanels();enemiesPanel.SetActive(true);SetEnemyPreviewsSprite();}
    public void OpenEnemyPanel(string str){CloseAllPanels();enemyPanel.SetActive(true);enemyMainPanel.SetActive(true);enemyToModify=str;SetEnemyPreviewsSprite();
        enemySpritePanel.SetActive(false);enemySpritesLibPanel.SetActive(false);}
    public void OpenEnemySpritePanel(){if(_canModifySpriteEn()){CloseAllPanels();enemyPanel.SetActive(true);enemySpritePanel.SetActive(true);SetEnemyPreviewsSprite();}}
    public void OpenEnemySpritesLibPanel(){CloseAllPanels();enemyPanel.SetActive(true);enemySpritesLibPanel.SetActive(true);}

    public void OpenWavesPanel(){CloseAllPanels();wavesPanel.SetActive(true);OpenWavesSpawnReqsInputs();}

    public void OpenCollectiblesPanel(){CloseAllPanels();collectiblesPanel.SetActive(true);collectiblesMainPanel.SetActive(true);
        basicCollectiblesPanel.SetActive(false);powerupsPanel.SetActive(false);}
    public void OpenBasicCollectiblesPanel(){CloseAllPanels();collectiblesPanel.SetActive(true);basicCollectiblesPanel.SetActive(true);}
    public void OpenPowerupsSpawnPanel(string str){CloseAllPanels();collectiblesPanel.SetActive(true);powerupsPanel.SetActive(true);powerupSpawnerToModify=str;
        SetPowerupsSpawnsChoices();SetPowerupSpawnerReqsInputs();OpenPowerupSpawnReqsInputs();}
    
    bool _anyFirstLevelPanelsActive(){bool b=false;
        if(presetsPanel.activeSelf
        ||globalPanel.activeSelf
        ||playerPanel.activeSelf
        ||enemiesPanel.activeSelf
        ||wavesPanel.activeSelf
        ){b=true;}
        return b;}
    void CloseAllPanels(){
        defaultPanel.SetActive(false);
        presetsPanel.SetActive(false);
        globalPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        enemyPanel.SetActive(false);
        wavesPanel.SetActive(false);
        collectiblesPanel.SetActive(false);

        enemyMainPanel.SetActive(false);
        enemySpritePanel.SetActive(false);
        enemySpritesLibPanel.SetActive(false);

        startingPowerupChoices.SetActive(false);

        collectiblesMainPanel.SetActive(false);
        basicCollectiblesPanel.SetActive(false);
        powerupsPanel.SetActive(false);
    }
    void OpenWavesSpawnReqsInputs(){
        CloseAllWaveSpawnInputs();
        switch(GameRules.instance.waveSpawnReqsType){
            case spawnReqsType.score:wavesScoreInput.SetActive(true);break;
            case spawnReqsType.kills:wavesKillsInput.SetActive(true);break;
            default:wavesTimeInput.SetActive(true);break;
        }

        void CloseAllWaveSpawnInputs(){
            wavesTimeInput.SetActive(false);
            wavesScoreInput.SetActive(false);
            wavesKillsInput.SetActive(false);
        }
    }
    void OpenPowerupSpawnReqsInputs(){
        CloseAllPowerupSpawnInputs();
        switch(_pwrSpawnMod().spawnReqsType){
            case spawnReqsType.score:pwrupsScoreInput.SetActive(true);break;
            case spawnReqsType.kills:pwrupsKillsInput.SetActive(true);break;
            default:pwrupsTimeInput.SetActive(true);break;
        }

        void CloseAllPowerupSpawnInputs(){
            pwrupsTimeInput.SetActive(false);
            pwrupsScoreInput.SetActive(false);
            pwrupsKillsInput.SetActive(false);
        }
    }

    public void SetPreset(string str){StartCoroutine(SetPresetI(str));}
    public IEnumerator SetPresetI(string str){
        if(GameRules.instance!=null)Destroy(GameRules.instance.gameObject);
        yield return new WaitForSecondsRealtime(0.02f);
        presetGameruleset=GameCreator.instance.gamerulesetsPrefabs[GameSession.instance.GetGamemodeID(str)];
        var go=Instantiate(presetGameruleset);
        go.name="GRSandbox";
        go.GetComponent<GameRules>().cfgName="Sandbox Mode";
        OpenDefaultPanel();
        SetupEverything();
    }

    void SetupEverything(){
        SetEnemyChoices();
        SetEnemySpritesLibrary();
        SetWaveChoices();
        SetPowerupSpawnersChoices();
        SetPowerupsSpawnsChoices();
    }

#region//Global
    public void SetGameSpeed(float v){GameRules.instance.defaultGameSpeed=(float)System.Math.Round(v,2);}
    public void SetScoreDisplay(){scoreDisplay s=GameRules.instance.scoreDisplay;
        switch(s){
            case scoreDisplay.sessionTime: s=scoreDisplay.score;break;
            case scoreDisplay.score: s=scoreDisplay.sessionTime;break;
        }
        GameRules.instance.scoreDisplay=s;
    }
    public void SetCrystalsOn(bool v){GameRules.instance.crystalsOn=v;}
    public void SetXpOn(bool v){GameRules.instance.xpOn=v;}
    public void SetCoresOn(bool v){GameRules.instance.coresOn=v;}
    public void SetLevelingOn(bool v){GameRules.instance.levelingOn=v;}
    public void SetShopOn(bool v){GameRules.instance.shopOn=v;}
    public void SetShopCargoOn(bool v){GameRules.instance.shopCargoOn=v;}
    public void SetModulesOn(bool v){GameRules.instance.modulesOn=v;}
    public void SetBarrierOn(bool v){GameRules.instance.barrierOn=v;}
    public void SetShopScoreRangeStart(string v){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Shop spawns are not set by score!");GameRules.instance.shopSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}}
    public void SetShopScoreRangeEnd(string v){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Shop spawns are not set by score!");GameRules.instance.shopSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}}
    public void SetBackgroundHue(float v){bgHue=(int)v;UpdateBgMaterial();}
    public void SetBackgroundSatur(float v){bgSatur=v;UpdateBgMaterial();}
    public void SetBackgroundValue(float v){bgValue=v;UpdateBgMaterial();}
    Material GetBgMat(){
        Material _mat=null;
        if(GameRules.instance.bgMaterial!=null)_mat=GameRules.instance.bgMaterial;
        if(_mat==null||(_mat!=null&&!_mat.shader.name.Contains("AllIn1SpriteShader"))){
            if(GameAssets.instance.Mat("HueShiftBG")!=null)_mat=Instantiate(GameAssets.instance.Mat("HueShiftBG"));
            else{
                _mat=new Material(Resources.Load("AllIn1SpriteShader", typeof(Shader)) as Shader);
                _mat.SetTexture("_MainTex",FindObjectOfType<BGManager>().GetBgTexture());
                _mat.EnableKeyword("HSV_ON");
                bgHue=_mat.GetInt("_HsvShift");
                bgSatur=_mat.GetFloat("_HsvSaturation");
                bgValue=_mat.GetFloat("_HsvBright");
            }
        }
        return _mat;
    }
    void UpdateBgMaterial(){    Material _mat=GetBgMat();
        _mat.SetInt("_HsvShift",bgHue);
        _mat.SetFloat("_HsvSaturation",bgSatur);
        _mat.SetFloat("_HsvBright",bgValue);
        GameRules.instance.bgMaterial=_mat;
    }
#endregion
#region//Player
    public void SetHealth(string v){GameRules.instance.healthPlayer=float.Parse(v);
        if(GameRules.instance.healthPlayer>GameRules.instance.healthMaxPlayer){GameRules.instance.healthMaxPlayer=GameRules.instance.healthPlayer;}}
    public void SetHealthMax(string v){GameRules.instance.healthMaxPlayer=float.Parse(v);}
    public void SetDefense(string v){GameRules.instance.defensePlayer=int.Parse(v);}
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
    public void OpenstartingPowerupChoices(int id){startingPowerupChoices.SetActive(true);startingPowerupChoices.transform.position=new Vector2(Input.mousePosition.x,Input.mousePosition.y+50f);powerupToSet=id;}
    public void SetPowerupStarting(string v){
        if(GameRules.instance.powerupsStarting.Count<=powerupToSet){for(var i=GameRules.instance.powerupsStarting.Count;i<=powerupToSet;i++){
            GameRules.instance.powerupsStarting.Add(new Powerup());}}
        if(GameRules.instance.powerupsStarting[powerupToSet]==null){GameRules.instance.powerupsStarting[powerupToSet]=new Powerup();}
        else{GameRules.instance.powerupsStarting[powerupToSet].name=v;}
        startingPowerupChoices.SetActive(false);
    }
#endregion
#region//Enemies
    #region///returns
    bool _canModifySpriteEn(){bool b=true;string s=enemyToModify;
        if(s=="Vortex Wheel"
        ||s=="Comet"
        ){
            b=false;}
    return b;}
    public EnemyClass _enGR(string str, GameRules gr){EnemyClass _en=null;if(!String.IsNullOrEmpty(str)){_en=System.Array.Find(gr.enemies,x=>x.name==str);}return _en;}
    public EnemyClass _en(string str){return _enGR(str,GameRules.instance);}
    public EnemyClass _enMod(){return _en(enemyToModify);}
    public Sprite _enSprGR(string str,GameRules gr){Sprite _spr=null;
        if(_enGR(str,gr)!=null){
            if(_enGR(str,gr).spr!=null){_spr=_enGR(str,gr).spr;}
            else{
                if(str=="Comet"){_spr=GameRules.instance.cometSettings.sprites[0];}
            }
            if(_spr!=null)return _spr;
            else{Debug.LogWarning("No spr for: "+str);return null;}
        }else{Debug.LogWarning("No enemy by name: "+str);return null;}
    }
    public Sprite _enSpr(string str){return _enSprGR(str,GameRules.instance);}
    public Sprite _enModSpr(){return _enSpr(enemyToModify);}
    public Material _enSprMat(string str){
        Material _mat=null;
        if(_en(str)!=null){
            if(_en(str).sprMat!=null)_mat=_en(str).sprMat;
            if(_mat==null||(_mat!=null&&!_mat.shader.name.Contains("AllIn1SpriteShader"))){Debug.LogWarning("New sprite material! for: "+str);
                if(GameAssets.instance.Mat("HueShift")!=null){_mat=Instantiate(GameAssets.instance.Mat("HueShift"));}
                _mat.SetInt("_HsvShift",0);
                _en(str).sprMat=_mat;
            }
        }
        return _mat;
    }
    public Material _enModSprMat(){return _enSprMat(enemyToModify);}
    #endregion///returns

    //Enemy Main Settings
    public void SetEnemyHealth(string v){_enMod().healthStart=float.Parse(v);}
    public void SetEnemyHealthMax(string v){_enMod().healthMax=float.Parse(v);}
    public void SetEnemyDefense(string v){_enMod().defense=int.Parse(v);}
    public void SetEnemyScoreStart(string v){_enMod().scoreValue=new Vector2(float.Parse(v),_enMod().scoreValue.y);}
    public void SetEnemyScoreEnd(string v){_enMod().scoreValue=new Vector2(_enMod().scoreValue.x,float.Parse(v));}

    //Enemy Sprite
    public void SetEnemySprite(string v){_enMod().spr=enemySprites.Find(x=>x.name==v).spr;OpenEnemySpritePanel();}
    public void SetEnemySprMatHue(float v){if(_enModSprMat()!=null)_enModSprMat().SetInt("_HsvShift",(int)v);}
    public void SetEnemySprMatSatur(float v){if(_enModSprMat()!=null)_enModSprMat().SetFloat("_HsvSaturation",v);}
    public void SetEnemySprMatValue(float v){if(_enModSprMat()!=null)_enModSprMat().SetFloat("_HsvBright",v);}
#endregion
#region//Waves
    public void SetWaveSpawnReqsType(int v){    spawnReqsType srt=0;string t=wavesSpawnReqsTypes[v];
        switch(t){
            case "Time":srt=spawnReqsType.time;break;
            case "Kills":srt=spawnReqsType.kills;break;
            default:srt=spawnReqsType.score;break;
        }
        GameRules.instance.waveSpawnReqsType=srt;
        spawnReqsMono.Validate(ref GameRules.instance.waveSpawnReqs, ref srt);
        OpenWavesSpawnReqsInputs();
    }
    public void SetWaveScoreRangeStart(string v){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by score!");GameRules.instance.waveSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}}
    public void SetWaveScoreRangeEnd(string v){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by score!");GameRules.instance.waveSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}}
    public void SetWaveTimeRangeStart(string v){if(GameRules.instance.waveSpawnReqs is spawnReqs&&!GameRules.instance.waveSpawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=GameRules.instance.waveSpawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Wave spawns are not set by time!");GameRules.instance.waveSpawnReqsType=spawnReqsType.time;GameRules.instance.waveSpawnReqs=new spawnReqs();var sr=GameRules.instance.waveSpawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}}
    public void SetWaveTimeRangeEnd(string v){if(GameRules.instance.waveSpawnReqs is spawnReqs&&!GameRules.instance.waveSpawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=GameRules.instance.waveSpawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Wave spawns are not set by time!");GameRules.instance.waveSpawnReqsType=spawnReqsType.time;GameRules.instance.waveSpawnReqs=new spawnReqs();var sr=GameRules.instance.waveSpawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}}
    public void SetWaveKillsNeeded(string v){if(GameRules.instance.waveSpawnReqs is spawnKills){var sr=(spawnKills)GameRules.instance.waveSpawnReqs;sr.killsNeeded=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by kills!");GameRules.instance.waveSpawnReqsType=spawnReqsType.kills;GameRules.instance.waveSpawnReqs=new spawnKills();var sr=(spawnKills)GameRules.instance.waveSpawnReqs;sr.killsNeeded=int.Parse(v);}}

    public void SetWaveWeight(string n, string v){
        LootTableEntryWaves e=GameRules.instance.waveList.Find(x=>x.lootItem.name==n);
        e.dropChance=(float)System.Math.Round(float.Parse(v),2);
        GameRules.instance.SumUpWavesWeightsTotal();
    }
    public void SetWaveLvlReq(string n, string v){
        LootTableEntryWaves e=GameRules.instance.waveList.Find(x=>x.lootItem.name==n);
        e.levelReq=int.Parse(v);
    }
    public void SetWaveStarting(string n){
        GameRules.instance.startingWave=GameRules.instance.waveList.FindIndex(x=>x.lootItem.name==n);
        GameRules.instance.startingWaveRandom=false;
    }
    public void SetWaveStartingRandom(bool v){ GameRules.instance.startingWaveRandom=v;}
#endregion
#region//Collectibles
    ///PowerupSpawns
    #region//returns
    public PowerupsSpawnerGR _pwrSpawnGR(string str, GameRules gr){PowerupsSpawnerGR _pwSp=null;if(!String.IsNullOrEmpty(str)){_pwSp=gr.powerupSpawners.Find(x=>x.name==str);}return _pwSp;}
    public PowerupsSpawnerGR _pwrSpawn(string str){return _pwrSpawnGR(str,GameRules.instance);}
    public PowerupsSpawnerGR _pwrSpawnMod(){return _pwrSpawn(powerupSpawnerToModify);}
    #endregion//returns
    public void SetPowerupSpawnerReqsType(int v){    spawnReqsType srt=0;string t=pwrupsSpawnReqsTypes[v];
        switch(t){
            case "Time":srt=spawnReqsType.time;break;
            case "Kills":srt=spawnReqsType.kills;break;
            default:srt=spawnReqsType.score;break;
        }
        _pwrSpawnMod().spawnReqsType=srt;
        spawnReqsMono.Validate(ref _pwrSpawnMod().spawnReqs, ref srt);
        OpenPowerupSpawnReqsInputs();
    }
    public void SetPowerupSpawnerName(string v){
        Transform powerupsSpawnerListTransform=collectiblesMainPanel.transform.GetChild(1).GetChild(0);
        var go=powerupsSpawnerListTransform.Find(powerupSpawnerToModify);

        _pwrSpawnMod().name=v;
        powerupSpawnerToModify=v;
        
        if(go!=null){go.name=v;
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=v;}
    }
    public void SetPowerupSpawnerScoreRangeStart(string v){if(_pwrSpawnMod().spawnReqs is spawnScore){var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Powerup spawns are not set by score!");_pwrSpawnMod().spawnReqsType=spawnReqsType.score;_pwrSpawnMod().spawnReqs=new spawnScore();var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}}
    public void SetPowerupSpawnerScoreRangeEnd(string v){if(_pwrSpawnMod().spawnReqs is spawnScore){var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Powerup spawns are not set by score!");_pwrSpawnMod().spawnReqsType=spawnReqsType.score;_pwrSpawnMod().spawnReqs=new spawnScore();var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}}
    public void SetPowerupSpawnerTimeRangeStart(string v){if(_pwrSpawnMod().spawnReqs is spawnReqs&&!_pwrSpawnMod().spawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=_pwrSpawnMod().spawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Powerup spawns are not set by time!");_pwrSpawnMod().spawnReqsType=spawnReqsType.time;_pwrSpawnMod().spawnReqs=new spawnReqs();var sr=_pwrSpawnMod().spawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}}
    public void SetPowerupSpawnerTimeRangeEnd(string v){if(_pwrSpawnMod().spawnReqs is spawnReqs&&!_pwrSpawnMod().spawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=_pwrSpawnMod().spawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Powerup spawns are not set by time!");_pwrSpawnMod().spawnReqsType=spawnReqsType.time;_pwrSpawnMod().spawnReqs=new spawnReqs();var sr=_pwrSpawnMod().spawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}}
    public void SetPowerupSpawnerKillsNeeded(string v){if(_pwrSpawnMod().spawnReqs is spawnKills){var sr=(spawnKills)_pwrSpawnMod().spawnReqs;sr.killsNeeded=int.Parse(v);}
        else{Debug.LogWarning("Powerup spawns are not set by kills!");_pwrSpawnMod().spawnReqsType=spawnReqsType.kills;_pwrSpawnMod().spawnReqs=new spawnKills();var sr=(spawnKills)_pwrSpawnMod().spawnReqs;sr.killsNeeded=int.Parse(v);}}

    public void SetPwrupWeight(string n, string v){
        LootTableEntryPowerup e=_pwrSpawnMod().powerupList.Find(x=>x.lootItem.name==n);
        e.dropChance=(float)System.Math.Round(float.Parse(v),2);
        _pwrSpawnMod().SumUpPowerupsWeightsTotal();
    }
    public void SetPwrupLvlReq(string n, string v){
        LootTableEntryPowerup e=_pwrSpawnMod().powerupList.Find(x=>x.lootItem.name==n);
        e.levelReq=int.Parse(v);
    }
#endregion

#region//Start & Update functions
    void SetStartingPowerupChoices(){
        Transform startingPowerupChoicesListTransform=startingPowerupChoices.transform.GetChild(0);
        GameObject prefab=startingPowerupChoicesListTransform.GetChild(0).gameObject;
        //for(var d=startingPowerupChoicesListTransform.childCount-1;d>=1;d--){Destroy(startingPowerupChoicesListTransform.GetChild(d).gameObject);}
            prefab.name="null";
            prefab.GetComponent<Image>().sprite=GameAssets.instance.Spr("nullPwrup");
            prefab.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(""));
        foreach(PowerupItem p in GameAssets.instance.powerupItems){if(p.powerupType==powerupType.weapon){
            GameObject go=Instantiate(prefab,startingPowerupChoicesListTransform);
            go.name=p.name;
            go.GetComponent<Image>().sprite=GameAssets.instance.GetObjSpr(p.assetName);
            go.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(p.name));
        }}
        startingPowerupChoices.SetActive(false);
    }

    void SetEnemyChoices(){
        Transform enemiesListTransform=enemiesPanel.transform.GetChild(1).GetChild(0);
        GameObject prefab=enemiesListTransform.GetChild(0).gameObject;
        for(var d=enemiesListTransform.childCount-1;d>=1;d--){Destroy(enemiesListTransform.GetChild(d).gameObject);}

        foreach(EnemyClass e in GameRules.instance.enemies){
            GameObject go=Instantiate(prefab,enemiesListTransform);
            go.name=e.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=e.name;
            Sprite _spr=_enSpr(e.name);
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>OpenEnemyPanel(e.name));
        }
        Destroy(prefab);
    }
    void SetEnemySpritesLibrary(){
        enemySprites=new List<GSprite>();
        foreach(GameRules gr in GameCreator.instance.gamerulesetsPrefabs){
            foreach(EnemyClass e in gr.enemies){
                Sprite _spr=_enSprGR(e.name,gr);
                string _n=_spr.name;
                
                if(e.name=="Comet"){
                    foreach(Sprite _cspr in gr.cometSettings.sprites.Concat(gr.cometSettings.spritesLunar).ToArray()){
                        _n=_cspr.name;
                        if(!enemySprites.Exists(x=>x.spr==_cspr))enemySprites.Add(new GSprite{name=_n,spr=_cspr});
                    }
                }else{      if(enemySprites.Exists(x=>x.name.Contains(_n))){_n+="_";}
                    if(!enemySprites.Exists(x=>x.spr==_spr)){enemySprites.Add(new GSprite{name=_n,spr=_spr});}
                }
            }
        }
        enemySprites=enemySprites.OrderBy(x=>x.name).ToList();
        

        Transform enemySprLibTransform=enemySpritesLibPanel.transform.GetChild(1).GetChild(0);
        GameObject prefab=enemySprLibTransform.GetChild(0).gameObject;
        for(var d=enemySprLibTransform.childCount-1;d>=1;d--){Destroy(enemySprLibTransform.GetChild(d).gameObject);}

        foreach(GSprite s in enemySprites){
            GameObject go=Instantiate(prefab,enemySprLibTransform);
            go.name=s.name;
            Sprite _spr=s.spr;
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>SetEnemySprite(s.name));
        }
        Destroy(prefab);
    }

    void SetWaveSpawnReqsInputs(){
        List<TMPro.TMP_Dropdown.OptionData> options=new List<TMPro.TMP_Dropdown.OptionData>();
        if(wavesSpawnTypeDropdown.options.Count==1){
            for(var i=0;i<wavesSpawnReqsTypes.Length;i++){
                options.Add(new TMPro.TMP_Dropdown.OptionData(wavesSpawnReqsTypes[i],wavesSpawnTypeDropdown.itemImage.sprite));
            }
            wavesSpawnTypeDropdown.ClearOptions();
            wavesSpawnTypeDropdown.AddOptions(options);
        }
        int o;
        switch(GameRules.instance.waveSpawnReqsType){
            case spawnReqsType.time:o=1;break;
            case spawnReqsType.kills:o=2;break;
            default:o=0;break;
        }
        wavesSpawnTypeDropdown.SetValueWithoutNotify(o);
        wavesSpawnTypeDropdown.RefreshShownValue();
    }
    void SetWaveChoices(){
        Transform wavesListTransform=wavesPanel.transform.GetChild(2).GetChild(0);
        GameObject prefab=wavesListTransform.GetChild(0).gameObject;
        for(var d=wavesListTransform.childCount-1;d>=1;d--){Destroy(wavesListTransform.GetChild(d).gameObject);}

        wavesWeightsSumTotal.text=GameRules.instance.wavesWeightsSumTotal.ToString();
        List<LootTableEntryWaves> sortedWaveList=GameRules.instance.waveList.OrderBy(x=>x.lootItem.name).ToList();
        foreach(LootTableEntryWaves e in sortedWaveList){
            GameObject go=Instantiate(prefab,wavesListTransform);
            go.name=e.lootItem.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=e.lootItem.name;
            Sprite _spr=e.lootItem.thumbnail;
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            var dropChanceInput=go.transform.GetChild(3).GetComponent<TMP_InputField>();
                dropChanceInput.text=e.dropChance.ToString();
                dropChanceInput.onEndEdit.AddListener((string s)=>SetWaveWeight(e.lootItem.name,s));
            var lvlReqInput=go.transform.GetChild(6).GetComponent<TMP_InputField>();
                lvlReqInput.text=e.levelReq.ToString();
                lvlReqInput.onEndEdit.AddListener((string s)=>SetWaveLvlReq(e.lootItem.name,s));
            var waveStartingT=go.transform.GetChild(8);
                waveStartingT.GetComponent<Button>().onClick.AddListener(()=>SetWaveStarting(e.lootItem.name));
                waveStartingT.GetChild(0).GetComponent<ToggleValue>().value="waveStarting_"+e.lootItem.name;
        }
        Destroy(prefab);
    }

    void SetPowerupSpawnerReqsInputs(){
        List<TMPro.TMP_Dropdown.OptionData> options=new List<TMPro.TMP_Dropdown.OptionData>();
        if(pwrupsSpawnTypeDropdown.options.Count==1){
            for(var i=0;i<pwrupsSpawnReqsTypes.Length;i++){
                options.Add(new TMPro.TMP_Dropdown.OptionData(pwrupsSpawnReqsTypes[i],pwrupsSpawnTypeDropdown.itemImage.sprite));
            }
            pwrupsSpawnTypeDropdown.ClearOptions();
            pwrupsSpawnTypeDropdown.AddOptions(options);
        }
        int o;
        switch(_pwrSpawnMod().spawnReqsType){
            case spawnReqsType.time:o=1;break;
            case spawnReqsType.kills:o=2;break;
            default:o=0;break;
        }
        pwrupsSpawnTypeDropdown.SetValueWithoutNotify(o);
        pwrupsSpawnTypeDropdown.RefreshShownValue();
    }
    void SetPowerupSpawnersChoices(){
        Transform powerupsSpawnerListTransform=collectiblesMainPanel.transform.GetChild(1).GetChild(0);
        GameObject prefab=powerupsSpawnerListTransform.GetChild(1).gameObject;
        for(var d=powerupsSpawnerListTransform.childCount-1;d>=2;d--){Destroy(powerupsSpawnerListTransform.GetChild(d).gameObject);}

        List<PowerupsSpawnerGR> sortedPowerupSpawnerList=GameRules.instance.powerupSpawners.OrderBy(x=>x.name).ToList();
        foreach(PowerupsSpawnerGR e in sortedPowerupSpawnerList){
            GameObject go=Instantiate(prefab,powerupsSpawnerListTransform);
            go.name=e.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=e.name;
            Sprite _spr=GameAssets.instance.Get(e.sprAssetName).GetComponent<SpriteRenderer>().sprite;
            if(GameAssets.instance.Spr(e.sprAssetName)!=null)GameAssets.instance.Spr(e.sprAssetName);
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>OpenPowerupsSpawnPanel(e.name));
        }
        Destroy(prefab);
    }
    void SetPowerupsSpawnsChoices(){
        Transform powerupsListTransform=powerupsPanel.transform.GetChild(2).GetChild(0);
        GameObject prefab=powerupsListTransform.GetChild(0).gameObject;
        for(var d=powerupsListTransform.childCount-1;d>=1;d--){Destroy(powerupsListTransform.GetChild(d).gameObject);}

        powerupsWeightsSumTotal.text=_pwrSpawnMod().sum.ToString();
        List<LootTableEntryPowerup> sortedPowerupsList=_pwrSpawnMod().powerupList.OrderBy(x=>x.lootItem.name).ToList();
        foreach(LootTableEntryPowerup e in sortedPowerupsList){
            GameObject go=Instantiate(prefab,powerupsListTransform);
            go.name=e.lootItem.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=e.name;
            Sprite _spr=GameAssets.instance.Get(e.lootItem.assetName).GetComponent<SpriteRenderer>().sprite;
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            var dropChanceInput=go.transform.GetChild(3).GetComponent<TMP_InputField>();
                dropChanceInput.text=e.dropChance.ToString();
                dropChanceInput.onEndEdit.AddListener((string s)=>SetPwrupWeight(e.lootItem.name,s));
            var lvlReqInput=go.transform.GetChild(6).GetComponent<TMP_InputField>();
                lvlReqInput.text=e.levelReq.ToString();
                lvlReqInput.onEndEdit.AddListener((string s)=>SetPwrupLvlReq(e.lootItem.name,s));
        }
        Destroy(prefab);
    }


    void CheckESC(){if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button1))Back();}
    void SetPowerups(){
        if(powerupInventory!=null){
            for(var i=0;i<GameRules.instance.powerupsCapacity;i++){
                Sprite _spr;
                if(GameRules.instance.powerupsStarting.Count>i&&!String.IsNullOrEmpty(GameRules.instance.powerupsStarting[i].name)){
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

            if(UIInputSystem.instance!=null){
                if(!Array.Exists(powerupInventory.GetComponentsInChildren<Transform>(),x=>x.gameObject==UIInputSystem.instance.currentSelected
                    &&x.GetComponent<Button>()==UIInputSystem.instance.btn))startingPowerupChoices.SetActive(false);
            }
        }else{Debug.LogError("PowerupInventory not assigned!");}
    }
    void SetEnemyPreviewsSprite(){
        if(true){foreach(Transform t in enemiesPanel.transform.GetChild(1).GetChild(0)){if(_en(t.gameObject.name)!=null){
            t.GetChild(0).GetComponent<Image>().sprite=_enSpr(t.gameObject.name);
            t.GetChild(0).GetComponent<Image>().material=_enSprMat(t.gameObject.name);
        }}}
        if(_enMod()!=null){
            if(_enModSpr()!=null){
                enemyMainPanel.transform.GetChild(1).GetComponent<Image>().sprite=_enModSpr();
                enemySpritePanel.transform.GetChild(1).GetComponent<Image>().sprite=_enModSpr();
            }
            if(_enModSprMat()!=null){
                enemyMainPanel.transform.GetChild(1).GetComponent<Image>().material=_enModSprMat();
                enemySpritePanel.transform.GetChild(1).GetComponent<Image>().material=_enModSprMat();
            }
        }
    }
#endregion
}