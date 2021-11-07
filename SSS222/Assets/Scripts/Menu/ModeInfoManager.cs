using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeInfoManager : MonoBehaviour{
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    [SerializeField] Image startIMG;
    [SerializeField] Sprite[] startSprites;
    void Start(){
        SetActivePanel(0);
        startIMG.sprite=startSprites[GameSession.instance.gameModeSelected];
    }
    void Update(){
        
    }
    public void SetActivePanel(int i){foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);panelActive=i;}
    public void OpenNextPanel(){if(panelActive<panels.Length-1)SetActivePanel(panelActive+1);else SetActivePanel(0);}
    public void OpenPrevPanel(){if(panelActive>0)SetActivePanel(panelActive-1);else SetActivePanel(panels.Length-1);}
}
