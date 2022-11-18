using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class GameCanvas : MonoBehaviour{    public static GameCanvas instance;
    float popupSumTime=0.25f;
    [SerializeField] List<HUDAlignment> upscaledHud;
    [SerializeField] List<HUDAlignment> classicHud;
    [SceneObjectsOnly][SerializeField] GameObject hpPopup;
    [DisableInEditorMode][SerializeField] float hpCount;
    [DisableInEditorMode][SerializeField] float hpTimer;
    [SceneObjectsOnly][SerializeField] GameObject hpAbsorpPopup;
    [DisableInEditorMode][SerializeField] float hpAbsorpCount;
    [DisableInEditorMode][SerializeField] float hpAbsorpTimer;
    [SceneObjectsOnly][SerializeField] GameObject enPopup;
    [DisableInEditorMode][SerializeField] float enCount;
    [DisableInEditorMode][SerializeField] float enTimer;
    [SceneObjectsOnly][SerializeField] GameObject enAbsorpPopup;
    [DisableInEditorMode][SerializeField] float enAbsorpCount;
    [DisableInEditorMode][SerializeField] float enAbsorpTimer;
    [SceneObjectsOnly][SerializeField] GameObject coinPopup;
    [DisableInEditorMode][SerializeField] float coinCount;
    [DisableInEditorMode][SerializeField] float coinTimer;
    [SceneObjectsOnly][SerializeField] GameObject corePopup;
    [DisableInEditorMode][SerializeField] float coreCount;
    [DisableInEditorMode][SerializeField] float coreTimer;
    [SceneObjectsOnly][SerializeField] GameObject xpPopup;
    [DisableInEditorMode][SerializeField] float xpCount;
    [DisableInEditorMode][SerializeField] float xpTimer;
    [SceneObjectsOnly][SerializeField] GameObject scPopup;
    [DisableInEditorMode][SerializeField] float scCount;
    [DisableInEditorMode][SerializeField] float scTimer;
    [SceneObjectsOnly][SerializeField] GameObject ammoPopup;
    [DisableInEditorMode][SerializeField] float ammoCount;
    [DisableInEditorMode][SerializeField] float ammoTimer;
    void Awake(){if(GameCanvas.instance!=null){Destroy(gameObject);}else{instance=this;}}
    void Start(){
        ChangeHUDAligment();
    }
    //int _hudAlignLastSet=-1;
    public void ChangeHUDAligment(){
        if(_canUpscaleHud()){//&&_hudAlignLastSet!=2){
            foreach(HUDAlignment hudAl in upscaledHud){
                _changeAlignment(hudAl);
            }
            //_hudAlignLastSet=2;

            /*foreach(RectTransform t in rescale16by9){
                t.localScale=new Vector2(2,2);
                t.anchoredPosition=new Vector2(t.anchoredPosition.x*2,t.anchoredPosition.y*2);
            }
            foreach(RectTransformAndPos rt in rescaleAndMove16by9){
                rt.trans.anchorMin=new Vector2(0,1);rt.trans.anchorMax=new Vector2(0,1);
                rt.trans.localScale=new Vector2(2,2);
                rt.trans.anchoredPosition=new Vector2(rt.pos.x,rt.pos.y);
            }
            foreach(RectTransformAndPos rt in onlyMove16by9){
                rt.trans.anchoredPosition=new Vector2(rt.pos.x,rt.pos.y);
            }
            foreach(RectTransformAlign rt in changeAlignment16by9){
                rt.trans.GetComponent<LayoutGroup>().childAlignment=rt.align;
            }*/

        }else{// if(!_canUpscaleHud()&&_hudAlignLastSet!=1){
            foreach(HUDAlignment hudAl in classicHud){
                _changeAlignment(hudAl);
                //rt.trans.anchoredPosition=new Vector2(rt.pos.x,rt.pos.y);
            }
            //_hudAlignLastSet=1;
        }
        void _changeAlignment(HUDAlignment hudAl){
            if(hudAl.trans!=null){
                if(hudAl.changeAlign)hudAl.trans.GetComponent<RectTransform>().SetAnchor(hudAl.align);
                var scaleFactor=GetComponent<Canvas>().scaleFactor;Debug.Log(scaleFactor);
                if(hudAl.scale!=1)scaleFactor=0;
                if(hudAl.pos.x!=0||hudAl.pos.y!=0)hudAl.trans.anchoredPosition=new Vector2(hudAl.pos.x,hudAl.pos.y);
                if(hudAl.scale!=0){
                    hudAl.trans.localScale=new Vector2(hudAl.scale,hudAl.scale);
                    if(hudAl.multiplyPosByScale)hudAl.trans.anchoredPosition=new Vector2(hudAl.trans.anchoredPosition.x*hudAl.scale,hudAl.trans.anchoredPosition.y*hudAl.scale);
                }/*if((hudAl.pos.x!=0&&hudAl.pos.y!=0)&&(hudAl.scale!=0)){
                    hudAl.trans.anchorMin=new Vector2(0,1);hudAl.trans.anchorMax=new Vector2(0,1);
                }*/
                //hudAl.trans.anchorMin=new Vector2(0,1);hudAl.trans.anchorMax=new Vector2(0,1);
                if(hudAl.changeLayoutGroupAlign)hudAl.trans.GetComponent<LayoutGroup>().childAlignment=hudAl.layoutGroupAlign;
            }
        }
    }
    public static bool _canUpscaleHud(){return (Camera.main.aspect>=1.33&&!SaveSerial.instance.settingsData.classicHUD&&!GameManager.instance.CheckGamemodeSelected("Classic"));}
    //public static bool _isClassicHud(){return (SaveSerial.instance.settingsData.classicHUD||GameManager.instance.CheckGamemodeSelected("Classic"));}
    void Update(){
        //ChangeHUDAligment();

        if(SaveSerial.instance!=null)if(popupSumTime!=SaveSerial.instance.settingsData.popupSumTime){popupSumTime=SaveSerial.instance.settingsData.popupSumTime;}

        if(hpPopup!=null&&hpCount!=0){string symbol="-";if(hpCount>0){symbol="+";}hpPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(hpCount,1)).ToString();}

        if(hpAbsorpPopup!=null&&hpAbsorpCount!=0){string symbol="-";if(hpAbsorpCount>0){symbol="+";}hpAbsorpPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(hpAbsorpCount,1)).ToString();}

        if(enPopup!=null&&enCount!=0){string symbol="-";if(enCount>0){symbol="+";}enPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(enCount,1)).ToString();}

        if(enAbsorpPopup!=null&&enAbsorpCount!=0){string symbol="-";if(enAbsorpCount>0){symbol="+";}enAbsorpPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(enAbsorpCount,1)).ToString();}

        if(coinPopup!=null&&coinCount!=0){string symbol="-";if(coinCount>0){symbol="+";}coinPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(coinCount,1)).ToString();}

        if(corePopup!=null&&coreCount!=0){string symbol="-";if(coreCount>0){symbol="+";}corePopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(coreCount,1)).ToString();}

        if(xpPopup!=null&&xpCount!=0){string symbol="-";if(xpCount>0){symbol="+";}xpPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(xpCount,1)).ToString();}

        if(scPopup!=null&&scCount!=0){string symbol="-";if(scCount>0){symbol="+";}scPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(scCount,1)).ToString();}

        if(ammoPopup!=null&&ammoCount!=0){string symbol="-";if(ammoCount>0){symbol="+";}ammoPopup.GetComponentInChildren<TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(ammoCount,1)).ToString();}

    if(!GameManager.GlobalTimeIsPaused){
        if(hpTimer>0){hpTimer-=Time.unscaledDeltaTime;}
        if(hpAbsorpTimer>0){hpAbsorpTimer-=Time.unscaledDeltaTime;}
        if(enTimer>0){enTimer-=Time.unscaledDeltaTime;}
        if(enAbsorpTimer>0){enAbsorpTimer-=Time.unscaledDeltaTime;}
        if(coinTimer>0){coinTimer-=Time.unscaledDeltaTime;}
        if(coreTimer>0){coreTimer-=Time.unscaledDeltaTime;}
        if(xpTimer>0){xpTimer-=Time.unscaledDeltaTime;}
        if(scTimer>0){scTimer-=Time.unscaledDeltaTime;}
        if(ammoTimer>0){ammoTimer-=Time.unscaledDeltaTime;}
    }
    }

    public void HpPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.hpPopupsSum){HpDispCount(amnt);}else{HpPopUpHUD(amnt);}}
    void HpDispCount(float amnt){
        if(hpTimer<=0){hpCount=0;}
        hpTimer=popupSumTime;
        if(hpTimer>0){if((hpCount==0)||(amnt<0&&hpCount<0)||(amnt>0&&hpCount>0)){}else{hpCount=0;}hpCount+=amnt;if(hpCount!=0)HpPopUpHUD(hpCount);}
    }
    void HpPopUpHUD(float amnt){
        if(hpPopup!=null){
        hpPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        hpPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("HpPopUpHUD not present");}
    }
    public void HpAbsorpPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.hpAbsorpPopupsSum){HpAbsorpDispCount(amnt);}else{HpAbsorpPopUpHUD(amnt);}}
    void HpAbsorpDispCount(float amnt){
        if(hpAbsorpTimer<=0){hpAbsorpCount=0;}
        hpAbsorpTimer=popupSumTime;
        if(hpAbsorpTimer>0){if((hpAbsorpCount==0)||(amnt<0&&hpAbsorpCount<0)||(amnt>0&&hpAbsorpCount>0)){}else{hpAbsorpCount=0;}hpAbsorpCount+=amnt;if(hpAbsorpCount!=0)HpAbsorpPopUpHUD(hpAbsorpCount);}
    }
    void HpAbsorpPopUpHUD(float amnt){
        if(hpAbsorpPopup!=null){
        hpAbsorpPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        hpAbsorpPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("hpAbsorpPopUpHUD not present");}
    }

    public void EnPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.enPopupsSum){EnDispCount(amnt);}else{EnPopUpHUD(amnt);}}
    void EnDispCount(float amnt){
        if(enTimer<=0){enCount=0;}
        enTimer=popupSumTime;
        if(enTimer>0){if((enCount==0)||(amnt<0&&enCount<0)||(amnt>0&&enCount>0)){}else{enCount=0;}enCount+=amnt;if(enCount!=0)EnPopUpHUD(enCount);}
    }
    public void EnPopUpHUD(float amnt){
        if(enPopup!=null){
        enPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        if(Player.instance!=null&&(!Player.instance._hasStatus("infenergy"))||(Player.instance._hasStatus("infenergy")&&symbol=="+")){
        enPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();}
        }else{Debug.LogWarning("EnergyPopUpHUD not present");}
    }
    public void EnAbsorpPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.enAbsorpPopupsSum){EnAbsorpDispCount(amnt);}else{EnAbsorpPopUpHUD(amnt);}}
    void EnAbsorpDispCount(float amnt){
        if(enAbsorpTimer<=0){enAbsorpCount=0;}
        enAbsorpTimer=popupSumTime;
        if(enAbsorpTimer>0){if((enAbsorpCount==0)||(amnt<0&&enAbsorpCount<0)||(amnt>0&&enAbsorpCount>0)){}else{enAbsorpCount=0;}enAbsorpCount+=amnt;if(enAbsorpCount!=0)EnAbsorpPopUpHUD(enAbsorpCount);}
    }
    void EnAbsorpPopUpHUD(float amnt){
        if(enAbsorpPopup!=null){
        enAbsorpPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        enAbsorpPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("enAbsorpPopUpHUD not present");}
    }

    public void CoinPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.coinPopupsSum){CoinDispCount(amnt);}else{CoinPopUpHUD(amnt);}}
    void CoinDispCount(float amnt){
        if(coinTimer<=0){coinCount=0;}
        coinTimer=popupSumTime;
        if(coinTimer>0){if((coinCount==0)||(amnt<0&&coinCount<0)||(amnt>0&&coinCount>0)){}else{coinCount=0;}coinCount+=amnt;if(coinCount!=0)CoinPopUpHUD(coinCount);}
    }
    void CoinPopUpHUD(float amnt){
        if(coinPopup!=null){
        coinPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        coinPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("CoinsPopUpHUD not present");}
    }

    public void CorePopupSwitch(float amnt){if(SaveSerial.instance.settingsData.corePopupsSum){CoreDispCount(amnt);}else{CorePopUpHUD(amnt);}}
    void CoreDispCount(float amnt){
        if(coreTimer<=0){coreCount=0;}
        coreTimer=popupSumTime;
        if(coreTimer>0){if((coreCount==0)||(amnt<0&&coreCount<0)||(amnt>0&&coreCount>0)){}else{coreCount=0;}coreCount+=amnt;if(coreCount!=0)CorePopUpHUD(coreCount);}
    }
    void CorePopUpHUD(float amnt){
        if(corePopup!=null){
        corePopup.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        corePopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("CoresPopUpHUD not present");}
    }

    public void XpPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.xpPopupsSum){XpDispCount(amnt);}else{XPPopUpHUD(amnt);}}
    void XpDispCount(float amnt){
        if(xpTimer<=0){xpCount=0;}
        xpTimer=popupSumTime;
        if(xpTimer>0){if((xpCount==0)||(amnt<0&&xpCount<0)||(amnt>0&&xpCount>0)){}else{xpCount=0;}xpCount+=amnt;if(xpCount!=0)XPPopUpHUD(xpCount);}
    }
    public void XPPopUpHUD(float amnt){
        if(xpPopup!=null){
        xpPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //xppopupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        xpPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("XPPopUpHUD not present");}
    }

    public void ScorePopupSwitch(float amnt){if(SaveSerial.instance.settingsData.scorePopupsSum){ScoreDispCount(amnt);}else{ScorePopUpHUD(amnt);}}
    void ScoreDispCount(float amnt){
        if(scTimer<=0){scCount=0;}
        scTimer=popupSumTime;
        if(scTimer>0){if((scCount==0)||(amnt<0&&scCount<0)||(amnt>0&&scCount>0)){}else{scCount=0;}scCount+=amnt;if(scCount!=0)ScorePopUpHUD(scCount);}
    }
    void ScorePopUpHUD(float amnt){
        if(scPopup!=null){
        scPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //scpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        scPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("ScorePopUpHUD not present");}
    }

    public void AmmoPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.ammoPopupsSum){AmmoDispCount(amnt);}else{AmmoPopUpHUD(amnt);}}
    void AmmoDispCount(float amnt){
        if(ammoTimer<=0){ammoCount=0;}
        ammoTimer=popupSumTime;
        if(ammoTimer>0){if((ammoCount==0)||(amnt<0&&ammoCount<0)||(amnt>0&&ammoCount>0)){}else{ammoCount=0;}ammoCount+=amnt;if(ammoCount!=0)AmmoPopUpHUD(ammoCount);}
    }
    void AmmoPopUpHUD(float amnt){
        if(ammoPopup!=null){
        ammoPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        if(Player.instance!=null&&(!Player.instance._hasStatus("infenergy"))||(Player.instance._hasStatus("infenergy")&&symbol=="+")){
        ammoPopup.GetComponentInChildren<TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        //energyUsedCount+=Mathf.Abs(amnt);
        }
        }else{Debug.LogWarning("AmmoPopUpHUD not present");}
    }
}