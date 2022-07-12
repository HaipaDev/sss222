using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class GameCreator : MonoBehaviour{   public static GameCreator instance;
    [Header("Main managers")]
    [AssetsOnly][SerializeField] GameObject saveSerialPrefab;
    [AssetsOnly][SerializeField] GameObject easySavePrefab;
    [AssetsOnly][SerializeField] GameObject gsceneManagerPrefab;
    [AssetsOnly][SerializeField] GameObject gameSessionPrefab;
    
    [Header("Assets managers")]
    [AssetsOnly][SerializeField] GameObject gameAssetsPrefab;
    [AssetsOnly][SerializeField] GameObject audioManagerPrefab;
    
    [Header("Networking, Advancements etc")]
    [AssetsOnly][SerializeField] GameObject dbaccessPrefab;
    [AssetsOnly][SerializeField] GameObject discordPresencePrefab;
    [AssetsOnly][SerializeField] GameObject steamManagerPrefab;
    [AssetsOnly][SerializeField] GameObject statsAchievsManagerPrefab;
    [Header("Game Rulesets")]
    //[SerializeField] int gamerulesetsID;
    [AssetsOnly][SerializeField] public GameRules[] gamerulesetsPrefabs;
    [ReadOnly][SerializeField] public int _gamerulesetsPrefabsLength;
    [AssetsOnly][SerializeField] public GameRules adventureGamerulesPrefab;
    [AssetsOnly][SerializeField] public List<AdventureZoneData> adventureZones;
    [AssetsOnly][SerializeField] public GameRules adventureTravelZonePrefab;
    private void Awake(){
        instance=this;
        if(SceneManager.GetActiveScene().name=="Loading")LoadPre();
        else Load();
    }
    void LoadPre(){
        if(FindObjectOfType<SaveSerial>()==null){Instantiate(saveSerialPrefab);}
        if(FindObjectOfType<ES3ReferenceMgr>()==null){Instantiate(easySavePrefab);}
        if(FindObjectOfType<GSceneManager>()==null){var go=Instantiate(gsceneManagerPrefab);go.GetComponent<GSceneManager>().enabled=true;}
            /*Idk it disables itself so I guess Ill turn it on manually*/
        if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
    }
    void Load(){
        LoadPre();
        if(FindObjectOfType<GameSession>()==null){Instantiate(gameSessionPrefab);}

        if(FindObjectOfType<GameAssets>()==null){Instantiate(gameAssetsPrefab);}
        if(FindObjectOfType<AudioManager>()==null){Instantiate(audioManagerPrefab);}

        //if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
        if(FindObjectOfType<DiscordPresence.PresenceManager>()==null){Instantiate(discordPresencePrefab);}
        if(FindObjectOfType<SteamManager>()==null){Instantiate(steamManagerPrefab);}
        if(FindObjectOfType<StatsAchievsManager>()==null){Instantiate(statsAchievsManagerPrefab);}
        
        if(FindObjectOfType<GameRules>()==null&&GameSession.instance.gamemodeSelected>0&&(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){
            Instantiate(GameSession.instance.GetGameRulesCurrent());}
        if(FindObjectOfType<GameRules>()==null&&GameSession.instance.gamemodeSelected==-1){Instantiate(adventureGamerulesPrefab);GameRules.instance.ReplaceAdventureZoneInfo(adventureZones[GameSession.instance.zoneSelected].gameRules);}
        if(FindObjectOfType<GameRules>()==null&&SceneManager.GetActiveScene().name=="SandboxMode"){
            GameRules gr=Instantiate(gamerulesetsPrefabs[0]);gr.gameObject.name="GRSandbox";gr.cfgName="Sandbox Mode";gr.cfgDesc="New Sandbox Mode Savefile!";gr.cfgIconsGo=null;gr.cfgIconAssetName="questionMark";}

        if(FindObjectOfType<PostProcessVolume>()!=null&& FindObjectOfType<SaveSerial>().settingsData.pprocessing!=true){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(FindObjectOfType<EventSystem>()!=null){if(FindObjectOfType<EventSystem>().GetComponent<UIInputSystem>()==null)FindObjectOfType<EventSystem>().gameObject.AddComponent<UIInputSystem>();}
        //yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
    }

    public static int GetGamerulesetsPrefabsLength(){return GameCreator.instance._gamerulesetsPrefabsLength;}
}
