using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour{
    public static GameCanvas instance;
    float popupSumTime=0.25f;
    GameObject hpPopup;
    [SerializeField] float hpCount;
    [SerializeField] float hpTimer;
    GameObject enPopup;
    [SerializeField] float enCount;
    [SerializeField] float enTimer;
    GameObject ammoPopup;
    [SerializeField] float ammoCount;
    [SerializeField] float ammoTimer;
    //[SerializeField] bool enCounted;
    GameObject xpPopup;
    [SerializeField] float xpCount;
    [SerializeField] float xpTimer;
    GameObject scPopup;
    [SerializeField] float scCount;
    [SerializeField] float scTimer;
    GameObject coinPopup;
    [SerializeField] float coinCount;
    [SerializeField] float coinTimer;
    GameObject corePopup;
    [SerializeField] float coreCount;
    [SerializeField] float coreTimer;
    void Awake(){
        instance=this;
        hpPopup=GameObject.Find("HPDiffParrent");
        enPopup=GameObject.Find("EnergyDiffParrent");
        ammoPopup=GameObject.Find("AmmoDiffParrent");
        xpPopup=GameObject.Find("XPDiffParrent");
        coinPopup=GameObject.Find("CoinsDiffParrent");
        corePopup=GameObject.Find("CoresDiffParrent");
        scPopup=GameObject.Find("ScoreDiffParrent");
    }
    void Update(){
        popupSumTime=SaveSerial.instance.settingsData.popupSumTime;
        if(hpPopup!=null&&hpCount!=0){string symbol="-";if(hpCount>0){symbol="+";}hpPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(hpCount,1)).ToString();}

        if(enPopup!=null&&enCount!=0){string symbol="-";if(enCount>0){symbol="+";}enPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(enCount,1)).ToString();}

        if(ammoPopup!=null&&ammoCount!=0){string symbol="-";if(ammoCount>0){symbol="+";}enPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(ammoCount,1)).ToString();}

        if(xpPopup!=null&&xpCount!=0){string symbol="-";if(xpCount>0){symbol="+";}xpPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(xpCount,1)).ToString();}

        if(scPopup!=null&&scCount!=0){string symbol="-";if(scCount>0){symbol="+";}scPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(scCount,1)).ToString();}

        if(coinPopup!=null&&coinCount!=0){string symbol="-";if(coinCount>0){symbol="+";}coinPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(coinCount,1)).ToString();}

        if(corePopup!=null&&coreCount!=0){string symbol="-";if(coreCount>0){symbol="+";}corePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=
        symbol+System.Math.Abs(System.Math.Round(coreCount,1)).ToString();}

    if(Time.timeScale>0.0001f){
        if(hpTimer>0){hpTimer-=Time.unscaledDeltaTime;}
        if(enTimer>0){enTimer-=Time.unscaledDeltaTime;}
        if(ammoTimer>0){ammoTimer-=Time.unscaledDeltaTime;}
        if(xpTimer>0){xpTimer-=Time.unscaledDeltaTime;}
        if(scTimer>0){scTimer-=Time.unscaledDeltaTime;}
        if(coinTimer>0){coinTimer-=Time.unscaledDeltaTime;}
        if(coreTimer>0){coreTimer-=Time.unscaledDeltaTime;}
    }
    }
    public static GameObject CreateOnUI(GameObject obj, Vector2 position){
        GameCanvas canvas=FindObjectOfType<GameCanvas>();
        GameObject childObject=Instantiate(obj,Camera.main.WorldToScreenPoint(position),Quaternion.identity,canvas.transform);
        //childObject.transform.parent = canvas.transform;
        childObject.transform.SetParent(canvas.transform);
        childObject.transform.position=Camera.main.WorldToScreenPoint(position);
        return childObject;
    }

    public void DMGPopup(float dmg, Vector2 pos, Color color, float scale=1, bool isPlayer=false){
        GameObject dmgpopup=GameCanvas.CreateOnUI(GameAssets.instance.GetVFX("DMGPopup"),pos);
        dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=color;
        dmgpopup.transform.localScale=new Vector2(scale,scale);
        if(FindObjectOfType<Player>()!=null&&isPlayer)dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/FindObjectOfType<Player>().armorMulti,2).ToString();
        else{dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,1).ToString();}
    }public GameObject DMGPopupReturn(float dmg, Vector2 pos, Color color, float scale=1, bool isPlayer=false){
        GameObject dmgpopup=GameCanvas.CreateOnUI(GameAssets.instance.GetVFX("DMGPopup"),pos);
        dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().color=color;
        dmgpopup.transform.localScale=new Vector2(scale,scale);
        if(FindObjectOfType<Player>()!=null&&isPlayer)dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg/FindObjectOfType<Player>().armorMulti,2).ToString();
        else{dmgpopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=System.Math.Round(dmg,1).ToString();}
        return dmgpopup;
    }
    public void HpPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.hpPopupsSum){HpDispCount(amnt);}else{HPPopUpHUD(amnt);}}
    void HpDispCount(float amnt){
        if(hpTimer<=0){hpCount=0;}
        hpTimer=popupSumTime;
        if(hpTimer>0){if((hpCount==0)||(amnt<0&&hpCount<0)||(amnt>0&&hpCount>0)){}else{hpCount=0;}hpCount+=amnt;if(hpCount!=0)HPPopUpHUD(hpCount);}
    }
    void HPPopUpHUD(float amnt){
        if(hpPopup!=null){
        hpPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //dmgpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        hpPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("HPPopUpHUD not present");}
    }

    public void EnPopupSwitch(float amnt){if(SaveSerial.instance.settingsData.enPopupsSum){EnDispCount(amnt);}else{EnPopUpHUD(amnt);}}
    void EnDispCount(float amnt){
        if(enTimer<=0){enCount=0;}
        enTimer=popupSumTime;
        if(enTimer>0){if((enCount==0)||(amnt<0&&enCount<0)||(amnt>0&&enCount>0)){}else{enCount=0;}enCount+=amnt;if(enCount!=0)EnPopUpHUD(enCount);}
    }
    /*IEnumerator enCor;
    public void EnDispCount(float amnt){if((enCount==0)||(amnt<0&&enCount<0)||(amnt>0&&enCount>0)){}else{enCount=0;}enCount+=amnt;EnPopUpHUD(enCount);
        if(!enCounted){enCor=EnDispCountI(amnt);StopCoroutine(enCor);StartCoroutine(enCor);}else{StopCoroutine(enCor);enCounted=false;}}
    IEnumerator EnDispCountI(float amnt){
        yield return new WaitForSeconds(popupSumTime);
        enCounted=true;
        enCount=0;
    }*/
    public void EnPopUpHUD(float amnt){
        if(enPopup!=null){
        enPopup.GetComponent<AnimationOn>().AnimationSet(true);
        //enpupupHud.GetComponent<Animator>().SetTrigger(0);
        string symbol="+";
        if(amnt<0)symbol="-";
        if(FindObjectOfType<Player>()!=null&&(!FindObjectOfType<Player>().infEnergy)||(FindObjectOfType<Player>().infEnergy&&symbol=="+")){
        enPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();}
        }else{Debug.LogWarning("EnergyPopUpHUD not present");}
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
        xpPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("XPPopUpHUD not present");}
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
        coinPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
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
        corePopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("CoresPopUpHUD not present");}
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
        if(FindObjectOfType<Player>()!=null&&(!FindObjectOfType<Player>().infEnergy)||(FindObjectOfType<Player>().infEnergy&&symbol=="+")){
        ammoPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        //energyUsedCount+=Mathf.Abs(amnt);
        }
        }else{Debug.LogWarning("AmmoPopUpHUD not present");}
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
        scPopup.GetComponentInChildren<TMPro.TextMeshProUGUI>().text=symbol+Mathf.Abs(amnt).ToString();
        }else{Debug.LogWarning("ScorePopUpHUD not present");}
    }
}
