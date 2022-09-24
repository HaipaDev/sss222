using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour{
    [SerializeField] GameObject socialsButton;
    [SerializeField] GameObject shipUI;
    void Update(){
        socialsButton.GetComponent<Animator>().SetBool("loggedIn",SaveSerial.instance.hyperGamerLoginData.loggedIn);
    }
    void LasersAnim(){
        shipUI.GetComponent<Animator>().enabled=true;
        shipUI.GetComponent<ShipUI>().MakeFlares();
    }
    void ExplosionShipAnim(){
        var dfx=ShipCustomizationManager.instance.GetDeathFx();
        var expl=Instantiate(dfx.obj,shipUI.transform);
        AssetsManager.instance.TransformIntoUIParticle(expl,0,-1,true);
        AudioManager.instance.Play(dfx.sound);
        shipUI.GetComponent<TrailVFX>().ClearTrail();
        foreach(MonoBehaviour c in shipUI.GetComponents<MonoBehaviour>())c.enabled=false;
    }
    void ExplosionScreenAnim(){
        var expl=Instantiate(AssetsManager.instance.GetVFX("Explosion"),transform);
        AudioManager.instance.Play("Explosion");
        AssetsManager.instance.TransformIntoUIParticle(expl,1000,-1,true);
    }
    const float laserAnimLength=0.14f;
    const float explosionShipAnimLength=0.2f;
    const float explosionScreenAnimLength=0.28f;
    public void StartButton(){LasersAnim();StartCoroutine(StartButtonI());}
    public IEnumerator StartButtonI(){yield return new WaitForSecondsRealtime(laserAnimLength);GSceneManager.instance.LoadGameModeChooseScene();}
    public void SocialsButton(){LasersAnim();StartCoroutine(SocialsButtonI());}
    public IEnumerator SocialsButtonI(){yield return new WaitForSecondsRealtime(laserAnimLength);GSceneManager.instance.LoadSocialsScene();}
    public void OptionsButton(){LasersAnim();StartCoroutine(OptionsButtonI());}
    public IEnumerator OptionsButtonI(){yield return new WaitForSecondsRealtime(laserAnimLength);GSceneManager.instance.LoadOptionsScene();}
    public void CustomizationButton(){ExplosionShipAnim();StartCoroutine(CustomizationButtonI());}
    public IEnumerator CustomizationButtonI(){yield return new WaitForSecondsRealtime(explosionShipAnimLength);GSceneManager.instance.LoadCustomizationScene();}
    public void ExitButton(){ExplosionScreenAnim();StartCoroutine(ExitButtonI());}
    public IEnumerator ExitButtonI(){yield return new WaitForSecondsRealtime(explosionScreenAnimLength);GSceneManager.instance.QuitGame();}
}
