using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour{
    public float timer=1f;
    private void Start()
    {
        //FindObjectOfType<GameSession>().savableData.Load();
        FindObjectOfType<SaveSerial>().Load();
    }
    // Update is called once per frame
    void Update()
    {
        FindObjectOfType<SaveSerial>().Load();
        timer -= Time.deltaTime;
        if(timer<=0){ SceneManager.LoadScene("Menu"); }
    }
}
