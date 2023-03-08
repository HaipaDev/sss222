using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class CoreSetup : MonoBehaviour{   public static CoreSetup instance;
    [Header("Main managers")]
    [AssetsOnly][SerializeField] GameObject saveSerialPrefab;
    [AssetsOnly][SerializeField] GameObject easySavePrefab;
    [AssetsOnly][SerializeField] GameObject gsceneManagerPrefab;
    [AssetsOnly][SerializeField] GameObject gameManagerPrefab;
    
    [Header("Assets managers")]
    [AssetsOnly][SerializeField] GameObject assetsManagerPrefab;
    [AssetsOnly][SerializeField] GameObject audioManagerPrefab;
    [AssetsOnly][SerializeField] GameObject jukeboxPrefab;
    
    [Header("Networking, Advancements etc")]
    [AssetsOnly][SerializeField] GameObject dbaccessPrefab;
    [AssetsOnly][SerializeField] GameObject discordRPCPrefab;
    [AssetsOnly][SerializeField] GameObject steamManagerPrefab;
    [AssetsOnly][SerializeField] GameObject statsAchievsManagerPrefab;
    [Header("Game Rulesets")]
    //[SerializeField] int gamerulesetsID;
    [AssetsOnly][SerializeField] public GameRules[] gamerulesetsPrefabs;
    [ReadOnly][SerializeField] public int _gamerulesetsPrefabsLength;
    [AssetsOnly][SerializeField] public GameRules adventureGamerulesPrefab;
    [AssetsOnly][SerializeField] public GameRules adventureTravelZonePrefab;
    [AssetsOnly][SerializeField] public List<AdventureZoneData> adventureZones;
    void Awake(){
        instance=this;
        if(SceneManager.GetActiveScene().name=="Loading")LoadPre();
        else Load();
    }
    void LoadPre(){
        if(FindObjectOfType<SaveSerial>()==null){Instantiate(saveSerialPrefab);}
        if(FindObjectOfType<ES3ReferenceMgr>()==null){Instantiate(easySavePrefab);}
        if(FindObjectOfType<GSceneManager>()==null){var go=Instantiate(gsceneManagerPrefab);go.GetComponent<GSceneManager>().enabled=true;}/*Idk it disables itself? so I guess Ill turn it on manually*/
        if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
        if(FindObjectOfType<AudioManager>()==null){Instantiate(audioManagerPrefab);}
    }
    void Load(){
        LoadPre();
        if(FindObjectOfType<GameManager>()==null){Instantiate(gameManagerPrefab);}

        if(FindObjectOfType<AssetsManager>()==null){Instantiate(assetsManagerPrefab);}

        //if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
        #if (!UNITY_ANDROID && !UNITY_EDITOR) || (UNITY_ANDROID && UNITY_EDITOR)
        if(FindObjectOfType<DiscordPresence.PresenceManager>()==null){Instantiate(discordRPCPrefab);}
        #endif
        if(FindObjectOfType<SteamManager>()==null){Instantiate(steamManagerPrefab);}
        if(FindObjectOfType<StatsAchievsManager>()==null){Instantiate(statsAchievsManagerPrefab);}
        
        if(FindObjectOfType<GameRules>()==null&&GameManager.instance.gamemodeSelected>0&&(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){
            Instantiate(GameManager.instance.GetGameRulesCurrent());}
        if(FindObjectOfType<GameRules>()==null&&GameManager.instance.gamemodeSelected==-1&&SceneManager.GetActiveScene().name=="Game"){Instantiate(adventureGamerulesPrefab);GameRules.instance.ReplaceAdventureZoneInfo(adventureZones[GameManager.instance.zoneSelected].gameRules);}
        if(FindObjectOfType<GameRules>()==null&&SceneManager.GetActiveScene().name=="SandboxMode"){
            GameRules gr=Instantiate(gamerulesetsPrefabs[0]);gr.gameObject.name="GRSandbox";gr.cfgName="Sandbox Mode";gr.cfgDesc="New Sandbox Mode Savefile!";gr.cfgIconsGo=null;gr.cfgIconAssetName="questionMark";}

        if(FindObjectOfType<PostProcessVolume>()!=null&& FindObjectOfType<SaveSerial>().settingsData.pprocessing!=true){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(FindObjectOfType<EventSystem>()!=null){if(FindObjectOfType<EventSystem>().GetComponent<UIInputSystem>()==null)FindObjectOfType<EventSystem>().gameObject.AddComponent<UIInputSystem>();}
        if(FindObjectOfType<Jukebox>()==null&&SceneManager.GetActiveScene().name=="Menu"){Instantiate(jukeboxPrefab);}
        //yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
    }

    public static int GetGamerulesetsPrefabsLength(){return CoreSetup.instance._gamerulesetsPrefabsLength;}
    public GameObject GetJukeboxPrefab(){return jukeboxPrefab;}
}
