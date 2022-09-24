using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class AchievListCanvas : MonoBehaviour{
    [SceneObjectsOnly][SerializeField] GameObject listObject;
    [AssetsOnly][SerializeField] GameObject elementPrefab;
    [DisableInEditorMode][SerializeField] int hiddenAchievsCount;
    void Start(){var mngr=StatsAchievsManager.instance;
        if(mngr!=null){
            Destroy(listObject.transform.GetChild(0).gameObject);
            foreach(Achievement a in mngr.achievsList){if(a._isCompleted())CreateAchievElement(a);}
            foreach(Achievement a in mngr.achievsList){if(!a._isCompleted()&&!a.hidden)CreateAchievElement(a);}
            foreach(Achievement a in mngr.achievsList){if(!a._isCompleted()&&a.hidden)hiddenAchievsCount++;}if(hiddenAchievsCount>0)CreateHiddenAchievsElement();
        }
    }
    GameObject CreateAchievElement(Achievement a){
        GameObject go=Instantiate(elementPrefab,listObject.transform);
        go.GetComponent<AchievListElement>().SetName(a.displayName);
        go.GetComponent<AchievListElement>().SetDesc(a.desc);
        go.GetComponent<AchievListElement>().SetIcon(a.iconInc);
        if(a._isCompleted())go.GetComponent<AchievListElement>().SetIcon(a.icon);
        go.GetComponent<AchievListElement>().SetEpic(a.epic);
        go.GetComponent<AchievListElement>().completed=a._isCompleted();
        return go;
    }
    GameObject CreateHiddenAchievsElement(){
        GameObject go=Instantiate(elementPrefab,listObject.transform);
        go.GetComponent<AchievListElement>().SetName("???");
        go.GetComponent<AchievListElement>().SetDesc(hiddenAchievsCount+"x Hidden achievements");
        go.GetComponent<AchievListElement>().SetIcon(AssetsManager.instance.Spr("AchievHidden"));
        return go;
    }
}
