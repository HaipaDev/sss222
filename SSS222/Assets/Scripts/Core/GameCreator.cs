using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class GameCreator : MonoBehaviour{
    public static GameCreator instance;
    [AssetsOnly][SerializeField] GameObject gameSessionPrefab;
    [AssetsOnly][SerializeField] GameObject loaderPrefab;
    [AssetsOnly][SerializeField] GameObject saveSerialPrefab;
    [AssetsOnly][SerializeField] public GameObject gameAssetsPrefab;
    [AssetsOnly][SerializeField] GameObject gSceneManagerPrefab;
    [AssetsOnly][SerializeField] GameObject audioManagerPrefab;
    [AssetsOnly][SerializeField] GameObject dbaccessPrefab;
    [AssetsOnly][SerializeField] GameObject discordPresencePrefab;
    //[SerializeField] int gamerulesetsID;
    [AssetsOnly][SerializeField] public GameRules[] gamerulesetsPrefabs;
    [AssetsOnly][SerializeField] public GameRules[] adventureZonesPrefabs;
    private void Awake(){
        instance=this;
        Load();
        //StartCoroutine(LoadI());
    }
    //IEnumerator LoadI(){
    void Load(){
        if(FindObjectOfType<GameSession>()==null){Instantiate(gameSessionPrefab);}
        //if(FindObjectOfType<Loader>()==null){Instantiate(loaderPrefab);}
        if(FindObjectOfType<SaveSerial>()==null){Instantiate(saveSerialPrefab);}
        if(FindObjectOfType<GameAssets>()==null){Instantiate(gameAssetsPrefab);}
        if(FindObjectOfType<GSceneManager>()==null){Instantiate(gSceneManagerPrefab);}
        if(FindObjectOfType<AudioManager>()==null){Instantiate(audioManagerPrefab);}
        if(FindObjectOfType<DBAccess>()==null){Instantiate(dbaccessPrefab);}
        if(FindObjectOfType<DiscordPresence.PresenceManager>()==null){Instantiate(discordPresencePrefab);}
        if(FindObjectOfType<GameRules>()==null&&(SceneManager.GetActiveScene().name=="Game"||SceneManager.GetActiveScene().name=="InfoGameMode")){Instantiate(GameCreator.instance.gamerulesetsPrefabs[GameSession.instance.gameModeSelected]);}
        //if(FindObjectOfType<GameRules>()==null&&SceneManager.GetActiveScene().name=="SandboxMenu"){Instantiate(gamerulesetsPrefabs[4].GetGamerules());}
        if(FindObjectOfType<PostProcessVolume>()!=null&& FindObjectOfType<SaveSerial>().settingsData.pprocessing!=true){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
        if(FindObjectOfType<EventSystem>()!=null){if(FindObjectOfType<EventSystem>().GetComponent<UIInputSystem>()==null)FindObjectOfType<EventSystem>().gameObject.AddComponent<UIInputSystem>();}
        //yield return new WaitForSeconds(0.5f);
        //Destroy(gameObject);
    }
}
