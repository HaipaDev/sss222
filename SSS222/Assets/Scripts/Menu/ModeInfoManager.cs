using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeInfoManager : MonoBehaviour{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject infoPanel;
    [SerializeField] Image startIMG;
    [SerializeField] Sprite[] startSprites;
    void Start(){
        OpenStartPanel();
        startIMG.sprite=startSprites[GameSession.instance.gameModeSelected];
    }
    void Update(){
        
    }
    public void OpenNextPanel(){if(startPanel.activeSelf){OpenInfoPanel();}else{OpenStartPanel();}}
    public void OpenPrevPanel(){if(!startPanel.activeSelf){OpenStartPanel();}else{OpenInfoPanel();}}
    public void OpenStartPanel(){startPanel.SetActive(true);infoPanel.SetActive(false);}
    public void OpenInfoPanel(){startPanel.SetActive(false);infoPanel.SetActive(true);}
}
