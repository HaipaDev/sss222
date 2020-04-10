using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCreator : MonoBehaviour{
    //Create important managers if they don't exist ;)
    [SerializeField] GameObject gameSessionPrefab;
    [SerializeField] GameObject loaderPrefab;
    [SerializeField] GameObject saveSerialPrefab;
    [SerializeField] GameObject gameAssetsPrefab;
    private void Awake()
    {
        if (FindObjectOfType<GameSession>() == null){Instantiate(gameSessionPrefab);}
        //if (FindObjectOfType<Loader>() == null){Instantiate(loaderPrefab);}
        if (FindObjectOfType<SaveSerial>() == null){Instantiate(saveSerialPrefab);}
        if (FindObjectOfType<GameAssets>() == null){Instantiate(gameAssetsPrefab);}
        Destroy(gameObject);
    }
}
