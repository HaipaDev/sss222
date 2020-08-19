using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour{
    public bool moveByMouse = true;
    private void Awake(){
        SetUpSingleton();
    }

    private void SetUpSingleton(){
        int numberOfObj = FindObjectsOfType<Settings>().Length;
        if (numberOfObj > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
