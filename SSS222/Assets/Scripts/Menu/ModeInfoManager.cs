using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class ModeInfoManager : MonoBehaviour{
    [SerializeField] int panelActive=0;
    [SerializeField] GameObject[] panels;
    [SerializeField] Image startIMG;
    [SerializeField] Sprite[] startSprites;
    [AssetsOnly][SerializeField] GameObject sandboxPanel;
    float _escapeDelay;
    void Start(){
        SetActivePanel(0);
        startIMG.sprite=startSprites[GameManager.instance.gamemodeSelected-1];

        SetSandboxCanvasToModeInfo();
    }
    void SetSandboxCanvasToModeInfo(){//StartCoroutine(SetSandboxCanvasToModeInfoI());}
    //IEnumerator SetSandboxCanvasToModeInfoI(){
        foreach(Transform t in panels[1].transform){Destroy(t.gameObject);}
        //foreach(Transform t in sandboxPanel.transform){Instantiate(t.gameObject,_mainTransform);}
        var _mainTransform=Instantiate(sandboxPanel,panels[1].transform).transform;
        
        for(var i=_mainTransform.GetChild(1).childCount-1;i>0;i--){if(_mainTransform.GetChild(1).GetChild(i).name!="ButtonsGrid")Destroy(_mainTransform.GetChild(1).GetChild(i).gameObject);}
        _mainTransform.GetChild(1).GetChild(0).GetComponent<ValueDisplay>().value="cfgNameCurrent";
        _mainTransform.GetChild(1).GetChild(0).GetComponent<Button>().interactable=false;
        
        //yield return new WaitForSecondsRealtime(0.02f);
        foreach(Button bt in _mainTransform.GetComponent<SandboxCanvas>()._powerupInventoryGameObject().transform.GetChild(0).GetComponentsInChildren<Button>(true)){bt.interactable=false;}
        for(var i=2;i<_mainTransform.childCount;i++){//skip Default Panel
            var _transform=_mainTransform.GetChild(i);
            foreach(Button c in _transform.GetComponentsInChildren<Button>(true)){
                if(c.transform.name=="MoveAxisButton"||c.transform.name=="ScoreImg"){
                    c.interactable=false;
                }
            }
            foreach(Toggle c in _transform.GetComponentsInChildren<Toggle>(true)){c.interactable=false;}
            foreach(Slider c in _transform.GetComponentsInChildren<Slider>(true)){c.interactable=false;}
            foreach(TMP_InputField c in _transform.GetComponentsInChildren<TMP_InputField>(true)){c.interactable=false;c.readOnly=true;}
            foreach(TMP_Dropdown c in _transform.GetComponentsInChildren<TMP_Dropdown>(true)){c.interactable=false;}
        }
    }
    void Update(){
        if(_escapeDelay>0){_escapeDelay-=Time.unscaledDeltaTime;}
        else{if(panelActive==1){
                if(panels[1].transform.GetChild(0).GetChild(1).gameObject.activeSelf){SwitchBackButtonAndArrows(true);if(GSceneManager.EscPressed()){GSceneManager.instance.LoadGameModeChooseScene();}}
                else{SwitchBackButtonAndArrows(false);}
            }else{SwitchBackButtonAndArrows(true);}
        }
        CheckESC();
    }
    void CheckESC(){
        if(Input.GetKeyDown(KeyCode.Escape)&&_escapeDelay<=0){
            if(panelActive!=1&&panelActive!=0){SetActivePanel(0);}
            else if(panelActive==0){GSceneManager.instance.LoadGameModeChooseScene();}
        }
    }
    void SwitchBackButtonAndArrows(bool b){
        transform.GetChild(0).gameObject.SetActive(b);
        transform.GetChild(1).gameObject.SetActive(b);
        transform.GetChild(2).gameObject.SetActive(b);
    }
    public void SetActivePanel(int i){foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);panelActive=i;_escapeDelay=0.4f;}
    public void OpenNextPanel(){if(panelActive<panels.Length-1)SetActivePanel(panelActive+1);else SetActivePanel(0);}
    public void OpenPrevPanel(){if(panelActive>0)SetActivePanel(panelActive-1);else SetActivePanel(panels.Length-1);}
}
