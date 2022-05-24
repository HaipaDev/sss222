using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialsCanvas : MonoBehaviour{
    [SerializeField] GameObject loginButton;
    void Update(){
        loginButton.GetComponent<Animator>().SetBool("loggedIn",SaveSerial.instance.hyperGamerLoginData.loggedIn);
    }
    public void LeaderboardsButton(){GSceneManager.instance.LoadLeaderboardsScene();}
    public void AchievementsButton(){GSceneManager.instance.LoadAchievementsScene();}
    public void StatsButton(){GSceneManager.instance.LoadStatsSocialScene();}
}
