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
    void SetSandboxCanvasToModeInfo(){
        foreach(Transform t in panels[1].transform){Destroy(t.gameObject);}
        var _mainTransform=Instantiate(sandboxPanel,panels[1].transform).transform;
        _mainTransform.GetComponent<RectTransform>().localPosition=Vector2.zero;
        _mainTransform.GetComponent<RectTransform>().localScale=Vector2.one;
        _mainTransform.GetComponent<RectTransform>().sizeDelta=Vector2.one;
        _mainTransform.GetComponent<RectTransform>().anchoredPosition=new Vector2(0,0);
        _mainTransform.GetComponent<RectTransform>().anchorMin=new Vector2(0,0);
        _mainTransform.GetComponent<RectTransform>().anchorMax=new Vector2(1,1);
        _mainTransform.GetComponent<RectTransform>().pivot=new Vector2(0.5f,0.5f);
        
        /*Destroy(_mainTransform.GetChild(1).GetChild(9).gameObject);
        Destroy(_mainTransform.GetChild(1).GetChild(8).gameObject);
        Destroy(_mainTransform.GetChild(1).GetChild(7).gameObject);
        Destroy(_mainTransform.GetChild(1).GetChild(3).gameObject);
        Destroy(_mainTransform.GetChild(1).GetChild(2).gameObject);
        Destroy(_mainTransform.GetChild(1).GetChild(1).gameObject);*/
        //Change the OpenPresetAppearance
        Destroy(_mainTransform.GetChild(1).GetChild(0).GetComponent<Button>());
        Destroy(_mainTransform.GetChild(1).GetChild(0).GetComponent<HoverUIEnable>());
        if(GameRules.instance.cfgIconsGo!=null){
            Destroy(_mainTransform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).gameObject);
            Instantiate(GameRules.instance.cfgIconsGo,_mainTransform.GetChild(1).GetChild(0).GetChild(0));
        }else{_mainTransform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite=AssetsManager.instance.SprAny(GameRules.instance.cfgIconAssetName);}
        //Destroy(_mainTransform.GetChild(1).GetChild(0).GetChild(2).gameObject);//Build version txt
        Destroy(_mainTransform.GetChild(1).GetChild(0).GetChild(1).GetComponent<ValueDisplay>());//Title
        _mainTransform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text=GameRules.instance.cfgName;

        /*Destroy(_mainTransform.GetChild(4).GetChild(2).GetComponent<HoverUIEnable>());//Change ScoreDisplay Note
        Destroy(_mainTransform.GetChild(4).GetChild(2).GetChild(1).gameObject);
        */
        
        //yield return new WaitForSecondsRealtime(0.02f);
        foreach(Button bt in _mainTransform.GetComponent<SandboxCanvas>()._powerupInventoryGameObject().transform.GetChild(0).GetComponentsInChildren<Button>(true)){bt.interactable=false;}
        for(var i=2;i<_mainTransform.childCount;i++){//skip Default Panel
            var _transform=_mainTransform.GetChild(i);
            foreach(Tag_SandboxEditableButton c in _transform.GetComponentsInChildren<Tag_SandboxEditableButton>(true)){c.GetComponent<Button>().interactable=false;}
            foreach(Toggle c in _transform.GetComponentsInChildren<Toggle>(true)){c.interactable=false;}
            foreach(Slider c in _transform.GetComponentsInChildren<Slider>(true)){c.interactable=false;}
            foreach(TMP_InputField c in _transform.GetComponentsInChildren<TMP_InputField>(true)){c.interactable=false;c.readOnly=true;}
            foreach(TMP_Dropdown c in _transform.GetComponentsInChildren<TMP_Dropdown>(true)){c.interactable=false;}
            foreach(Tag_SandboxDefaultButton t in _transform.GetComponentsInChildren<Tag_SandboxDefaultButton>()){Destroy(t.gameObject);}
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
        if(SandboxCanvas.instance!=null)if(SandboxCanvas.instance.gameObject.activeInHierarchy)SandboxCanvas.instance.transform.GetChild(0).gameObject.SetActive(!b);
    }
    public void SetActivePanel(int i){foreach(GameObject p in panels){p.SetActive(false);}panels[i].SetActive(true);panelActive=i;_escapeDelay=0.4f;}
    public void OpenNextPanel(){if(panelActive<panels.Length-1)SetActivePanel(panelActive+1);else SetActivePanel(0);}
    public void OpenPrevPanel(){if(panelActive>0)SetActivePanel(panelActive-1);else SetActivePanel(panels.Length-1);}
}
