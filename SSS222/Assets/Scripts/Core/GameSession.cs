using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using BayatGames.SaveGameFree;
public class GameSession : MonoBehaviour{
    public static GameSession instance;
    [HeaderAttribute("Current Player Values")]
    public int score = 0;
    public float scoreMulti = 1f;
    public int coins = 0;
    public int cores = 0;
    public float coresXp = 0f;
    public float coresXpTotal = 0f;
    public int enemiesCount = 0;
    [HeaderAttribute("EVent Score Values")]
    public int EVscore = 0;
    public int EVscoreMax = 50;
    public int shopScore = 0;
    public int shopScoreMax = 200;
    public int shopScoreMaxS = 200;
    public int shopScoreMaxE = 450;
    [HeaderAttribute("XP Values")]
    public float xp_forCore=100f;
    public float xp_wave=20f;
    public float xp_shop=10f;
    public float xp_powerup=3f;
    public float xp_flying=7f;
    public float flyingTimeReq=25f;
    public float xp_staying=-5f;
    public float stayingTimeReq=4f;
    [HeaderAttribute("Settings")]
    [Range(0.0f, 10.0f)] public float gameSpeed = 1f;
    [HeaderAttribute("Other")]
    public bool cheatmode;
    public bool dmgPopups=true;
    
    Player player;
    public bool speedChanged;
    //public string gameVersion;
    //public bool moveByMouse = true;

    /*public SavableData savableData;
    [System.Serializable]
    public class SavableData{
        public int highscore;
        public SavableData(SavableData data)
        {
            highscore = data.highscore;
        }
        public void Save()
        {
            SaveSystem.SaveData(this);
        }
        public void Load(){
            SavableData data = SaveSystem.LoadData();
            highscore = data.highscore;
        }
    }*/

    private void Awake(){
        SetUpSingleton();
    }
    private void SetUpSingleton(){
        int numberOfObj = FindObjectsOfType<GameSession>().Length;
        if(numberOfObj > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        FindObjectOfType<SaveSerial>().highscore = 0;
    }
    private void Update()
    {
        Time.timeScale = gameSpeed;
        if(shopScore>=shopScoreMax && coins>0)
        {
            Shop.instance.SpawnCargo();
            /*Shop.shopOpen = true;
            foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
                enemy.givePts = false;
                enemy.health = -1;
                enemy.Die();
            }
            gameSpeed = 0f;*/
            shopScoreMax = Random.Range(shopScoreMaxS,shopScoreMaxE);
            shopScore = 0;
        }

        if(FindObjectOfType<Player>()!=null){
            if(FindObjectOfType<Player>().timeFlyingCore>flyingTimeReq){AddXP(xp_flying);FindObjectOfType<Player>().timeFlyingCore=0f;}
            if(FindObjectOfType<Player>().stayingTimerCore>stayingTimeReq){SubXP(xp_staying);FindObjectOfType<Player>().stayingTimerCore=0f;}
        }

        Mathf.Clamp(coresXp,0,xp_forCore);
        if(coresXpTotal<0)coresXpTotal=0;
        if(coresXp>=xp_forCore){
            //cores++;
            GameAssets.instance.Make("PowerCore",new Vector2(Random.Range(-3.5f, 3.5f),7.4f));
            FindObjectOfType<UpgradeMenu>().total_UpgradesCount++;
            //FindObjectOfType<UpgradeMenu>().total_UpgradesCount++;
            coresXp=0;
            //AudioManager.instance.Play("LvlUp");
            AudioManager.instance.Play("LvlUp2");
        }

        //Set speed to normal
        if(PauseMenu.GameIsPaused==false&&Shop.shopOpened==false&&UpgradeMenu.UpgradeMenuIsOpen==false&&
        (FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().matrix==false)&&speedChanged!=true){gameSpeed=1;}
        if(SceneManager.GetActiveScene().name!="Game"){gameSpeed=1;}
        //if(Shop.shopOpen==false&&Shop.shopOpened==false){gameSpeed=1;}
        if(FindObjectOfType<Player>()==null){gameSpeed=1;}
        
        //Restart with R or Space/Resume with Space
        if((GameObject.Find("GameOverCanvas")!=null&&GameObject.Find("GameOverCanvas").activeSelf==true)&&(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.R))
        ||(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.R))){
            FindObjectOfType<Level>().RestartGame();}
        else if(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.Space)){FindObjectOfType<PauseMenu>().Resume();}

        //var inv=false;
        if((PauseMenu.GameIsPaused==true||Shop.shopOpened==true||UpgradeMenu.UpgradeMenuIsOpen==true)&&(FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().inverter==true)){
            //inv=true;
            //FindObjectOfType<Player>().inverter=false;
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
                if(sound!=null){
                    GameObject snd=sound.gameObject;
                    //if(sound!=musicPlayer){
                    if(snd.GetComponent<MusicPlayer>()==null){
                        //sound.pitch=1;
                        sound.Stop();
                    }
                }
            }
        }/*else if(PauseMenu.GameIsPaused==false&&Shop.shopOpened==false&&UpgradeMenu.UpgradeMenuIsOpen==false&&
        FindObjectOfType<Player>()!=null&&inv==true){FindObjectOfType<Player>().inverter=true;}*/
        if(FindObjectOfType<Player>()!=null&&FindObjectOfType<Player>().inverter==false){
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
                if(sound!=null){
                    GameObject snd=sound.gameObject;
                    //if(sound!=musicPlayer){
                    if(snd.GetComponent<MusicPlayer>()==null){
                        if(sound.pitch==-1)sound.pitch=1;
                        if(sound.loop==true)sound.loop=false;
                    }
                }
            }
        }

        CheckCodes(0,0);
    }

    public int GetScore(){return score;}
    public int GetCoins(){return coins;}
    public int GetCores(){return cores;}
    public float GetCoresXP(){return coresXp;}
    public int GetEVScore(){return EVscore;}
    public int GetShopScore(){return shopScore; }
    public int GetHighscore(){return FindObjectOfType<SaveSerial>().highscore;}
    public string GetVersion(){return FindObjectOfType<SaveSerial>().gameVersion;}

    public void AddToScore(int scoreValue){
        score += Mathf.RoundToInt(scoreValue*scoreMulti);
        EVscore += scoreValue;
        shopScore += Mathf.RoundToInt(scoreValue*scoreMulti);
        ScorePopUpHUD(scoreValue*scoreMulti);
    }

    public void MultiplyScore(float multipl)
    {
        int result=Mathf.RoundToInt(score * multipl);
        score = result;
    }

    public void AddToScoreNoEV(int scoreValue){score += scoreValue;ScorePopUpHUD(scoreValue);}
    public void AddXP(float xpValue){coresXp += xpValue;coresXpTotal+=xpValue;XPPopUpHUD(xpValue);}
    public void SubXP(float xpValue){coresXp += xpValue;coresXpTotal+=xpValue;XPSubPopUpHUD(xpValue);}
    public void AddEnemyCount(){enemiesCount++;FindObjectOfType<DisruptersSpawner>().EnemiesCountHealDrone++;
    var ps=FindObjectsOfType<PowerupsSpawner>();
    foreach(PowerupsSpawner p in ps){
        if(p.enemiesCountReq!=-1){
            p.enemiesCount++;
        }    
    }
    }

    public void ResetScore(){
        score=0;
        EVscore = 0;
        shopScore = 0;
        coins = 0;
        coresXp = 0;
        cores = 0;
    }
    public void SaveHighscore()
    {
        if (score > FindObjectOfType<SaveSerial>().highscore) FindObjectOfType<SaveSerial>().highscore = score;
        //FindObjectOfType<DataSavable>().highscore = highscore;
    }
    public void SaveSettings(){
        var ss=FindObjectOfType<SaveSerial>();
        var sm=FindObjectOfType<SettingsMenu>();
        ss.moveByMouse = sm.moveByMouse;
        ss.quality = sm.quality;
        ss.fullscreen = sm.fullscreen;
        ss.scbuttons = sm.scbuttons;
        ss.pprocessing = sm.pprocessing;
        ss.masterVolume = sm.masterVolume;
        ss.soundVolume = sm.soundVolume;
        ss.musicVolume = sm.musicVolume;

        ss.SaveSettings();
    }
    public void SaveInventory(){
        FindObjectOfType<SaveSerial>().chameleonColor[0] = FindObjectOfType<Inventory>().chameleonColorArr[0];
        FindObjectOfType<SaveSerial>().chameleonColor[1] = FindObjectOfType<Inventory>().chameleonColorArr[1];
        FindObjectOfType<SaveSerial>().chameleonColor[2] = FindObjectOfType<Inventory>().chameleonColorArr[2];
    }
    public void Save(){ FindObjectOfType<SaveSerial>().Save(); FindObjectOfType<SaveSerial>().SaveSettings(); }
    public void Load(){ FindObjectOfType<SaveSerial>().Load(); FindObjectOfType<SaveSerial>().LoadSettings(); }
    public void DeleteAllShowConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((false)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((true)); }
    public void DeleteAllHideConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((true)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((false)); }
    public void DeleteAll(){ FindObjectOfType<SaveSerial>().Delete(); ResetSettings(); FindObjectOfType<Level>().Restart(); Destroy(FindObjectOfType<SaveSerial>().gameObject); Destroy(gameObject);}
    public void ResetSettings(){
        FindObjectOfType<SaveSerial>().ResetSettings();
        FindObjectOfType<Level>().RestartScene();
        FindObjectOfType<SaveSerial>().SaveSettings();
        /*var s=FindObjectOfType<SettingsMenu>();
        s.moveByMouse=true;
        s.quality=4;
        s.fullscreen=true;
        s.masterVolume=1;
        s.soundVolume=1;
        s.musicVolume=1;*/
    }public void ResetMusicPitch(){
        if(FindObjectOfType<MusicPlayer>()!=null)FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().pitch=1;
    }

    void OnApplicationQuit(){
        /*var skills=player.GetComponent<PlayerSkills>().skills;
        foreach(SkillSlotID skill in skills){
            skill.keySet=skillKeyBind.Disabled;
        }*/
    }
    public void CheckCodes(int fkey, int nkey){
        if(fkey==0&&nkey==0){}
        if(Input.GetKey(KeyCode.Delete) || fkey==-1){
            if(Input.GetKeyDown(KeyCode.Alpha0) || nkey==0){
                cheatmode=true;
            }if(Input.GetKeyDown(KeyCode.Alpha9) || nkey==9){
                cheatmode=false;
            }
        }
        if(cheatmode==true){
            if(Input.GetKey(KeyCode.F1) || fkey==1){
                player=FindObjectOfType<Player>();
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){player.health=player.maxHP;}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){player.energy=player.maxEnergy;}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){player.gclover=true;player.gcloverTimer=player.gcloverTime;}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){player.health=0;}
            }
            if(Input.GetKey(KeyCode.F2) || fkey==2){
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){AddToScoreNoEV(100);}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){AddToScoreNoEV(1000);}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){EVscore=EVscoreMax;}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){coins+=1;shopScore=shopScoreMax;}
                if(Input.GetKeyDown(KeyCode.Alpha5) || nkey==5){AddXP(100);}
                if(Input.GetKeyDown(KeyCode.Alpha6) || nkey==6){coins+=100;cores+=100;}
                if(Input.GetKeyDown(KeyCode.Alpha7) || nkey==7){FindObjectOfType<UpgradeMenu>().total_UpgradesLvl+=10;}
            }
            if(Input.GetKey(KeyCode.F3) || fkey==3){
                player=FindObjectOfType<Player>();
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){player.powerup="laser3";}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){player.powerup="mlaser";}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){player.powerup="lsaber";}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){player.powerup="lclaws";}
                if(Input.GetKeyDown(KeyCode.Alpha5) || nkey==5){player.powerup="cstream";}
            }
        }
    }

    void ScorePopUpHUD(float score){
        GameObject scpopupHud=GameObject.Find("ScoreDiffParrent");
        scpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //scpupupHud.GetComponent<Animator>().SetTrigger(0);
        scpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+score.ToString();
    }void XPPopUpHUD(float xp){
        GameObject xppopupHud=GameObject.Find("XPDiffParrent");
        xppopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //xppopupHud.GetComponent<Animator>().SetTrigger(0);
        xppopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+xp.ToString();
    }void XPSubPopUpHUD(float xp){
        GameObject xppopupHud=GameObject.Find("XPDiffParrent");
        xppopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //xppopupHud.GetComponent<Animator>().SetTrigger(0);
        xppopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+Mathf.Abs(xp).ToString();
    }
    //public void PlayDenySFX(){AudioManager.instance.Play("Deny");}
}
