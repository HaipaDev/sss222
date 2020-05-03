using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour{
    public SaveData activeSave;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Save(){


    }
    public void Load(){

        
    }
}

[System.Serializable]
public class SaveData{
    public string saveName;
    public int highscore;

}