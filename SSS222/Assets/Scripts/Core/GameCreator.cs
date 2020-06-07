using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameCreator : MonoBehaviour{
    //Create important managers if they don't exist ;)
    [SerializeField] GameObject gameSessionPrefab;
    [SerializeField] GameObject loaderPrefab;
    [SerializeField] GameObject saveSerialPrefab;
    [SerializeField] GameObject gameAssetsPrefab;
    [SerializeField] GameObject levelPrefab;
    [SerializeField] GameObject audioManagerPrefab;
    private void Awake()
    {
        if (FindObjectOfType<GameSession>() == null){Instantiate(gameSessionPrefab);}
        //if (FindObjectOfType<Loader>() == null){Instantiate(loaderPrefab);}
        if (FindObjectOfType<SaveSerial>() == null){Instantiate(saveSerialPrefab);}
        if (FindObjectOfType<GameAssets>() == null){Instantiate(gameAssetsPrefab);}
        if (FindObjectOfType<Level>() == null){Instantiate(levelPrefab);}
        if (FindObjectOfType<AudioManager>() == null){Instantiate(audioManagerPrefab);}
        if (FindObjectOfType<PostProcessVolume>() != null && FindObjectOfType<SaveSerial>().pprocessing!=true){Destroy(FindObjectOfType<PostProcessVolume>());}
        Destroy(gameObject);
    }
}
