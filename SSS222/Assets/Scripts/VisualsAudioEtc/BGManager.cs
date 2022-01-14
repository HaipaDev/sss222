using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGManager : MonoBehaviour{
    [SerializeField]Material defMat;
    [SerializeField]Material hardcoreMat;
    [SerializeField]Material classicMat;
    IEnumerator Start(){
        /*if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Game"){*/yield return new WaitForSecondsRealtime(0.075f);//}else{}

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name!="Loading"){
            if(GameSession.instance.CheckGamemodeSelected("Hardcore")){SetColorMat(hardcoreMat);}
            else if(GameSession.instance.CheckGamemodeSelected("Classic")){SetColorMat(classicMat);}
            else{SetColorMat(defMat);}
        }
    }
    public void SetColorMat(Material mat){
        foreach(Tag_BGColor t in transform.GetComponentsInChildren<Tag_BGColor>()){ 
            t.GetComponent<Renderer>().material=mat; 
        } 
    }
}
