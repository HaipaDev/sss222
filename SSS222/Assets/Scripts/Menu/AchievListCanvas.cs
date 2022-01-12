using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AchievListCanvas : MonoBehaviour{
    [SceneObjectsOnly][SerializeField] GameObject listObject;
    [AssetsOnly][SerializeField] GameObject elementPrefab;
    void Start(){var mngr=StatsAchievsManager.instance;
        if(mngr!=null){
            foreach(Achievement a in mngr.achievsList){if(a.completed)CreateAchievElement(a);}
            foreach(Achievement a in mngr.achievsList){if(!a.completed)CreateAchievElement(a);}
        }
    }
    GameObject CreateAchievElement(Achievement a){
        GameObject go=Instantiate(elementPrefab,listObject.transform);
        go.GetComponent<AchievListElement>().SetName(a.name);
        go.GetComponent<AchievListElement>().SetDesc(a.desc);
        go.GetComponent<AchievListElement>().SetIcon(a.icon);
        go.GetComponent<AchievListElement>().completed=a.completed;
        return go;
    }
}
