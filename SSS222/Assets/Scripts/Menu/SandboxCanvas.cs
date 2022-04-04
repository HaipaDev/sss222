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
    [Header("Enemy Subpanels")]
    [SceneObjectsOnly][SerializeField]GameObject enemyMainPanel;
    [SceneObjectsOnly][SerializeField]GameObject enemySpritePanel;
    [SceneObjectsOnly][SerializeField]GameObject enemySpritesLibPanel;
    [Title("Variables & Other obj", titleAlignment: TitleAlignments.Centered)]
    [DisableInEditorMode][SerializeField] public GameRules presetGameruleset;
    [SceneObjectsOnly][SerializeField]GameObject powerupInventory;
    [DisableInEditorMode][SerializeField]int powerupToSet;
    [SceneObjectsOnly][SerializeField]GameObject powerupChoices;
    [DisableInEditorMode][SerializeField]public string enemyToModify;
    [DisableInEditorMode][SerializeField]public List<GSprite> enemySprites;
    [DisableInEditorMode][SerializeField][Range(0,360)]public int bgHue;
    [DisableInEditorMode][SerializeField][Range(0,2)]public float bgSatur=1;
    [DisableInEditorMode][SerializeField][Range(0,2)]public float bgValue=1;
    void Start(){
        instance=this;
        presetGameruleset=GameCreator.instance.gamerulesetsPrefabs[0];
        OpenDefaultPanel();
        SetPowerupChoices();
        SetEnemyChoices();
        SetEnemySpritesLibrary();
    }
    void Update(){
        CheckESC();
        SetPowerups();
        //SetEnemyPreviewsSprite();
        GameSession.instance.gameSpeed=GameRules.instance.defaultGameSpeed;
    }
    public void Back(){
        if(_anyFirstLevelPanelsActive()){OpenDefaultPanel();}
            else if(enemyMainPanel.activeSelf){OpenEnemiesPanel();}
                else if(enemySpritePanel.activeSelf){OpenEnemyPanel(enemyToModify);}
                    else if(enemySpritesLibPanel.activeSelf){OpenEnemySpritePanel();}
        else{GSceneManager.instance.LoadGameModeChooseScene();}
    }
    public void OpenDefaultPanel(){CloseAllPanels();defaultPanel.SetActive(true);}
    public void OpenPresetsPanel(){CloseAllPanels();presetsPanel.SetActive(true);}
    public void OpenGlobalPanel(){CloseAllPanels();globalPanel.SetActive(true);}
    public void OpenPlayerPanel(){CloseAllPanels();playerPanel.SetActive(true);}
    public void OpenEnemiesPanel(){CloseAllPanels();enemiesPanel.SetActive(true);SetEnemyPreviewsSprite();}
    public void OpenEnemyPanel(string str){CloseAllPanels();enemyPanel.SetActive(true);enemyMainPanel.SetActive(true);enemyToModify=str;SetEnemyPreviewsSprite();}
    public void OpenEnemySpritePanel(){if(_canModifySpriteEn()){CloseAllPanels();enemyPanel.SetActive(true);enemySpritePanel.SetActive(true);SetEnemyPreviewsSprite();}}
    public void OpenEnemySpritesLibPanel(){CloseAllPanels();enemyPanel.SetActive(true);enemySpritesLibPanel.SetActive(true);}
    bool _anyFirstLevelPanelsActive(){bool b=false;
        if(presetsPanel.activeSelf
        ||globalPanel.activeSelf
        ||playerPanel.activeSelf
        ||enemiesPanel.activeSelf
        ){b=true;}
        return b;}
    void CloseAllPanels(){
        defaultPanel.SetActive(false);
        presetsPanel.SetActive(false);
        globalPanel.SetActive(false);
        playerPanel.SetActive(false);
        enemiesPanel.SetActive(false);
        enemyPanel.SetActive(false);

        enemyMainPanel.SetActive(false);
        enemySpritePanel.SetActive(false);
        enemySpritesLibPanel.SetActive(false);

        powerupChoices.SetActive(false);
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
    }

#region//Global
    public void SetGameSpeed(float v){GameRules.instance.defaultGameSpeed=(float)System.Math.Round(v,2);}
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
    public void OpenPowerupChoices(int id){powerupChoices.SetActive(true);powerupChoices.transform.position=new Vector2(Input.mousePosition.x,Input.mousePosition.y+50f);powerupToSet=id;}
    public void SetPowerupStarting(string v){
        if(GameRules.instance.powerupsStarting.Count<=powerupToSet){for(var i=GameRules.instance.powerupsStarting.Count;i<=powerupToSet;i++){
            GameRules.instance.powerupsStarting.Add(new Powerup());}}
        if(GameRules.instance.powerupsStarting[powerupToSet]==null){GameRules.instance.powerupsStarting[powerupToSet]=new Powerup();}
        else{GameRules.instance.powerupsStarting[powerupToSet].name=v;}
        powerupChoices.SetActive(false);
    }
#endregion
#region//Enemies
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
            if(_mat==null||(_mat!=null&&!_mat.shader.name.Contains("AllIn1SpriteShader"))){Debug.LogWarning(".");
                if(GameAssets.instance.Mat("HueShift")!=null){_mat=Instantiate(GameAssets.instance.Mat("HueShift"));}
                _mat.SetInt("_HsvShift",0);
                _en(str).sprMat=_mat;
            }
        }
        return _mat;
    }
    public Material _enModSprMat(){return _enSprMat(enemyToModify);}

    //Enemy Main Settings
    public void SetEnemyHealth(string v){_enMod().healthStart=float.Parse(v);}
    public void SetEnemyHealthMax(string v){_enMod().healthMax=float.Parse(v);}
    public void SetEnemyDefense(string v){_enMod().defense=int.Parse(v);}
    public void SetEnemyScoreStart(string v){_enMod().scoreValue=new Vector2(float.Parse(v),_enMod().scoreValue.y);}
    public void SetEnemyScoreEnd(string v){_enMod().scoreValue=new Vector2(_enMod().scoreValue.x,float.Parse(v));}

    //Enemy Sprite
    public void SetEnemySprite(string v){_enMod().spr=enemySprites.Find(x=>x.name==v).spr;OpenEnemySpritePanel();}
    public void SetEnemySprMatHue(float v){_enModSprMat().SetInt("_HsvShift",(int)v);}
    public void SetEnemySprMatSatur(float v){_enModSprMat().SetFloat("_HsvSaturation",v);}
    public void SetEnemySprMatValue(float v){_enModSprMat().SetFloat("_HsvBright",v);}
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
            go.GetComponent<Image>().sprite=GameAssets.instance.GetObjSpr(p.assetName);
            go.GetComponent<Button>().onClick.AddListener(()=>SetPowerupStarting(p.name));
        }}
        powerupChoices.SetActive(false);
    }
    void SetEnemyChoices(){
        GameObject prefab=enemiesPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        enemySprites=new List<GSprite>();
        foreach(EnemyClass e in GameRules.instance.enemies){
            GameObject go=Instantiate(prefab,enemiesPanel.transform.GetChild(1).GetChild(0));
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
                //if(!enemySprites.Exists(x=>x.spr==_spr)){
                    string _n=_spr.name;
                    
                    if(e.name=="Comet"){
                        //var _ci=0;
                        foreach(Sprite _cspr in gr.cometSettings.sprites.Concat(gr.cometSettings.spritesLunar).ToArray()){
                            _n=_cspr.name;
                            //_ci++;  _n=e.name+_ci;
                            //if(_cspr.name.Contains("Lunar")){_n=e.name+"Lunar"+_ci;}
                            //if(enemySprites.Exists(x=>x.name.Contains(_n))){_n+="_";}
                            if(!enemySprites.Exists(x=>x.spr==_cspr))enemySprites.Add(new GSprite{name=_n,spr=_cspr});
                        }
                    }else{      if(enemySprites.Exists(x=>x.name.Contains(_n))){_n+="_";}
                        if(!enemySprites.Exists(x=>x.spr==_spr)){enemySprites.Add(new GSprite{name=_n,spr=_spr});}
                    }
                //}
            }
        }
        enemySprites=enemySprites.OrderBy(x=>x.name).ToList();
        
        GameObject prefab=enemySpritesLibPanel.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        foreach(GSprite s in enemySprites){
            GameObject go=Instantiate(prefab,enemySpritesLibPanel.transform.GetChild(1).GetChild(0));
            go.name=s.name;
            Sprite _spr=s.spr;
            if(_spr!=null)go.transform.GetChild(0).GetComponent<Image>().sprite=_spr;
            go.GetComponent<Button>().onClick.AddListener(()=>SetEnemySprite(s.name));
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