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
    [AssetsOnly][SerializeField] GameObject gsceneManagerPrefab;
    [AssetsOnly][SerializeField] GameObject gameSessionPrefab;
    
    [Header("Assets managers")]
    [AssetsOnly][SerializeField] public GameObject gameAssetsPrefab;
    [AssetsOnly][SerializeField] GameObject audioManagerPrefab;
    [Header("Networking, Advancements etc")]
    [AssetsOnly][SerializeField] GameObject dbaccessPrefab;
    [AssetsOnly][SerializeField] GameObject discordPresencePrefab;
    [AssetsOnly][SerializeField] GameObject statsAchievsManagerPrefab;
    [Header("Game Rulesets")]
    //[SerializeField] int gamerulesetsID;
    [AssetsOnly][SerializeField] public GameRules[] gamerulesetsPrefabs;
    [AssetsOnly][SerializeField] public int _gamerulesetsPrefabsLength;
    [AssetsOnly][SerializeField] public GameRules[] adventureZonesPrefabs;
    private void Awake(){
        instance=this;
        if(SceneManager.GetActiveScene().name=="Loading")LoadPre();
        else Load();
    }
    void LoadPre(){
        if(FindObjectOfType<SaveSerial>()==null){Instantiate(saveSerialPrefab);}
        if(FindObjectOfType<GSceneManager>()==null){var go=Instantiate(gsceneManagerPrefab);go.GetComponent<GSceneManager>().enabled=true;}
            /*Idk it disables itself so I guess Ill turn it on manually*/
    }
    void Load(){
        LoadPre();
        if(FindObjectOfType<GameSession>()==null){Instantiate(gameSessionPrefab);}

        if(FindObjectOfType<GameAssets>()==null){Instantiate(gameAssetsPrefab);}
        if(FindObjectOfType<AudioManager>()==null){Instantiate(audioManagerPrefab);}

        if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
        if(FindObjectOfType<DiscordPresence.PresenceManager>()==null){Instantiate(discordPresencePrefab);}
        if(FindObjectOfType<StatsAchievsManager>()==null){Instantiate(statsAchievsManagerPrefab);}
        
        if(FindObjectOfType<GameRules>()==null&&(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){Instantiate(GameSession.instance.GetGameRulesCurrent());}
        //if(FindObjectOfType<GameRules>()==null&&SceneManager.GetActiveScene().name=="SandboxMenu"){Instantiate(gamerulesetsPrefabs[4].GetGamerules());}

        if(FindObjectOfType<PostProcessVolume>()!=null&& FindObjectOfType<SaveSerial>().settingsData.pprocessing!=true){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(FindObjectOfType<EventSystem>()!=null){if(FindObjectOfType<EventSystem>().GetComponent<UIInputSystem>()==null)FindObjectOfType<EventSystem>().gameObject.AddComponent<UIInputSystem>();}
        //yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
    }

    public static int GetGamerulesetsPrefabsLength(){return GameCreator.instance._gamerulesetsPrefabsLength;}
}
