using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SandboxCanvas : MonoBehaviour{     public static SandboxCanvas instance;
#region//Variables
    [Title("Main Panels", titleAlignment: TitleAlignments.Centered)]
    [SceneObjectsOnly][SerializeField]GameObject defaultPanel;
    [SceneObjectsOnly][SerializeField]GameObject presetAppearancePanel;
    [SceneObjectsOnly][SerializeField]GameObject presetsPanel;
    [SceneObjectsOnly][SerializeField]GameObject globalPanel;
    [SceneObjectsOnly][SerializeField]GameObject damagePanel;
    [SceneObjectsOnly][SerializeField]GameObject playerPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemiesPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemyPanel;
    [SceneObjectsOnly][SerializeField]GameObject spawnsPanel;
    [SceneObjectsOnly][SerializeField]GameObject collectiblesPanel;
    [Header("Presets Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject presetAppearanceMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject presetAppearanceIconPanel;
    [SceneObjectsOnly][SerializeField]GameObject presetAppearanceIconSprLibPanel;
    [Header("")]
    [SceneObjectsOnly][SerializeField]GameObject builtInPresetsPanel;
    [SceneObjectsOnly][SerializeField]GameObject yoursPresetsPanel;
    [SceneObjectsOnly][SerializeField]GameObject onlinePresetsPanel;
    [Header("Player Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject playerMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject playerSpritePanel;
    [SceneObjectsOnly][SerializeField]GameObject playerSpritesLibPanel;
    [Header("Enemy Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject enemyMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemySpritePanel;
    [SceneObjectsOnly][SerializeField]GameObject enemySpritesLibPanel;
    [Header("Spawns Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject spawnsMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject wavesPanel;
    [SceneObjectsOnly][SerializeField]GameObject disruptersPanel;
    [Header("Collectibles Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject collectiblesMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject basicCollectiblesPanel;
    [SceneObjectsOnly][SerializeField]GameObject powerupsPanel;

    [Title("Variables & Other obj", titleAlignment: TitleAlignments.Centered)]
    [AssetsOnly][SerializeField] GameObject sandboxIconsGo;
    [AssetsOnly][SerializeField] GameObject gameModeListElementPrefab;
    [AssetsOnly][SerializeField] GameObject yoursPrefabElementPrefab;
    [SceneObjectsOnly][SerializeField] public GameObject savePopup;
    [SceneObjectsOnly][SerializeField] public GameObject saveSandboxPrompt;
    [SceneObjectsOnly][SerializeField] public GameObject loadSandboxPrompt;
    [SceneObjectsOnly][SerializeField] public GameObject deleteSandboxPrompt;
    [SceneObjectsOnly][SerializeField] public GameObject quitSaveSandboxPrompt;
    [SceneObjectsOnly][SerializeField] public GameObject enterLoadSandboxPrompt;
    [SceneObjectsOnly][SerializeField] public GameObject loadDefaultPrompt;
    [DisableInEditorMode][SerializeField] public string saveSelected="Sandbox Save #1";
    //[DisableInEditorMode][SerializeField] public string _cachedSandboxName;
    [DisableInEditorMode][SerializeField] public string curSaveFileName;
    [DisableInEditorMode][SerializeField] public SandboxSaveInfo saveInfo=new SandboxSaveInfo();
    [DisableInEditorMode][SerializeField] public bool _modified=false;
    [DisableInEditorMode][SerializeField] public int savesCount;
    [DisableInEditorMode][SerializeField] public string _selectedDefPreset;
    [DisableInEditorMode][SerializeField] public GameRules defPresetGameruleset;
    [Header("")]
    [DisableInPlayMode][SerializeField]List<GSprite> presetIconSpritesPre;
    [DisableInEditorMode][SerializeField]List<GSprite> presetIconSprites;
    [DisableInEditorMode][SerializeField]List<GSprite> playerSprites;
    [SceneObjectsOnly][SerializeField]GameObject powerupInventory;
    [DisableInEditorMode][SerializeField]int powerupToSet=-1;
    [SceneObjectsOnly][SerializeField]GameObject startingPowerupChoices;
    [Header("")]
    [DisableInEditorMode][SerializeField]public string enemyToModify;
    [DisableInPlayMode][SerializeField]List<GSprite> enemySpritesPost;
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
    Color defaultColor=new Color(1,1,1,0.56f);
    Color selectedColor=new Color(0.3f,0.3f,1,0.56f);
    Color activeColor=new Color(1,1,0.3f,0.56f);
    bool _initiated=false;
    int sandboxSaveFormat=1;
#endregion
#region//Base
    void Start(){
        instance=this;
        if(!String.IsNullOrEmpty(GameManager.instance.GetTempSandboxSaveName())){
            SelectPreset(GameManager.instance.GetTempSandboxSaveName());
        }

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="SandboxMode"){
            CountSaveFiles();
            saveSelected="Sandbox Save #"+(savesCount+1);
            saveInfo.name="Sandbox Save #"+(savesCount+1);
            saveInfo.desc="New Sandbox Mode Savefile";
            defPresetGameruleset=CoreSetup.instance.gamerulesetsPrefabs[0];
            saveInfo.presetFrom=CoreSetup.instance.gamerulesetsPrefabs[0].cfgName;
            saveInfo.saveBuild=0;_modified=false;
            saveInfo.gameBuild=GameManager.instance.buildVersion;
            saveInfo.iconAssetName="questionMark";
        }
        OpenDefaultPanel();
        SetupEverything();
        SetWaveSpawnReqsInputs();
        SetStartingPowerupChoices();
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="SandboxMode"){
            SetBuiltInPresetsButtons();
            Destroy(defaultPanel.transform.GetChild(4).gameObject);//ModeInfo edit button

            if(!String.IsNullOrEmpty(SaveSerial.instance.lastEditedSandbox)){
                curSaveFileName=saveSelected;
                saveSelected=SaveSerial.instance.lastEditedSandbox;
                OpenEnterLoadPrompt();
            }
        }

        defaultColor=new Color(1,1,1,0.56f);
        selectedColor=new Color(0.3f,0.3f,1,0.56f);
        activeColor=new Color(1,1,0.3f,0.56f);
        StartCoroutine(Initiate());
    }
    IEnumerator Initiate(){yield return new WaitForSecondsRealtime(0.5f);SavePopup("Initiated",0.05f);_initiated=true;}
    string _backText;
    void Update(){
        if(GSceneManager.EscPressed()){Back();}
        GameManager.instance.defaultGameSpeed=GameRules.instance.defaultGameSpeed;

        if(_anySavingPromptActive()&&_backText!="DON'T"){_backText="DON'T";}
        else if(!_anySavingPromptActive()&&_backText!="BACK"){_backText="BACK";}
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="InfoGameMode")if(transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text!=_backText){transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text=_backText;}
    }
    public void Back(){
        if(_anyFirstLevelPanelsActive()){OpenDefaultPanel();return;}
            else if(presetsPanel.activeSelf){
                    if(_anySavingPromptActive()){CloseSavingPrompts();return;}
                    else{OpenDefaultPanel();saveSelected=curSaveFileName;return;}
                }
            else if(presetAppearanceIconPanel.activeSelf){OpenPresetAppearancePanel();return;}
                else if(presetAppearanceIconSprLibPanel.activeSelf){OpenPresetAppearanceIconPanel();return;}
            else if(playerSpritePanel.activeSelf){OpenPlayerPanel();return;}
                else if(playerSpritesLibPanel.activeSelf){OpenPlayerSpritePanel();return;}
            else if(wavesPanel.activeSelf||disruptersPanel.activeSelf){OpenSpawnsPanel();return;}
            else if(enemyMainPanel.activeSelf){OpenEnemiesPanel();return;}
                else if(enemySpritePanel.activeSelf){OpenEnemyPanel(enemyToModify);return;}
                    else if(enemySpritesLibPanel.activeSelf){OpenEnemySpritePanel();return;}
            else if(collectiblesMainPanel.activeSelf){OpenDefaultPanel();return;}
                else if(basicCollectiblesPanel.activeSelf||powerupsPanel.activeSelf){OpenCollectiblesPanel();return;}
        else{
            if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="InfoGameMode"){FindObjectOfType<ModeInfoManager>().SetActivePanel(0);return;}
            else{//if SandboxMode
                if(!_modified){
                    if(_anySavingPromptActive()){CloseSavingPrompts();return;}
                    else{QuitSandbox();return;}
                }else{
                    if(_anySavingPromptActive()){CloseSavingPrompts();return;}
                    if(!_anySavingPromptActive()){OpenQuitSavePrompt();return;}
                    if(saveInfo.saveBuild!=0){SaveSerial.instance.lastEditedSandbox=curSaveFileName;return;}
                }
            }
        }
    }
    public void QuitSandbox(){GSceneManager.instance.LoadGameModeChooseScene();}
    public bool _anySavingPromptActive(){
        return (
            saveSandboxPrompt.activeSelf||
            loadSandboxPrompt.activeSelf||
            deleteSandboxPrompt.activeSelf||
            quitSaveSandboxPrompt.activeSelf||
            enterLoadSandboxPrompt.activeSelf||
            loadDefaultPrompt.activeSelf
        );
    }
    public void OpenDefaultPanel(){CloseAllPanels();defaultPanel.SetActive(true);}
    public void OpenPresetAppearancePanel(){CloseAllPanels();presetAppearancePanel.SetActive(true);presetAppearanceMainPanel.SetActive(true);SetPresetIconPreviewsSprite();
        presetAppearanceIconPanel.SetActive(false);presetAppearanceIconSprLibPanel.SetActive(false);}
    public void OpenPresetAppearanceIconPanel(){CloseAllPanels();presetAppearancePanel.SetActive(true);presetAppearanceIconPanel.SetActive(true);SetPresetIconPreviewsSprite();
        presetAppearanceMainPanel.SetActive(false);presetAppearanceIconSprLibPanel.SetActive(false);}
    public void OpenPresetAppearanceIconSprLibPanel(){CloseAllPanels();presetAppearancePanel.SetActive(true);presetAppearanceIconSprLibPanel.SetActive(true);
        presetAppearanceMainPanel.SetActive(false);presetAppearanceIconPanel.SetActive(false);}

    string _selectedPresetPanel;
    public void OpenPresetsPanel(){
        if(savesCount>0&&String.IsNullOrEmpty(_selectedPresetPanel)){_selectedPresetPanel="yours";}
        switch(_selectedPresetPanel){
            case "yours":
                OpenYoursPresetsPanel();
            break;
            case "online":
                OpenOnlinePresetsPanel();
            break;
            default:
                OpenBuiltInPresetsPanel();
            break;
        }
    }
    public void OpenBuiltInPresetsPanel(){CloseAllPanels();presetsPanel.SetActive(true);builtInPresetsPanel.SetActive(true);ResetBuiltinPresetButtonsColors();/*HighlightActiveBuiltinPreset();*/_selectedPresetPanel="builtin";_selectedDefPreset="";}
    public void OpenYoursPresetsPanel(){CloseAllPanels();presetsPanel.SetActive(true);yoursPresetsPanel.SetActive(true);SetYoursPresetsButtons();HighlightActivePreset();/*HighlightSelectedPreset();*/_selectedPresetPanel="yours";_selectedDefPreset="";RefreshYoursPanel();}
    public void OpenOnlinePresetsPanel(){CloseAllPanels();presetsPanel.SetActive(true);onlinePresetsPanel.SetActive(true);_selectedPresetPanel="online";_selectedDefPreset="";}

    public void OpenGlobalPanel(){CloseAllPanels();globalPanel.SetActive(true);}
    public void OpenDamagePanel(){CloseAllPanels();damagePanel.SetActive(true);}
    
    public void OpenPlayerPanel(){CloseAllPanels();playerPanel.SetActive(true);playerMainPanel.SetActive(true);SetPlayerPreviewsSprite();
        playerSpritePanel.SetActive(false);playerSpritesLibPanel.SetActive(false);}
    public void OpenPlayerSpritePanel(){if(_canModifySpriteEn()){CloseAllPanels();playerPanel.SetActive(true);playerSpritePanel.SetActive(true);SetPlayerPreviewsSprite();}}
    public void OpenPlayerSpritesLibPanel(){CloseAllPanels();playerPanel.SetActive(true);playerSpritesLibPanel.SetActive(true);}

    public void OpenEnemiesPanel(){CloseAllPanels();enemiesPanel.SetActive(true);SetEnemyPreviewsSprite();}
    public void OpenEnemyPanel(string str){CloseAllPanels();enemyPanel.SetActive(true);enemyMainPanel.SetActive(true);enemyToModify=str;SetEnemyPreviewsSprite();SetEnemyTypePreview();
        enemySpritePanel.SetActive(false);enemySpritesLibPanel.SetActive(false);}
    public void OpenEnemySpritePanel(){if(_canModifySpriteEn()){CloseAllPanels();enemyPanel.SetActive(true);enemySpritePanel.SetActive(true);SetEnemyPreviewsSprite();}}
    public void OpenEnemySpritesLibPanel(){if(_canChangeSpriteEn()){CloseAllPanels();enemyPanel.SetActive(true);enemySpritesLibPanel.SetActive(true);}}

    public void OpenSpawnsPanel(){CloseAllPanels();spawnsPanel.SetActive(true);spawnsMainPanel.SetActive(true);
        wavesPanel.SetActive(false);disruptersPanel.SetActive(false);}
    public void OpenWavesPanel(){CloseAllPanels();spawnsPanel.SetActive(true);wavesPanel.SetActive(true);OpenWavesSpawnReqsInputs();}
    public void OpenDisruptersPanel(){CloseAllPanels();spawnsPanel.SetActive(true);disruptersPanel.SetActive(true);}

    public void OpenCollectiblesPanel(){CloseAllPanels();collectiblesPanel.SetActive(true);collectiblesMainPanel.SetActive(true);
        basicCollectiblesPanel.SetActive(false);powerupsPanel.SetActive(false);}
    public void OpenBasicCollectiblesPanel(){CloseAllPanels();collectiblesPanel.SetActive(true);basicCollectiblesPanel.SetActive(true);}
    public void OpenPowerupsSpawnPanel(string str){CloseAllPanels();collectiblesPanel.SetActive(true);powerupsPanel.SetActive(true);powerupSpawnerToModify=str;
        SetPowerupsSpawnsChoices();SetPowerupSpawnerReqsInputs();OpenPowerupSpawnReqsInputs();}
    
    bool _anyFirstLevelPanelsActive(){  return(
        //presetsPanel.activeSelf||
        presetAppearanceMainPanel.activeSelf||
        globalPanel.activeSelf||
        //damagePanel.activeSelf||
        playerMainPanel.activeSelf||
        enemiesPanel.activeSelf||
        spawnsPanel.activeSelf
    );}
    void CloseAllPanels(){
        defaultPanel.SetActive(false);
        presetAppearancePanel.SetActive(false);
        //if(presetsPanel.activeSelf){if(!String.IsNullOrEmpty(_cachedSandboxName)){saveSelected=_cachedSandboxName;_cachedSandboxName="";}}
        presetsPanel.SetActive(false);

        globalPanel.SetActive(false);
        //damagePanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        enemyPanel.SetActive(false);
        spawnsPanel.SetActive(false);
        collectiblesPanel.SetActive(false);

        presetAppearanceMainPanel.SetActive(false);
        presetAppearanceIconPanel.SetActive(false);
        presetAppearanceIconSprLibPanel.SetActive(false);
        builtInPresetsPanel.SetActive(false);
        yoursPresetsPanel.SetActive(false);
        onlinePresetsPanel.SetActive(false);

        playerMainPanel.SetActive(false);
        playerSpritePanel.SetActive(false);
        playerSpritesLibPanel.SetActive(false);
        startingPowerupChoices.SetActive(false);

        enemyMainPanel.SetActive(false);
        enemySpritePanel.SetActive(false);
        enemySpritesLibPanel.SetActive(false);

        spawnsMainPanel.SetActive(false);
        wavesPanel.SetActive(false);
        disruptersPanel.SetActive(false);

        collectiblesMainPanel.SetActive(false);
        basicCollectiblesPanel.SetActive(false);
        powerupsPanel.SetActive(false);

        CloseSavingPrompts();

        //saveSelected=saveInfo.name;
        _selectedDefPreset="";
        StopRefreshYoursPanel();
        ChangeSaveOverwriteBulb(false);
    }
    Coroutine refreshYoursPanelCor=null;
    void RefreshYoursPanel(){if(refreshYoursPanelCor==null)refreshYoursPanelCor=StartCoroutine(RefreshYoursPanelI());}
    void StopRefreshYoursPanel(){if(refreshYoursPanelCor!=null){StopCoroutine(refreshYoursPanelCor);refreshYoursPanelCor=null;}}
    IEnumerator RefreshYoursPanelI(){
        yield return new WaitForSecondsRealtime(0.2f);
        if(GetCountSaveFiles()!=savesCount)OpenYoursPresetsPanel();
        if(refreshYoursPanelCor!=null)refreshYoursPanelCor=null;RefreshYoursPanel();
        yield return null;
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

    void SetupEverything(){
        SetEnemyChoices();
        SetEnemySpritesLibrary();
        SetPresetIconSpritesLibrary();
        SetWaveChoices();
        SetPowerups();
        SetPowerupSpawnersChoices();
    }
#endregion
#region///Setting Preset, Loading and Saving
    public void SelectBuiltinPreset(string str){
        Transform builtInPresetsListTransform=builtInPresetsPanel.transform.GetChild(0).GetChild(0);
        if(_selectedDefPreset!=str){
            _selectedDefPreset=str;
            ResetBuiltinPresetButtonsColors();
            builtInPresetsListTransform.Find(str+"-PresetButton").GetComponent<Image>().color=selectedColor;
        }else{DeselectBuiltinPreset();}
    }
    void HighlightActiveBuiltinPreset(){
        Transform builtInPresetsListTransform=builtInPresetsPanel.transform.GetChild(0).GetChild(0);
        //ResetBuiltinPresetButtonsColors();
        if(String.IsNullOrEmpty(saveInfo.presetFrom)){saveInfo.presetFrom=defPresetGameruleset.cfgName;}
        builtInPresetsListTransform.Find(saveInfo.presetFrom+"-PresetButton").GetComponent<Image>().color=activeColor;
    }
    public void DeselectBuiltinPreset(){_selectedDefPreset="";ResetBuiltinPresetButtonsColors();}
    void ResetBuiltinPresetButtonsColors(){foreach(Button b in builtInPresetsPanel.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Button>()){b.GetComponent<Image>().color=defaultColor;}HighlightActiveBuiltinPreset();}
    public void SetDefPresetSelected(){if(_selectedDefPreset!=""){
        SetDefPresetGameruleset(_selectedDefPreset);
        SavePopup(_selectedDefPreset+" <color=yellow>SET AS DEFAULT PRESET</color>");
        _selectedDefPreset="";
        ResetBuiltinPresetButtonsColors();
        HighlightActiveBuiltinPreset();
    }}
    public void OpenLoadBuiltinPresetPrompt(){
        if(!String.IsNullOrEmpty(_selectedDefPreset)){
            loadDefaultPrompt.SetActive(true);
            loadDefaultPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=
            "Are you sure you want to reset your current save file to \u0022"+_selectedDefPreset
            +"\u0022 ?";
        }else{}
    }
    public void LoadBuiltinPresetSelected(){if(_selectedDefPreset!=""){
        StartCoroutine(LoadBuiltinPresetI(_selectedDefPreset));
        SavePopup(_selectedDefPreset+" <color=blue>LOADED</color>");
        _selectedDefPreset="";
    }}
    public void LoadBuiltinPreset(string str,bool savePopup=false){if(str!=""){
        StartCoroutine(LoadBuiltinPresetI(str));
        if(savePopup)SavePopup(str+" <color=blue>LOADED</color>");
    }}

    public IEnumerator LoadBuiltinPresetI(string str){
        if(GameRules.instance!=null)Destroy(GameRules.instance.gameObject);
        yield return new WaitForSecondsRealtime(0.02f);
        SetDefPresetGameruleset(str);
        var gr=Instantiate(defPresetGameruleset);
        gr.gameObject.name="GRSandbox";
        gr.cfgName="Sandbox Save #"+(savesCount+1);
        saveInfo.name="Sandbox Save #"+(savesCount+1);
        gr.cfgDesc="New Sandbox Mode Savefile";
        saveInfo.desc="New Sandbox Mode Savefile";
        gr.cfgIconAssetName="questionMark";
        gr.cfgIconsGo=null;
        CloseSavingPrompts();
        OpenDefaultPanel();
        SetupEverything();
    }
    void OnChangeAnything(){
        if(_initiated){
            if(saveInfo.saveBuild==0){saveInfo.saveBuild=1;}
            if(!_modified){_modified=true;}
        }
    }
    public void SetDefPresetGameruleset(string str){defPresetGameruleset=CoreSetup.instance.gamerulesetsPrefabs[GameManager.instance.GetGamemodeID(str)];saveInfo.presetFrom=str;}
    public void SelectPreset(string str){
        if(!String.IsNullOrEmpty(str)&&str!=saveSelected){
            if(String.IsNullOrEmpty(curSaveFileName)){curSaveFileName=saveSelected;}
            saveSelected=str;
            HighlightSelectedPresetDirect(str);
            /*if(String.IsNullOrEmpty(curSaveFileName)){
                if(saveSelected!=curSaveFileName){
                    curSaveFileName=saveSelected;
                }//else{_cachedSandboxName="";}
            }else{
                ResetYoursPresetButtonsColors();
                if(curSaveFileName==""){curSaveFileName=saveSelected;}
            }*/
            if(str!=curSaveFileName&&curSaveFileName!=""){ChangeSaveOverwriteBulb();}else{ChangeSaveOverwriteBulb(false);}
        }
        else if(str==saveSelected){DeselectPreset();}
        else if(str==curSaveFileName){saveSelected=curSaveFileName;HighlightSelectedPresetDirect(curSaveFileName);}
    }
    //public void DeselectPreset(){if(_cachedSandboxName!=""){_cachedSandboxName="";saveSelected=_cachedSandboxName;ResetYoursPresetButtonsColors();ChangeSaveOverwriteBulb();}else{}}
    public void DeselectPreset(){if(curSaveFileName!=""){saveSelected=curSaveFileName;ResetYoursPresetButtonsColors();ChangeSaveOverwriteBulb(false);}else{saveSelected=saveInfo.name;curSaveFileName=saveInfo.name;}}
    void ChangeSaveOverwriteBulb(bool on=true){
        Transform bulbs=yoursPresetsPanel.transform.GetChild(1);
        bulbs.GetChild(1).gameObject.SetActive(!on);
        bulbs.GetChild(2).gameObject.SetActive(on);
    }
    void ResetYoursPresetButtonsColors(){
        Transform yoursModesListTransform=yoursPresetsPanel.transform.GetChild(0).GetChild(0);
        foreach(Transform tt in yoursModesListTransform){tt.gameObject.GetComponent<Image>().color=defaultColor;}
        HighlightActivePreset();
    }
    void HighlightSelectedPreset(){
        Transform yoursModesListTransform=yoursPresetsPanel.transform.GetChild(0).GetChild(0);
        ResetYoursPresetButtonsColors();
        Transform t=yoursModesListTransform.Find(saveSelected+"-PresetButton");if(t!=null){GameObject goF=t.gameObject;goF.GetComponent<Image>().color=selectedColor;}
    }
    void HighlightActivePreset(){
        Transform yoursModesListTransform=yoursPresetsPanel.transform.GetChild(0).GetChild(0);
        //ResetYoursPresetButtonsColors();
        var _obj=yoursModesListTransform.Find(curSaveFileName+"-PresetButton");
        if(_obj!=null)_obj.GetComponent<Image>().color=activeColor;
    }
    void HighlightSelectedPresetDirect(string str){
        Transform yoursModesListTransform=yoursPresetsPanel.transform.GetChild(0).GetChild(0);
        ResetYoursPresetButtonsColors();
        Transform t=yoursModesListTransform.Find(str+"-PresetButton");if(t!=null){GameObject goF=t.gameObject;goF.GetComponent<Image>().color=selectedColor;}
    }
    public string _sandboxDataDir(){return Application.persistentDataPath+"/SandboxData";}
    public string _sandboxRecycleDir(){return _sandboxDataDir()+"/RecycleBin";}
    public string _sandboxSavesDir(){return _sandboxDataDir()+"/SandboxSaves";}
    public string _sandboxShipSkinsDir(){return _sandboxDataDir()+"/ShipSkins";}
    public string _currentSandboxFilePath(){return _sandboxSavesDir()+"/"+curSaveFileName+".json";}
    public string _selectedSandboxFilePath(){return _sandboxSavesDir()+"/"+saveSelected+".json";}
    public void OpenSaveSandboxPrompt(){
        if(ES3.FileExists(_selectedSandboxFilePath())){
            saveSandboxPrompt.SetActive(true);
            saveSandboxPrompt.transform.GetChild(0).gameObject.SetActive(true);
            saveSandboxPrompt.transform.GetChild(1).gameObject.SetActive(false);
            saveSandboxPrompt.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text=
            "Are you sure you want to save \n \u0022"+saveSelected
            +"\u0022, \n overwriting the old build?";
            if(curSaveFileName!=""&&curSaveFileName!=saveSelected){//Overwriting
                saveSandboxPrompt.transform.GetChild(0).gameObject.SetActive(false);
                saveSandboxPrompt.transform.GetChild(1).gameObject.SetActive(true);
                saveSandboxPrompt.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text=
                "Are you sure you want to overwrite \n"+
                "\u0022"+saveSelected+"\u0022 \n"+
                "with \n"+
                "\u0022"+curSaveFileName+"\u0022 ?";
            }
        }
        else{SaveSandbox();}
    }
    public void OpenLoadSandboxPrompt(){
        if(!String.IsNullOrEmpty(saveSelected)){
            if(ES3.FileExists(_selectedSandboxFilePath())){
                if(saveInfo.saveBuild!=0){
                    loadSandboxPrompt.SetActive(true);
                    loadSandboxPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=
                    "Are you sure you want to load \n \u0022"+saveSelected
                    +"\u0022, \nover your current save file?";
                }else{
                    LoadSandbox();
                }
            }else{
                Debug.LogWarning("No file at: "+_selectedSandboxFilePath());
                SavePopup("<color=orange> No file by name: </color>"+saveSelected);
            }
        }else{
            SavePopup("<color=orange> File name is empty </color>");
        }
    }
    public void OpenDeleteSandboxPrompt(){
        if(!String.IsNullOrEmpty(saveSelected)){
            if(ES3.FileExists(_selectedSandboxFilePath())){
                deleteSandboxPrompt.SetActive(true);
                deleteSandboxPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=
                "Are you sure you want to delete \n \u0022"+saveSelected+"\u0022 ?";
            }else{
                Debug.LogWarning("No file at: "+_selectedSandboxFilePath());
                SavePopup("<color=orange> No file by name: </color>"+saveSelected);
            }
        }else{
            SavePopup("<color=orange> File name is empty </color>");
        }
    }
    public void CloseSavingPrompts(){
        saveSandboxPrompt.SetActive(false);
        loadSandboxPrompt.SetActive(false);
        deleteSandboxPrompt.SetActive(false);
        quitSaveSandboxPrompt.SetActive(false);
        if(enterLoadSandboxPrompt.activeSelf){saveSelected=curSaveFileName;}
        enterLoadSandboxPrompt.SetActive(false);
        loadDefaultPrompt.SetActive(false);
    }
    public void OpenQuitSavePrompt(){
        quitSaveSandboxPrompt.SetActive(true);
    }
    public void QuitSave(){
        //CloseSavingPrompts();OpenSaveSandboxPrompt();
        StartCoroutine(QuitSaveI());
    }
    IEnumerator QuitSaveI(){
        SaveSandbox();
        yield return new WaitForSeconds(0.05f);
        QuitSandbox();
    }
    public void OpenEnterLoadPrompt(){
        enterLoadSandboxPrompt.SetActive(true);
        enterLoadSandboxPrompt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=
        "Do you want to load the last edited save ?\n"+
        "(\u0022"+saveSelected+"\u0022)";
    }
    //public void SaveSandboxOverwriteSaveInfo(bool b){overwriteSaveInfo=b;}
    public void SaveSandbox(bool overwriteSaveInfo=false){
        bool _overwriting=false;
        if(String.IsNullOrEmpty(saveSelected)){if(String.IsNullOrEmpty(curSaveFileName)){saveSelected=curSaveFileName;}else{SavePopup("<color=orange> File name is empty </color>");}}
        if(!String.IsNullOrEmpty(saveSelected)){
            if(GameRules.instance!=null){
                if(!ES3.FileExists(_selectedSandboxFilePath())){
                    AddNewYoursPresetButton(saveSelected+".json",GameRules.instance);
                    HighlightSelectedPreset();
                    SavePopup("\u0022"+saveSelected+"\u0022 <color=green> CREATED </color>");
                    if(curSaveFileName==""){curSaveFileName=saveSelected;}
                }else{
                    if(curSaveFileName!=""&&curSaveFileName!=saveSelected){
                        _overwriting=true;
                        SavePopup("\u0022"+saveSelected+"\u0022 <color=yellow> OVERRITEN WITH </color> "+curSaveFileName,1.3f);
                        //saveSelected=curSaveFileName;
                        //saveInfo.name=curSaveFileName;
                        //_cachedSandboxName="";
                    }else{
                        SavePopup("\u0022"+saveSelected+"\u0022 <color=green> SAVED </color>");
                        HighlightSelectedPreset();
                    }
                    SetYoursPresetsButtonsDelay(0.02f);
                }
                if(_overwriting&&!overwriteSaveInfo){//If not overwriting then load the saveInfo back
                    if(ES3.FileExists(_selectedSandboxFilePath())){if(ES3.KeyExists("saveInfo",_selectedSandboxFilePath())){
                        saveInfo=ES3.Load<SandboxSaveInfo>("saveInfo",_selectedSandboxFilePath());}}
                }

                //var settings=new ES3Settings(_selectedSandboxFilePath());
                //settings.referenceMode=ES3.ReferenceMode.ByValue;
                var settings=new ES3Settings(_selectedSandboxFilePath(),ES3.Location.Cache);
                //SandboxSaveInfo _saveInfo=new SandboxSaveInfo();
                if(!ES3.KeyExists("saveInfo",_selectedSandboxFilePath())){
                    saveInfo.author=SaveSerial.instance.hyperGamerLoginData.username;
                    saveInfo.saveBuild=1;
                }
                else{
                    if(saveInfo.saveBuild>0)saveInfo.saveBuild++;
                    if(defPresetGameruleset.cfgName!=ES3.Load<SandboxSaveInfo>("saveInfo",_selectedSandboxFilePath()).presetFrom){saveInfo.presetFrom=defPresetGameruleset.cfgName;}
                }
                saveInfo.gameBuild=GameManager.instance.buildVersion;
                //saveInfo.author=SaveSerial.instance.hyperGamerLoginData.username;//Make it set the unique ID later on

                /*if(!_overwriting||(_overwriting&&overwriteSaveInfo)){
                    //var _saveInfo=saveInfo;
                    if(_overwriting&&!overwriteSaveInfo){if(ES3.FileExists(_selectedSandboxFilePath())){saveInfo=ES3.Load<SandboxSaveInfo>("saveInfo",_selectedSandboxFilePath());}}//If not overwriting then load the saveInfo back
                    ES3.Save("saveInfo",saveInfo,settings);
                }*/
                ES3.Save("saveFormat",sandboxSaveFormat,settings);
                ES3.Save("saveInfo",saveInfo,settings);
                ES3.Save("gamerulesData",GameRules.instance,settings);
                ES3.StoreCachedFile(_selectedSandboxFilePath());
                _modified=false;
                if(_overwriting)saveSelected=curSaveFileName;
            }else{
                SavePopup("<color=red> GameRules.instance is null! </color>",1.5f);
            }
        }
        CloseSavingPrompts();
    }
    public void LoadSandbox(){
        if(!String.IsNullOrEmpty(saveSelected)){
            if(ES3.FileExists(_selectedSandboxFilePath())){
                if(ES3.KeyExists("gamerulesData",_selectedSandboxFilePath())){
                    ES3.LoadInto<GameRules>("gamerulesData",_selectedSandboxFilePath(),GameRules.instance);
                    if(ES3.KeyExists("saveInfo",_selectedSandboxFilePath())){
                        ES3.LoadInto("saveInfo",_selectedSandboxFilePath(),saveInfo);
                        //GameRules.instance.cfgName=saveInfo.name;
                        //GameRules.instance.cfgDesc=saveInfo.desc;
                        //GameRules.instance.cfgIconAssetName=saveInfo.iconAssetName;
                        //saveInfo.iconShaderMatProps=saveInfo.iconShaderMatProps;
                        SetDefPresetGameruleset(saveInfo.presetFrom);
                    }else{
                        saveInfo.name=GameRules.instance.cfgName;
                        saveInfo.saveBuild=1;
                        saveInfo.gameBuild=GameManager.instance.buildVersion;
                        SetDefPresetGameruleset("Arcade Mode");
                    }
                    SavePopup("\u0022"+saveSelected+"\u0022 <color=blue> LOADED </color>");
                    ChangeSaveOverwriteBulb(false);
                    curSaveFileName=saveSelected;//saveSelected="";
                    //_cachedSandboxName="";
                }
                else{
                    Debug.LogWarning("No key by "+"gamerulesData"+"for: "+_selectedSandboxFilePath());
                    SavePopup("<color=orange> No key by "+"\u0022sandboxData\u0022"+" for: \u0022"+saveSelected+"\u0022</color>");
                }
            }else{
                Debug.LogWarning("No file at: "+_selectedSandboxFilePath());
                SavePopup("<color=orange> No file by name: </color>"+saveSelected);
            }
        }else{
            SavePopup("<color=orange> File name is empty </color>");
        }
        CloseSavingPrompts();
        SetPresetIconPreviewsSprite();
    }
    public void DeleteSandbox(bool deletePermanently=false){
        if(!String.IsNullOrEmpty(saveSelected)){
            if(ES3.FileExists(_selectedSandboxFilePath())){
                /*if(Application.platform==RuntimePlatform.WindowsPlayer||Application.platform==RuntimePlatform.WindowsEditor){
                    SavePopup("\u0022"+saveSelected+"\u0022 <color=orange> MOVED TO RECYCLE BIN </color>");
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(_selectedSandboxFilePath(),Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                }else{
                    SavePopup("\u0022"+saveSelected+"\u0022 <color=orange> MOVED TO RECYCLE IN SANDBOX DIR</color>");
                    System.IO.Directory.CreateDirectory(_sandboxRecycleDir());
                    System.IO.File.Move(_selectedSandboxFilePath(),(_sandboxRecycleDir()+"/"+saveSelected+".json"));
                }*/
                if(deletePermanently){
                    ES3.DeleteFile(_selectedSandboxFilePath());
                    SavePopup("\u0022"+saveSelected+"\u0022 <color=red> Removed Permanently</color>",2f);
                }else{
                    SavePopup("\u0022"+saveSelected+"\u0022 <color=red> MOVED TO RecycleBin IN SandboxData</color>",2f);
                    if(!System.IO.Directory.Exists(_sandboxRecycleDir()))System.IO.Directory.CreateDirectory(_sandboxRecycleDir());
                    var _fname=saveSelected;
                    while(System.IO.File.Exists(_sandboxRecycleDir()+"/"+_fname+".json")){_fname+="_";}
                    System.IO.File.Move(_selectedSandboxFilePath(),(_sandboxRecycleDir()+"/"+_fname+".json"));
                }
                if(!String.IsNullOrEmpty(curSaveFileName))saveSelected=curSaveFileName;
                SetYoursPresetsButtons();
                ChangeSaveOverwriteBulb(false);
            }else{
                SavePopup("<color=orange> No file by name: </color>"+saveSelected);
            }
        }else{
            SavePopup("<color=orange> File name is empty </color>");
        }
        CloseSavingPrompts();
    }
    public void BrowseSandboxData(){Application.OpenURL("file:///"+_sandboxDataDir());}
    public void BrowseSandboxSaves(){Application.OpenURL("file:///"+_sandboxSavesDir());}
#endregion

#region///Preset Display
    public void SetSandboxName(string v){saveSelected=v;saveInfo.name=v;OnChangeAnything();}//GameRules.instance.cfgName=v;}//sandboxName=v;}
    public void SetSandboxDesc(string v){saveInfo.desc=v;OnChangeAnything();}//GameRules.instance.cfgName=v;}//sandboxName=v;}
    public void SetPresetIcon(string v){saveInfo.iconAssetName=v;OpenPresetAppearanceIconPanel();OnChangeAnything();}
    public void SetPresetIconSprMatHue(float v){saveInfo.iconShaderMatProps.hue=(float)Math.Round(v,2);UpdateIconSprMat();OnChangeAnything();}
    public void SetPresetIconSprMatSatur(float v){saveInfo.iconShaderMatProps.saturation=(float)Math.Round(v,2);UpdateIconSprMat();OnChangeAnything();}
    public void SetPresetIconSprMatValue(float v){saveInfo.iconShaderMatProps.value=(float)Math.Round(v,2);UpdateIconSprMat();OnChangeAnything();}
    public void SetPresetIconSprMatNegative(float v){saveInfo.iconShaderMatProps.negative=(float)Math.Round(v,2);UpdateIconSprMat();OnChangeAnything();}
    public void SetPresetIconSprMatPixelate(float v){saveInfo.iconShaderMatProps.pixelate=Mathf.Clamp((float)Math.Round(v,2),(4/512),1);UpdateIconSprMat();OnChangeAnything();}
    public void SetPresetIconSprMatBlur(float v){saveInfo.iconShaderMatProps.blur=(float)Math.Round(v,2);UpdateIconSprMat();OnChangeAnything();}
    void UpdateIconSprMat(){SetPresetIconPreviewsSprite();}
    public void ResetIconShaderMat(){
        saveInfo.iconShaderMatProps=new ShaderMatProps();
        UpdateIconSprMat();
    }
#endregion
#region///Global
    public void SetGameSpeed(float v){GameRules.instance.defaultGameSpeed=(float)System.Math.Round(v,2);OnChangeAnything();}
    public void SetScoreDisplay(){scoreDisplay s=GameRules.instance.scoreDisplay;
        switch(s){
            case scoreDisplay.sessionTime: s=scoreDisplay.score;break;
            case scoreDisplay.score: s=scoreDisplay.sessionTime;break;
        }
        GameRules.instance.scoreDisplay=s;
        OnChangeAnything();
    }
    public void SetCrystalsOn(bool v){GameRules.instance.crystalsOn=v;OnChangeAnything();}
    public void SetXpOn(bool v){GameRules.instance.xpOn=v;OnChangeAnything();}
    public void SetCoresOn(bool v){GameRules.instance.coresOn=v;OnChangeAnything();}
    public void SetLevelingOn(bool v){GameRules.instance.levelingOn=v;OnChangeAnything();}
    public void SetShopOn(bool v){GameRules.instance.shopOn=v;OnChangeAnything();}
    public void SetShopCargoOn(bool v){GameRules.instance.shopCargoOn=v;OnChangeAnything();}
    public void SetModulesOn(bool v){GameRules.instance.modulesOn=v;OnChangeAnything();}
    public void SetBarrierOn(bool v){GameRules.instance.barrierOn=v;OnChangeAnything();}
    public void ResetGlobal(){
        GameRules.instance.defaultGameSpeed=defPresetGameruleset.defaultGameSpeed;
        GameRules.instance.scoreDisplay=defPresetGameruleset.scoreDisplay;
        GameRules.instance.crystalsOn=defPresetGameruleset.crystalsOn;
        GameRules.instance.xpOn=defPresetGameruleset.xpOn;
        GameRules.instance.coresOn=defPresetGameruleset.coresOn;
        GameRules.instance.levelingOn=defPresetGameruleset.levelingOn;
        GameRules.instance.shopOn=defPresetGameruleset.shopOn;
        GameRules.instance.shopCargoOn=defPresetGameruleset.shopCargoOn;
        GameRules.instance.modulesOn=defPresetGameruleset.modulesOn;
        GameRules.instance.barrierOn=defPresetGameruleset.barrierOn;
        OnChangeAnything();
    }
    public void SetShopScoreRangeStart(string v){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Shop spawns are not set by score!");GameRules.instance.shopSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        OnChangeAnything();
    }
    public void SetShopScoreRangeEnd(string v){if(GameRules.instance.shopSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.shopSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Shop spawns are not set by score!");GameRules.instance.shopSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        OnChangeAnything();
    }
    public void SetBackgroundHue(float v){GameRules.instance.bgMaterial.hue=(float)Math.Round(v,2);UpdateBgMaterial();OnChangeAnything();}
    public void SetBackgroundSatur(float v){GameRules.instance.bgMaterial.saturation=(float)Math.Round(v,2);UpdateBgMaterial();OnChangeAnything();}
    public void SetBackgroundValue(float v){GameRules.instance.bgMaterial.value=(float)Math.Round(v,2);UpdateBgMaterial();OnChangeAnything();}
    public void SetBackgroundNegative(float v){GameRules.instance.bgMaterial.negative=(float)Math.Round(v,2);UpdateBgMaterial();OnChangeAnything();}
    public void ResetBackgroundMaterial(){
        AssetsManager.ReplaceShaderMat(ref GameRules.instance.bgMaterial,defPresetGameruleset.bgMaterial);
        UpdateBgMaterial();OnChangeAnything();
    }
    void UpdateBgMaterial(){
        GameRules.instance.bgMaterial.text=FindObjectOfType<BGManager>().GetBgTexture();
        //Material _mat=new Material(Resources.Load("AllIn1SpriteShader", typeof(Shader)) as Shader);
        //_mat=AssetsManager.instance.UpdateShaderMatProps(FindObjectOfType<BGManager>().GetBgMat(),GameRules.instance.bgMaterial);
    }
#endregion
#region///Player
    //Player Sprite
    public void SetPlayerSprite(string v){}//GameRules.instance.playerSprite=playerSprites.Find(x=>x.name==v).spr;OpenPlayerSpritePanel();}
    public void SetPlayerSprMatHue(float v){GameRules.instance.playerShaderMatProps.hue=(float)Math.Round(v,2);UpdatePlayerSprMat();OnChangeAnything();}
    public void SetPlayerSprMatSatur(float v){GameRules.instance.playerShaderMatProps.saturation=(float)Math.Round(v,2);UpdatePlayerSprMat();OnChangeAnything();}
    public void SetPlayerSprMatValue(float v){GameRules.instance.playerShaderMatProps.value=(float)Math.Round(v,2);UpdatePlayerSprMat();OnChangeAnything();}
    public void SetPlayerSprMatNegative(float v){GameRules.instance.playerShaderMatProps.negative=(float)Math.Round(v,2);UpdatePlayerSprMat();OnChangeAnything();}
    public void SetPlayerSprMatPixelate(float v){GameRules.instance.playerShaderMatProps.pixelate=Mathf.Clamp((float)Math.Round(v,2),(4/512),1);UpdatePlayerSprMat();OnChangeAnything();}
    public void SetPlayerSprMatBlur(float v){GameRules.instance.playerShaderMatProps.blur=(float)Math.Round(v,2);UpdatePlayerSprMat();OnChangeAnything();}
    void UpdatePlayerSprMat(){SetPlayerPreviewsSprite();}

    public void SetHealth(string v){GameRules.instance.healthPlayer=float.Parse(v);
        if(GameRules.instance.healthPlayer>GameRules.instance.healthMaxPlayer){GameRules.instance.healthMaxPlayer=GameRules.instance.healthPlayer;}
        OnChangeAnything();
    }
    public void SetHealthMax(string v){GameRules.instance.healthMaxPlayer=float.Parse(v);OnChangeAnything();}
    public void SetDefense(string v){GameRules.instance.defensePlayer=int.Parse(v);OnChangeAnything();}
    public void SetEnergy(string v){GameRules.instance.energyPlayer=float.Parse(v);
        if(GameRules.instance.energyPlayer>GameRules.instance.energyMaxPlayer){GameRules.instance.energyMaxPlayer=GameRules.instance.energyPlayer;}
        if(GameRules.instance.energyMaxPlayer==0){GameRules.instance.energyOnPlayer=false;}else{GameRules.instance.energyOnPlayer=true;}
        OnChangeAnything();
    }
    public void SetEnergyMax(string v){GameRules.instance.energyMaxPlayer=float.Parse(v);
        if(GameRules.instance.energyMaxPlayer==0){GameRules.instance.energyOnPlayer=false;}else{GameRules.instance.energyOnPlayer=true;}
        OnChangeAnything();
    }
    public void ResetPlayerMain1(){
        GameRules.instance.healthPlayer=defPresetGameruleset.healthPlayer;
        GameRules.instance.healthMaxPlayer=defPresetGameruleset.healthMaxPlayer;
        GameRules.instance.defensePlayer=defPresetGameruleset.defensePlayer;
        GameRules.instance.energyPlayer=defPresetGameruleset.energyPlayer;
        GameRules.instance.energyMaxPlayer=defPresetGameruleset.energyMaxPlayer;
        if(GameRules.instance.energyMaxPlayer==0){GameRules.instance.energyOnPlayer=false;}else{GameRules.instance.energyOnPlayer=true;}
        OnChangeAnything();
    }

    public void SetMoveAxis(){
        var i=GameRules.instance;
        if(i.moveX&&!i.moveY){i.moveX=false;i.moveY=true;}
        else if(!i.moveX&&i.moveY){i.moveX=true;i.moveY=true;}
        else if(i.moveX&&i.moveY){i.moveX=true;i.moveY=false;}
        OnChangeAnything();
    }
    public void SetPlayerSpeed(string v){GameRules.instance.moveSpeedPlayer=float.Parse(v);OnChangeAnything();}
    public void SetPlayerStartPosX(string v){GameRules.instance.startingPosPlayer.x=float.Parse(v);OnChangeAnything();}
    public void SetPlayerStartPosY(string v){GameRules.instance.startingPosPlayer.y=float.Parse(v);OnChangeAnything();}
    public void SetPlayerDefaultScale(string v){GameRules.instance.shipScaleDefault=float.Parse(v);OnChangeAnything();}
    public void ResetPlayerMain2(){
        GameRules.instance.moveX=defPresetGameruleset.moveX;
        GameRules.instance.moveY=defPresetGameruleset.moveY;
        GameRules.instance.moveSpeedPlayer=defPresetGameruleset.moveSpeedPlayer;
        GameRules.instance.startingPosPlayer=defPresetGameruleset.startingPosPlayer;
        GameRules.instance.shipScaleDefault=defPresetGameruleset.shipScaleDefault;
        OnChangeAnything();
    }
    public void SetPowerupsCapacity(float v){GameRules.instance.powerupsCapacity=(int)v;SetPowerups();OnChangeAnything();}
    public void SetAutoshoot(bool v){GameRules.instance.autoShootPlayer=v;OnChangeAnything();}
    public void OpenStartingPowerupChoices(int id){
        if(id!=powerupToSet){
            startingPowerupChoices.SetActive(true);
            var powerupInventorySelectedPos=powerupInventory.transform.GetChild(0).GetChild(id).position;
            startingPowerupChoices.transform.position=new Vector2(powerupInventorySelectedPos.x+50f,powerupInventorySelectedPos.y+105f);
            powerupToSet=id;
        }else{CloseStartingPowerupChoices();}
    }
    public void CloseStartingPowerupChoices(){startingPowerupChoices.SetActive(false);powerupToSet=-1;}
    public void SetPowerupStarting(string v){
        if(GameRules.instance.powerupsStarting.Count<=powerupToSet){for(var i=GameRules.instance.powerupsStarting.Count;i<=powerupToSet;i++){
            GameRules.instance.powerupsStarting.Add(new Powerup());}}
        if(GameRules.instance.powerupsStarting[powerupToSet]==null){GameRules.instance.powerupsStarting[powerupToSet]=new Powerup();}
        else{GameRules.instance.powerupsStarting[powerupToSet].name=v;}
        startingPowerupChoices.SetActive(false);
        SetPowerups();
        OnChangeAnything();
    }
    public void ResetPlayerMain3(){
        GameRules.instance.autoShootPlayer=defPresetGameruleset.autoShootPlayer;
        GameRules.instance.powerupsCapacity=defPresetGameruleset.powerupsCapacity;
        GameRules.instance.powerupsStarting.Clear();
        foreach(Powerup p in defPresetGameruleset.powerupsStarting){GameRules.instance.powerupsStarting.Add(p);}
        SetPowerups();
        OnChangeAnything();
    }
#endregion
#region///Enemy / Enemies
    #region///returns
    bool _canModifySpriteEn(){bool b=true;string s=enemyToModify;
        /*if(false
        ){
            b=false;}*/
    return b;}
    bool _canChangeSpriteEn(){bool b=true;string s=enemyToModify;
        if(s=="Vortex Wheel"
        ||s=="Comet"
        ){
            b=false;}
    return b;}
    public EnemyClass _enGR(string str, GameRules gr){EnemyClass _en=null;if(!String.IsNullOrEmpty(str)){_en=System.Array.Find(gr.enemies,x=>x.name==str);}return _en;}
    public EnemyClass _en(string str){return _enGR(str,GameRules.instance);}
    public EnemyClass _enMod(){return _en(enemyToModify);}
    public EnemyClass _enModDef(){return _enGR(enemyToModify,defPresetGameruleset);}

    public Sprite _enSprGR(string str,GameRules gr){Sprite _spr=null;
        if(_enGR(str,gr)!=null){
            if(!String.IsNullOrEmpty(_enGR(str,gr).sprAsset)&&str!="Comet"){_spr=AssetsManager.instance.SprAnyReverse(_enGR(str,gr).sprAsset);}
            //else{
                else if(str=="Comet"){_spr=AssetsManager.instance.SprAnyReverse(GameRules.instance.cometSettings.sprites[0]);}
            //}
            if(_spr!=null)return _spr;
            else{Debug.LogWarning("No spr for: "+str);return null;}
        }else{Debug.LogWarning("No enemy by name: "+str);return null;}
    }
    public string _enSprStrGR(string str,GameRules gr){string _spr="";
        if(_enGR(str,gr)!=null){
            if(!String.IsNullOrEmpty(_enGR(str,gr).sprAsset)){_spr=_enGR(str,gr).sprAsset;}
            /*else{
                if(str=="Comet"){_spr=GameRules.instance.cometSettings.sprites[0];}
            }*/
            else{Debug.LogWarning("No spr for: "+str);}
        }else{Debug.LogWarning("No enemy by name: "+str);}
        return _spr;
    }
    public Sprite _enSpr(string str){return _enSprGR(str,GameRules.instance);}
    public string _enSprStr(string str){return _enSprStrGR(str,GameRules.instance);}
    public Sprite _enModSpr(){return _enSpr(enemyToModify);}
    public string _enModSprStr(){return _enSprStr(enemyToModify);}
    public Sprite _enModSprDef(){return _enSprGR(enemyToModify,defPresetGameruleset);}
    public string _enModSprStrDef(){return _enSprStrGR(enemyToModify,defPresetGameruleset);}
    public ShaderMatProps _enSprMatGR(string str, GameRules gr){
        if(_enGR(str,gr)!=null){return _enGR(str,gr).sprMatProps;}
        else return null;
    }
    public ShaderMatProps _enSprMat(string str){return _enSprMatGR(str,GameRules.instance);}
    public ShaderMatProps _enModSprMat(){return _enSprMat(enemyToModify);}
    public ShaderMatProps _enModSprMatDef(){return _enSprMatGR(enemyToModify,defPresetGameruleset);}
    #endregion///returns

    //Enemy Main Settings
    public void SetEnemyType(int v){_enMod().type=(enemyType)v;}
    public void SetEnemyHealth(string v){_enMod().healthStart=float.Parse(v);OnChangeAnything();}
    public void SetEnemyHealthMax(string v){_enMod().healthMax=float.Parse(v);OnChangeAnything();}
    public void SetEnemyDefense(string v){_enMod().defense=int.Parse(v);OnChangeAnything();}
    public void ResetEnemyMain1(){
        _enMod().healthStart=_enModDef().healthStart;
        _enMod().healthMax=_enModDef().healthMax;
        _enMod().defense=_enModDef().defense;
        _enMod().type=_enModDef().type;
    }

    public void SetEnemyScoreStart(string v){_enMod().scoreValue=new Vector2(float.Parse(v),_enMod().scoreValue.y);OnChangeAnything();}
    public void SetEnemyScoreEnd(string v){_enMod().scoreValue=new Vector2(_enMod().scoreValue.x,float.Parse(v));OnChangeAnything();}
    public void ResetEnemyMain2(){
        _enMod().scoreValue=_enModDef().scoreValue;
    }

    ///Enemy Sprite
    public void SetEnemySprite(string v){_enMod().sprAsset=enemySprites.Find(x=>x.name==v).name;OpenEnemySpritePanel();}
    public void SetEnemySprMatHue(float v){if(_enModSprMat()!=null)_enModSprMat().hue=(float)Math.Round(v,2);UpdateEnModSprMat();OnChangeAnything();}
    public void SetEnemySprMatSatur(float v){if(_enModSprMat()!=null)_enModSprMat().saturation=(float)Math.Round(v,2);UpdateEnModSprMat();OnChangeAnything();}
    public void SetEnemySprMatValue(float v){if(_enModSprMat()!=null)_enModSprMat().value=(float)Math.Round(v,2);UpdateEnModSprMat();OnChangeAnything();}
    public void SetEnemySprMatNegative(float v){if(_enModSprMat()!=null)_enModSprMat().negative=(float)Math.Round(v,2);UpdateEnModSprMat();OnChangeAnything();}
    public void SetEnemySprMatPixelate(float v){if(_enModSprMat()!=null)_enModSprMat().pixelate=Mathf.Clamp((float)Math.Round(v,2),(4/512),1);UpdateEnModSprMat();OnChangeAnything();}
    public void SetEnemySprMatBlur(float v){if(_enModSprMat()!=null)_enModSprMat().blur=(float)Math.Round(v,2);UpdateEnModSprMat();OnChangeAnything();}
    void UpdateEnModSprMat(){SetEnemyPreviewsSprite();}
    public void ResetEnemySprite(){
        _enMod().sprAsset=_enModSprStrDef();
        AssetsManager.ReplaceShaderMat(ref _enMod().sprMatProps,_enModSprMatDef());
        UpdateEnModSprMat();
    }
#endregion
#region///Waves
    public void SetWaveSpawnReqsType(int v){    spawnReqsType srt=0;string t=wavesSpawnReqsTypes[v];
        switch(t){
            case "Time":srt=spawnReqsType.time;break;
            case "Kills":srt=spawnReqsType.kills;break;
            default:srt=spawnReqsType.score;break;
        }
        GameRules.instance.waveSpawnReqsType=srt;
        spawnReqsMono.Validate(ref GameRules.instance.waveSpawnReqs, ref srt);
        OpenWavesSpawnReqsInputs();
        OnChangeAnything();
    }
    public void SetWaveScoreRangeStart(string v){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by score!");GameRules.instance.waveSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        OnChangeAnything();
    }
    public void SetWaveScoreRangeEnd(string v){if(GameRules.instance.waveSpawnReqs is spawnScore){var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by score!");GameRules.instance.waveSpawnReqsType=spawnReqsType.score;GameRules.instance.waveSpawnReqs=new spawnScore();var sr=(spawnScore)GameRules.instance.waveSpawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        OnChangeAnything();
    }
    public void SetWaveTimeRangeStart(string v){if(GameRules.instance.waveSpawnReqs is spawnReqs&&!GameRules.instance.waveSpawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=GameRules.instance.waveSpawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Wave spawns are not set by time!");GameRules.instance.waveSpawnReqsType=spawnReqsType.time;GameRules.instance.waveSpawnReqs=new spawnReqs();var sr=GameRules.instance.waveSpawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}
        OnChangeAnything();
    }
    public void SetWaveTimeRangeEnd(string v){if(GameRules.instance.waveSpawnReqs is spawnReqs&&!GameRules.instance.waveSpawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=GameRules.instance.waveSpawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Wave spawns are not set by time!");GameRules.instance.waveSpawnReqsType=spawnReqsType.time;GameRules.instance.waveSpawnReqs=new spawnReqs();var sr=GameRules.instance.waveSpawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}
        OnChangeAnything();
    }
    public void SetWaveKillsNeeded(string v){if(GameRules.instance.waveSpawnReqs is spawnKills){var sr=(spawnKills)GameRules.instance.waveSpawnReqs;sr.killsNeeded=int.Parse(v);}
        else{Debug.LogWarning("Wave spawns are not set by kills!");GameRules.instance.waveSpawnReqsType=spawnReqsType.kills;GameRules.instance.waveSpawnReqs=new spawnKills();var sr=(spawnKills)GameRules.instance.waveSpawnReqs;sr.killsNeeded=int.Parse(v);}
        OnChangeAnything();
    }
    public void ResetWaveSpawns1(){
        GameRules.instance.waveSpawnReqsType=defPresetGameruleset.waveSpawnReqsType;
        GameRules.instance.waveSpawnReqs=defPresetGameruleset.waveSpawnReqs;
        spawnReqsMono.Validate(ref GameRules.instance.waveSpawnReqs, ref GameRules.instance.waveSpawnReqsType);
        OnChangeAnything();
    }

    public void SetWaveWeight(string n, string v){
        LootTableEntryWaves e=GameRules.instance.waveList.Find(x=>x.lootItem.name==n);
        e.dropChance=(float)System.Math.Round(float.Parse(v),2);
        GameRules.instance.SumUpWavesWeightsTotal();
        OnChangeAnything();
    }
    public void SetWaveLvlReq(string n, string v){
        LootTableEntryWaves e=GameRules.instance.waveList.Find(x=>x.lootItem.name==n);
        e.levelReq=int.Parse(v);
        OnChangeAnything();
    }
    public void SetWaveStarting(string n){
        GameRules.instance.startingWave=GameRules.instance.waveList.FindIndex(x=>x.lootItem.name==n);
        GameRules.instance.startingWaveRandom=false;
        OnChangeAnything();
    }
    public void SetWaveStartingRandom(bool v){GameRules.instance.startingWaveRandom=v;OnChangeAnything();}
    public void ResetWaveSpawns2(){
        GameRules.instance.startingWave=defPresetGameruleset.startingWave;
        GameRules.instance.startingWaveRandom=defPresetGameruleset.startingWaveRandom;
        foreach(LootTableEntryWaves e in GameRules.instance.waveList){
            LootTableEntryWaves ed=defPresetGameruleset.waveList.Find(x=>x.lootItem.name==e.lootItem.name);
            e.dropChance=ed.dropChance;
            e.levelReq=ed.levelReq;
        }
        SetWaveChoices();
        OnChangeAnything();
    }
#endregion
#region///Collectibles
    public void SetEnergyGain_EnergyBall(string v){GameRules.instance.energyBall_energyGain=float.Parse(v);OnChangeAnything();}
    public void SetEnergyGain_Battery(string v){GameRules.instance.battery_energyGain=float.Parse(v);OnChangeAnything();}
    public void SetBlackEnergyGain_Ball(string v){GameRules.instance.benergyBallGain=float.Parse(v);OnChangeAnything();}
    public void SetBlackEnergyGain_Vial(string v){GameRules.instance.benergyVialGain=float.Parse(v);OnChangeAnything();}
    public void SetCrystalSmallGain(string v){GameRules.instance.crystalGain=int.Parse(v);OnChangeAnything();}
    public void SetCrystalBigGain(string v){GameRules.instance.crystalBigGain=int.Parse(v);OnChangeAnything();}
    public void SetHpGain_Medkit(string v){GameRules.instance.medkit_hpGain=float.Parse(v);OnChangeAnything();}
    public void SetEnergyGain_Medkit(string v){GameRules.instance.medkit_energyGain=float.Parse(v);OnChangeAnything();}
    public void SetHpGain_LunarGel(string v){GameRules.instance.lunarGel_hpGain=float.Parse(v);OnChangeAnything();}
    public void SetEnergyGain_Powerups(string v){GameRules.instance.powerups_energyGain=float.Parse(v);OnChangeAnything();}
    public void SetEnergyNeeded_Powerups(string v){GameRules.instance.powerups_energyNeeded=float.Parse(v);OnChangeAnything();}
    public void ResetBasicCollectibles(){
        GameRules.instance.energyBall_energyGain=defPresetGameruleset.energyBall_energyGain;
        GameRules.instance.battery_energyGain=defPresetGameruleset.battery_energyGain;
        GameRules.instance.benergyBallGain=defPresetGameruleset.benergyBallGain;
        GameRules.instance.benergyVialGain=defPresetGameruleset.benergyVialGain;
        GameRules.instance.crystalGain=defPresetGameruleset.crystalGain;
        GameRules.instance.crystalBigGain=defPresetGameruleset.crystalBigGain;
        GameRules.instance.medkit_hpGain=defPresetGameruleset.medkit_hpGain;
        GameRules.instance.medkit_energyGain=defPresetGameruleset.medkit_energyGain;
        GameRules.instance.lunarGel_hpGain=defPresetGameruleset.lunarGel_hpGain;
        GameRules.instance.powerups_energyGain=defPresetGameruleset.powerups_energyGain;
        GameRules.instance.powerups_energyNeeded=defPresetGameruleset.powerups_energyNeeded;
        OnChangeAnything();
    }
    ///Powerups
    #region//returns
    public PowerupsSpawnerGR _pwrSpawnGR(string str, GameRules gr){PowerupsSpawnerGR _pwSp=null;if(!String.IsNullOrEmpty(str)){_pwSp=gr.powerupSpawners.Find(x=>x.name==str);}return _pwSp;}
    public PowerupsSpawnerGR _pwrSpawn(string str){return _pwrSpawnGR(str,GameRules.instance);}
    public PowerupsSpawnerGR _pwrSpawnMod(){return _pwrSpawn(powerupSpawnerToModify);}
    public PowerupsSpawnerGR _pwrSpawnModDef(){return _pwrSpawnGR(powerupSpawnerToModify,defPresetGameruleset);}
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
        OnChangeAnything();
    }
    public void SetPowerupSpawnerName(string v){
        Transform powerupsSpawnerListTransform=collectiblesMainPanel.transform.GetChild(1).GetChild(0);powerupsSpawnerListTransform.GetComponent<ContentSizeFitter>().enabled=true;
        Transform t=powerupsSpawnerListTransform.Find(powerupSpawnerToModify);GameObject go=t.gameObject;

        _pwrSpawnMod().name=v;
        powerupSpawnerToModify=v;
        
        if(go!=null){go.name=v;
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=v;}
        OnChangeAnything();
    }
    public void SetPowerupSpawnerScoreRangeStart(string v){if(_pwrSpawnMod().spawnReqs is spawnScore){var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        else{Debug.LogWarning("Powerup spawns are not set by score!");_pwrSpawnMod().spawnReqsType=spawnReqsType.score;_pwrSpawnMod().spawnReqs=new spawnScore();var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.x=int.Parse(v);}
        OnChangeAnything();
    }
    public void SetPowerupSpawnerScoreRangeEnd(string v){if(_pwrSpawnMod().spawnReqs is spawnScore){var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        else{Debug.LogWarning("Powerup spawns are not set by score!");_pwrSpawnMod().spawnReqsType=spawnReqsType.score;_pwrSpawnMod().spawnReqs=new spawnScore();var sr=(spawnScore)_pwrSpawnMod().spawnReqs;sr.scoreMaxSetRange.y=int.Parse(v);}
        OnChangeAnything();
    }
    public void SetPowerupSpawnerTimeRangeStart(string v){if(_pwrSpawnMod().spawnReqs is spawnReqs&&!_pwrSpawnMod().spawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=_pwrSpawnMod().spawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Powerup spawns are not set by time!");_pwrSpawnMod().spawnReqsType=spawnReqsType.time;_pwrSpawnMod().spawnReqs=new spawnReqs();var sr=_pwrSpawnMod().spawnReqs;sr.time.x=(float)System.Math.Round(float.Parse(v),3);}
        OnChangeAnything();
    }
    public void SetPowerupSpawnerTimeRangeEnd(string v){if(_pwrSpawnMod().spawnReqs is spawnReqs&&!_pwrSpawnMod().spawnReqs.GetType().IsSubclassOf(typeof(spawnReqs))){var sr=_pwrSpawnMod().spawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}
        else{Debug.LogWarning("Powerup spawns are not set by time!");_pwrSpawnMod().spawnReqsType=spawnReqsType.time;_pwrSpawnMod().spawnReqs=new spawnReqs();var sr=_pwrSpawnMod().spawnReqs;sr.time.y=(float)System.Math.Round(float.Parse(v),3);}
        OnChangeAnything();
    }
    public void SetPowerupSpawnerKillsNeeded(string v){if(_pwrSpawnMod().spawnReqs is spawnKills){var sr=(spawnKills)_pwrSpawnMod().spawnReqs;sr.killsNeeded=int.Parse(v);}
        else{Debug.LogWarning("Powerup spawns are not set by kills!");_pwrSpawnMod().spawnReqsType=spawnReqsType.kills;_pwrSpawnMod().spawnReqs=new spawnKills();var sr=(spawnKills)_pwrSpawnMod().spawnReqs;sr.killsNeeded=int.Parse(v);}
        OnChangeAnything();
    }
    public void ResetPowerupSpawns1(){
        _pwrSpawnMod().spawnReqsType=_pwrSpawnModDef().spawnReqsType;
        _pwrSpawnMod().spawnReqs=_pwrSpawnModDef().spawnReqs;
        spawnReqsMono.Validate(ref _pwrSpawnMod().spawnReqs, ref _pwrSpawnMod().spawnReqsType);
        OnChangeAnything();
    }

    public void SetPwrupWeight(string n, string v){
        LootTableEntryPowerup e=_pwrSpawnMod().powerupList.Find(x=>x.lootItem.name==n);
        e.dropChance=(float)System.Math.Round(float.Parse(v),2);
        _pwrSpawnMod().SumUpPowerupsWeightsTotal();
        OnChangeAnything();
    }
    public void SetPwrupLvlReq(string n, string v){
        LootTableEntryPowerup e=_pwrSpawnMod().powerupList.Find(x=>x.lootItem.name==n);
        e.levelReq=int.Parse(v);
        OnChangeAnything();
    }
    public void ResetPowerupSpawns2(){
        foreach(LootTableEntryPowerup e in _pwrSpawnMod().powerupList){
            LootTableEntryPowerup ed=_pwrSpawnModDef().powerupList.Find(x=>x.lootItem.name==e.lootItem.name);
            e.dropChance=ed.dropChance;
            e.levelReq=ed.levelReq;
        }
        SetPowerupsSpawnsChoices();
        OnChangeAnything();
    }
#endregion

#region///Start & Update functions
    void SetPresetIconSpritesLibrary(){
        presetIconSprites=new List<GSprite>();
        var _presetIconSpritesPre=presetIconSpritesPre;_presetIconSpritesPre.OrderBy(x=>x.name).ToList();presetIconSprites.AddRange(_presetIconSpritesPre);
        var _enemySprites=new List<GSprite>();
        foreach(GSprite g in enemySprites){if(!presetIconSprites.Exists(x=>x.name==g.name)&&g.name!="questionMark")_enemySprites.Add(g);}
        _enemySprites.OrderBy(x=>x.name).ToList();
        presetIconSprites.AddRange(_enemySprites);
        if(!AssetsManager.instance.sprites.Contains(_enemySprites[0]))AssetsManager.instance.sprites.AddRange(_enemySprites);
        //foreach(GameRules gr in CoreSetup.instance.gamerulesetsPrefabs){}
        //presetIconSprites=presetIconSprites.OrderBy(x=>x.name).ToList();

        Transform presetAppearanceIconSprLibTransform=presetAppearanceIconSprLibPanel.transform.GetChild(1).GetChild(0);presetAppearanceIconSprLibTransform.GetComponent<ContentSizeFitter>().enabled=true;presetAppearanceIconSprLibTransform.localPosition=new Vector2(0,-999);
        GameObject prefab=presetAppearanceIconSprLibTransform.GetChild(0).gameObject;
        for(var d=presetAppearanceIconSprLibTransform.childCount-1;d>=1;d--){Destroy(presetAppearanceIconSprLibTransform.GetChild(d).gameObject);}

        foreach(GSprite s in presetIconSprites){
            GameObject go=Instantiate(prefab,presetAppearanceIconSprLibTransform);
            go.name=s.name;
            Sprite _spr=s.spr;
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>SetPresetIcon(s.name));
        }
        prefab.GetComponent<Button>().onClick.AddListener(()=>SetPresetIcon("questionMark"));
        //Destroy(prefab);
    }
    /*void SetPlayerSpritesLibrary(){
        playerSprites=new List<GSprite>();
        if(ES3.DirectoryExists(_sandboxShipSkinsDir())){foreach(string f in ES3.GetFiles(_sandboxShipSkinsDir())){if(f.Contains(".png")){var spr=new GSprite();spr.name=f;spr.spr=Sprite.Create(ES3.LoadImage(f));playerSprites.Add(spr);}else{Debug.LogWarning(f+" is not a PNG file");}}}
        playerSprites=playerSprites.OrderBy(x=>x.name).ToList();

        Transform playerSprLibTransform=playerSpritesLibPanel.transform.GetChild(1).GetChild(0);playerSprLibTransform.GetComponent<ContentSizeFitter>().enabled=true;playerSprLibTransform.localPosition=new Vector2(0,-999);
        GameObject prefab=playerSprLibTransform.GetChild(0).gameObject;
        for(var d=playerSprLibTransform.childCount-1;d>=1;d--){Destroy(playerSprLibTransform.GetChild(d).gameObject);}

        foreach(GSprite s in playerSprites){
            GameObject go=Instantiate(prefab,playerSprLibTransform);
            go.name=s.name;
            Sprite _spr=s.spr;
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>SetPlayerSprite(s.name));
        }
        Destroy(prefab);
    }*/
    void SetStartingPowerupChoices(){
        Transform startingPowerupChoicesListTransform=startingPowerupChoices.transform.GetChild(0);//startingPowerupChoicesListTransform.GetComponent<ContentSizeFitter>().enabled=true;startingPowerupChoicesListTransform.localPosition=new Vector2(0,-999);
        GameObject prefab=startingPowerupChoicesListTransform.GetChild(0).gameObject;
            prefab.name="null";
            prefab.GetComponent<Image>().sprite=AssetsManager.instance.Spr("nullPwrup");
            prefab.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(""));
        List<PowerupItem> sortedPowerupList=AssetsManager.instance.powerupItems.OrderBy(x=>x.name).ToList();
        sortedPowerupList=sortedPowerupList.Distinct().ToList();
        foreach(PowerupItem p in sortedPowerupList){if(p.powerupType==powerupType.weapon){
            GameObject go=Instantiate(prefab,startingPowerupChoicesListTransform);
            go.name=p.name;
            go.GetComponent<Image>().sprite=AssetsManager.instance.GetObjSpr(p.assetName);
            go.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(p.name));
        }}
        startingPowerupChoices.SetActive(false);
    }

    void SetEnemyChoices(){
        Transform enemiesListTransform=enemiesPanel.transform.GetChild(1).GetChild(0);enemiesListTransform.GetComponent<ContentSizeFitter>().enabled=true;enemiesListTransform.localPosition=new Vector2(0,-999);
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
        foreach(GameRules gr in CoreSetup.instance.gamerulesetsPrefabs){
            foreach(EnemyClass e in gr.enemies){
                Sprite _spr=_enSprGR(e.name,gr);
                //string _n=_spr.name;
                string _n="";_n=_enSprStrGR(e.name,gr);
                
                if(e.name!="Comet"){
                    if(enemySprites.Exists(x=>x.name.Contains(_n))){_n+="_";}
                    if(!enemySprites.Exists(x=>x.spr==_spr)){enemySprites.Add(new GSprite{name=_n,spr=_spr});}
                }else{//For comets
                    foreach(string _csprn in gr.cometSettings.sprites.Concat(gr.cometSettings.spritesLunar).ToArray()){
                        Sprite _cspr=AssetsManager.instance.SprAnyReverse(_csprn);
                        if(!enemySprites.Exists(x=>x.spr==_cspr))enemySprites.Add(new GSprite{name=_csprn,spr=_cspr});
                    }
                }
            }
        }
        /*
        foreach(AdventureZoneData az in CoreSetup.instance.adventureZones){///Add adventure & boss sprites
            var gr=az.gameRules;
            foreach(EnemyClass e in gr.enemies){
                Sprite _spr=_enSprGR(e.name,gr);
                string _n=_spr.name;
                
                if(enemySprites.Exists(x=>x.name.Contains(_n))){_n+="_";}
                if(!enemySprites.Exists(x=>x.spr==_spr)){enemySprites.Add(new GSprite{name=_n,spr=_spr});}
            }
            if(az.isBoss){
                Sprite _spr=_enSprGR(gr.bossInfo.name,gr);
                string _n=_spr.name;
                
                if(enemySprites.Exists(x=>x.name.Contains(_n))){_n+="_";}
                if(!enemySprites.Exists(x=>x.spr==_spr)){enemySprites.Add(new GSprite{name=_n,spr=_spr});}
            }
        }
        */
        enemySprites=enemySprites.OrderBy(x=>x.name).ToList();
        enemySprites.AddRange(enemySpritesPost);
        

        Transform enemySprLibTransform=enemySpritesLibPanel.transform.GetChild(1).GetChild(0);enemySprLibTransform.GetComponent<ContentSizeFitter>().enabled=true;enemySprLibTransform.localPosition=new Vector2(0,-999);
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
        Transform wavesListTransform=wavesPanel.transform.GetChild(2).GetChild(0);wavesListTransform.GetComponent<ContentSizeFitter>().enabled=true;wavesListTransform.localPosition=new Vector2(0,-999);
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
            var waveStartingT=go.transform.GetChild(7);
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
        Transform powerupsSpawnerListTransform=collectiblesMainPanel.transform.GetChild(1).GetChild(0);powerupsSpawnerListTransform.GetComponent<ContentSizeFitter>().enabled=true;powerupsSpawnerListTransform.localPosition=new Vector2(0,-999);
        GameObject prefab=powerupsSpawnerListTransform.GetChild(1).gameObject;
        for(var d=powerupsSpawnerListTransform.childCount-1;d>=2;d--){Destroy(powerupsSpawnerListTransform.GetChild(d).gameObject);}

        List<PowerupsSpawnerGR> sortedPowerupSpawnerList=GameRules.instance.powerupSpawners.OrderBy(x=>x.name).ToList();
        foreach(PowerupsSpawnerGR e in sortedPowerupSpawnerList){
            GameObject go=Instantiate(prefab,powerupsSpawnerListTransform);
            go.name=e.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=e.name;
            Sprite _spr=AssetsManager.instance.GetObjSpr(e.sprAssetName);
            if(AssetsManager.instance.Spr(e.sprAssetName)!=null)AssetsManager.instance.Spr(e.sprAssetName);
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>OpenPowerupsSpawnPanel(e.name));
        }
        Destroy(prefab);
    }
    void SetPowerupsSpawnsChoices(){
        Transform powerupsListTransform=powerupsPanel.transform.GetChild(2).GetChild(0);powerupsListTransform.GetComponent<ContentSizeFitter>().enabled=true;powerupsListTransform.localPosition=new Vector2(0,-999);
        GameObject prefab=powerupsListTransform.GetChild(0).gameObject;
        for(var d=powerupsListTransform.childCount-1;d>=1;d--){Destroy(powerupsListTransform.GetChild(d).gameObject);}

        powerupsWeightsSumTotal.text=_pwrSpawnMod().sum.ToString();
        List<LootTableEntryPowerup> sortedPowerupsList=_pwrSpawnMod().powerupList.OrderBy(x=>x.lootItem.name).ToList();
        foreach(LootTableEntryPowerup e in sortedPowerupsList){
            GameObject go=Instantiate(prefab,powerupsListTransform);
            go.name=e.lootItem.name;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=e.lootItem.name;
            Sprite _spr=AssetsManager.instance.Get(e.lootItem.assetName).GetComponent<SpriteRenderer>().sprite;
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

    void SetBuiltInPresetsButtons(){
        foreach(GameRules gr in CoreSetup.instance.gamerulesetsPrefabs){
            string name=gr.cfgName;     if(name.Contains(" Mode"))name=name.Replace(" Mode","");
            Transform builtInModesListTransform=builtInPresetsPanel.transform.GetChild(0).GetChild(0);
            GameObject go=Instantiate(gameModeListElementPrefab,builtInModesListTransform);
            builtInModesListTransform.GetComponent<ContentSizeFitter>().enabled=true;builtInModesListTransform.localPosition=new Vector2(0,-999);
            go.name=gr.cfgName+"-PresetButton";
            go.GetComponent<Button>().onClick.AddListener(()=>SelectBuiltinPreset(gr.cfgName));
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=name;
            go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=gr.cfgDesc;
            if(go.transform.GetChild(1).GetChild(0)!=null){Destroy(go.transform.GetChild(1).GetChild(0).gameObject);}
            if(gr.cfgIconsGo!=null){Instantiate(gr.cfgIconsGo,go.transform.GetChild(1));}
            else{go.transform.GetChild(1).gameObject.AddComponent<Image>().sprite=AssetsManager.instance.SprAny(gr.cfgIconAssetName);}
        }
        ResetBuiltinPresetButtonsColors();
    }
    void SetYoursPresetsButtonsDelay(float delay){StartCoroutine(SetYoursPresetsButtonsDelayI(delay));}
    IEnumerator SetYoursPresetsButtonsDelayI(float delay){
        yield return new WaitForSecondsRealtime(delay);
        SetYoursPresetsButtons();
    }
    void SetYoursPresetsButtons(){
        Transform yoursModesListTransform=yoursPresetsPanel.transform.GetChild(0).GetChild(0);
        for(var d=yoursModesListTransform.childCount-1;d>=0;d--){Destroy(yoursModesListTransform.GetChild(d).gameObject);}

        if(ES3.DirectoryExists(_sandboxSavesDir())){foreach(string f in ES3.GetFiles(_sandboxSavesDir())){if(f.Contains(".json")){AddNewYoursPresetButton(f);}else{Debug.LogWarning(f+" is not a JSON file");}}}
        HighlightActivePreset();
    }
    void AddNewYoursPresetButton(string f,GameRules _grCur=null){
        ES3.Save<GameRules>("gamerulesData",GameRules.instance,_sandboxSavesDir()+"/"+"_temp.json");
        Transform yoursModesListTransform=yoursPresetsPanel.transform.GetChild(0).GetChild(0);
        
        var fpath=_sandboxSavesDir()+"/"+f;
        if(f!="_temp"&&ES3.FileExists(fpath)||_grCur!=null){if(ES3.KeyExists("gamerulesData",fpath)||_grCur!=null){
            List<GameRules> gr=new List<GameRules>();if(_grCur==null){gr.Add(ES3.Load<GameRules>("gamerulesData",fpath));}else{gr.Add(_grCur);}

            if(gr.Count>0&&gr[0]!=null){
                string name=f,fname=f;SandboxSaveInfo _saveInfo=null;
                if(fname.Contains(".json")){fname=fname.Replace(".json","");}name=fname;///Old taking name from file name
                if(ES3.KeyExists("saveInfo",fpath)){_saveInfo=ES3.Load<SandboxSaveInfo>("saveInfo",fpath);name=_saveInfo.name;}

                GameObject go=Instantiate(yoursPrefabElementPrefab,yoursModesListTransform);
                yoursModesListTransform.GetComponent<ContentSizeFitter>().enabled=true;yoursModesListTransform.localPosition=new Vector2(0,-999);
                go.name=fname+"-PresetButton";
                go.GetComponent<Button>().onClick.AddListener(()=>SelectPreset(fname));
                go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=name;
                if(_saveInfo!=null){go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=_saveInfo.desc;}
                else{go.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text=gr[0].cfgDesc;}
                go.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text=GetTimestamp(fname);
                go.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text=GetSaveBuild(fname);
                if(_saveInfo!=null&&!String.IsNullOrEmpty(_saveInfo.iconAssetName)&&_saveInfo.iconAssetName!="questionMark"){
                    go.transform.GetChild(1).gameObject.GetComponent<Image>().sprite=AssetsManager.instance.SprAny(_saveInfo.iconAssetName);
                    go.transform.GetChild(1).gameObject.GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(go.transform.GetChild(1).gameObject.GetComponent<Image>().material,_saveInfo.iconShaderMatProps,true);
                }else{
                    /*if(gr[0].cfgIconsGo!=null){
                        Instantiate(gr[0].cfgIconsGo,go.transform.GetChild(1));
                        Destroy(go.transform.GetChild(1).GetComponent<Image>());
                    }else{*/
                        if(!String.IsNullOrEmpty(gr[0].cfgIconAssetName)){
                            go.transform.GetChild(1).gameObject.GetComponent<Image>().sprite=AssetsManager.instance.SprAny(gr[0].cfgIconAssetName);
                            go.transform.GetChild(1).gameObject.GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(go.transform.GetChild(1).gameObject.GetComponent<Image>().material,gr[0].cfgIconShaderMatProps,true);
                        }
                    //}
                }
            }
        }}
        ES3.LoadInto("gamerulesData",_sandboxSavesDir()+"/"+"_temp.json",GameRules.instance);
        ES3.DeleteFile(_sandboxSavesDir()+"/"+"_temp.json");
        CountSaveFiles();
    }
    int GetCountSaveFiles(){
        var listOfValidSaves=ES3.GetFiles(_sandboxSavesDir());
        listOfValidSaves=listOfValidSaves.Where(x=>x.Contains(".json")).ToArray();
        return listOfValidSaves.Length;
    }
    void CountSaveFiles(){savesCount=GetCountSaveFiles();}


    void SetPowerups(){
        if(powerupInventory!=null){
            for(var i=0;i<GameRules.instance.powerupsCapacity;i++){
                Sprite _spr;
                if(GameRules.instance.powerupsStarting.Count>i&&!String.IsNullOrEmpty(GameRules.instance.powerupsStarting[i].name)){
                    _spr=AssetsManager.instance.Get(AssetsManager.instance.GetPowerupItem(GameRules.instance.powerupsStarting[i].name).assetName).GetComponent<SpriteRenderer>().sprite;
                }else{_spr=AssetsManager.instance.Spr("nullPwrup");}
                powerupInventory.transform.GetChild(0).GetChild(i).GetComponent<Image>().sprite=_spr;
            }
            for(var i=9;i>=GameRules.instance.powerupsCapacity;i--){
                if(powerupInventory.transform.GetChild(0).GetChild(i).gameObject.activeSelf)powerupInventory.transform.GetChild(0).GetChild(i).gameObject.SetActive(false);
            }
            for(var i=0;i<GameRules.instance.powerupsCapacity;i++){
                if(!powerupInventory.transform.GetChild(0).GetChild(i).gameObject.activeSelf)powerupInventory.transform.GetChild(0).GetChild(i).gameObject.SetActive(true);
            }

            if(UIInputSystem.instance!=null){
                if(
                    (startingPowerupChoices!=UIInputSystem.instance.currentSelected)
                    &&(!Array.Exists(startingPowerupChoices.GetComponentsInChildren<Transform>(),x=>x.gameObject==UIInputSystem.instance.currentSelected
                        &&x.GetComponent<Button>()==UIInputSystem.instance.btn))
                    &&(!Array.Exists(powerupInventory.GetComponentsInChildren<Transform>(),x=>x.gameObject==UIInputSystem.instance.currentSelected
                        &&x.GetComponent<Button>()==UIInputSystem.instance.btn))
                )startingPowerupChoices.SetActive(false);
            }
        }else{Debug.LogError("PowerupInventory not assigned!");}
    }
    void SetPresetIconPreviewsSprite(){
        var _spr=AssetsManager.instance.SprAny(saveInfo.iconAssetName);
        defaultPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite=_spr;
        presetAppearanceMainPanel.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Image>().sprite=_spr;
        presetAppearanceIconPanel.transform.GetChild(1).GetComponent<Image>().sprite=_spr;

        //var _mat=;
        defaultPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(defaultPanel.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().material,saveInfo.iconShaderMatProps,true);
        presetAppearanceMainPanel.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(presetAppearanceMainPanel.GetComponentInChildren<Button>().transform.GetChild(0).GetComponent<Image>().material,saveInfo.iconShaderMatProps,true);
        presetAppearanceIconPanel.transform.GetChild(1).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(presetAppearanceIconPanel.transform.GetChild(1).GetComponent<Image>().material,saveInfo.iconShaderMatProps,true);
    }
    void SetPlayerPreviewsSprite(){
        playerMainPanel.transform.GetChild(0).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(playerMainPanel.transform.GetChild(0).GetComponent<Image>().material,GameRules.instance.playerShaderMatProps,true);
        playerSpritePanel.transform.GetChild(0).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(playerSpritePanel.transform.GetChild(0).GetComponent<Image>().material,GameRules.instance.playerShaderMatProps,true);
    }
    void SetEnemyPreviewsSprite(){
        if(true){foreach(Transform t in enemiesPanel.transform.GetChild(1).GetChild(0)){if(_en(t.gameObject.name)!=null){
            t.GetChild(0).GetComponent<Image>().sprite=_enSpr(t.gameObject.name);
            t.GetChild(0).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(t.GetChild(0).GetComponent<Image>().material,_enSprMat(t.gameObject.name),true);
        }}}
        if(_enMod()!=null){
            if(_enModSpr()!=null){
                enemyMainPanel.transform.GetChild(1).GetComponent<Image>().sprite=_enModSpr();
                enemySpritePanel.transform.GetChild(1).GetComponent<Image>().sprite=_enModSpr();
            }
            if(_enModSprMat()!=null){
                enemyMainPanel.transform.GetChild(1).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(enemyMainPanel.transform.GetChild(1).GetComponent<Image>().material,_enModSprMat(),true);
                enemySpritePanel.transform.GetChild(1).GetComponent<Image>().material=AssetsManager.instance.UpdateShaderMatProps(enemySpritePanel.transform.GetChild(1).GetComponent<Image>().material,_enModSprMat(),true);
            }
        }
    }
    void SetEnemyTypePreview(){
        if(_enMod()!=null){
            enemyMainPanel.transform.GetChild(3).GetComponent<TMP_Dropdown>().value=(int)_enMod().type;
        }
    }

    public void SavePopup(string str,float length=1f){SandboxCanvas.instance.StartCoroutine(SandboxCanvas.instance.SavePopupI(str,length));}
    IEnumerator SavePopupI(string str,float length=1f){
        savePopup.gameObject.SetActive(true);
        savePopup.GetComponentInChildren<TextMeshProUGUI>().text=str;
        yield return new WaitForSecondsRealtime(length);
        savePopup.gameObject.SetActive(false);
    }
    public GameObject _powerupInventoryGameObject(){return powerupInventory;}
    public string GetTimestamp(string fname,bool sandboxSavesDir=true,bool currentTimeIfNull=false){
        DateTime _timestamp=new DateTime();
        if(sandboxSavesDir){_timestamp=ES3.GetTimestamp(_sandboxSavesDir()+"/"+fname+".json");}
        else{_timestamp=ES3.GetTimestamp(fname);}
        var _timestampString="??.????"+"??:??";
        if(_timestamp==DateTime.UnixEpoch&&currentTimeIfNull){_timestamp=DateTime.Now;}
        if(_timestamp!=DateTime.UnixEpoch){_timestampString=_timestamp.ToShortDateString()+" "+_timestamp.ToShortTimeString();}
        return _timestampString;
    }
    public string GetTimestampCurrent(bool currentTimeIfNull=false){return GetTimestamp(saveSelected,currentTimeIfNull:currentTimeIfNull);}
    public string GetSaveBuild(string fname,bool sandboxSavesDir=true,bool currentTimeIfNull=false){
        int _saveBuild=-1;
        if(sandboxSavesDir){var _path=_sandboxSavesDir()+"/"+fname+".json";if(ES3.KeyExists("saveInfo",_path))_saveBuild=ES3.Load<SandboxSaveInfo>("saveInfo",_path).saveBuild;}
        else{if(ES3.KeyExists("saveInfo",fname))_saveBuild=ES3.Load<SandboxSaveInfo>("saveInfo",fname).saveBuild;}
        var _saveBuildString="BUILD\n"+_saveBuild;
        if(_saveBuild==-1){_saveBuildString="";}
        return _saveBuildString;
    }
#endregion
}

[System.Serializable]
public class SandboxSaveInfo{
    public string name;
    public string desc;
    public string author;
    public int saveBuild;
    public float gameBuild;
    public string presetFrom;
    public string iconAssetName;
    public ShaderMatProps iconShaderMatProps;
}