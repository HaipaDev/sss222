using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : MonoBehaviour{
    [SerializeField] GameObject socialsButton;
    void Update(){
        socialsButton.GetComponent<Animator>().SetBool("loggedIn",SaveSerial.instance.hyperGamerLoginData.loggedIn);
    }
    public void StartButton(){GSceneManager.instance.LoadGameModeChooseScene();}
    public void SocialsButton(){GSceneManager.instance.LoadSocialsScene();}
    public void OptionsButton(){GSceneManager.instance.LoadOptionsScene();}
    public void ExitButton(){GSceneManager.instance.QuitGame();}
}
