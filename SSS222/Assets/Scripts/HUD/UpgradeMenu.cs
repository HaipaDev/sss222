using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour{
    public static bool UpgradeMenuIsOpen = false;
    public GameObject upgradeMenuUI;
    public float prevGameSpeed = 1f;
    [HeaderAttribute("Upgrade Values")]
    public float maxHealth_upgradeAmnt=5f;
    public int maxHealth_upgradeCost=1;
    GameSession gameSession;
    Player player;
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
        player = FindObjectOfType<Player>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if (Input.GetKeyDown(KeyCode.E)){
            if(UpgradeMenuIsOpen){
                Resume();
            }else{
                if(Shop.shopOpened!=true && FindObjectOfType<Player>()!=null)Open();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && UpgradeMenuIsOpen)Resume();
    }

    public void Resume(){
        upgradeMenuUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = prevGameSpeed;
        UpgradeMenuIsOpen = false;
    }
    public void Open(){
        prevGameSpeed = gameSession.gameSpeed;
        upgradeMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        gameSession.gameSpeed = 0f;
        UpgradeMenuIsOpen = true;
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }

    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}

    public void UpgradeFloat(ref float value,float amnt,int cost){if(gameSession.cores>=cost){value+=amnt;gameSession.cores-=cost;}else{AudioSource.PlayClipAtPoint(gameSession.denySFX,transform.position);}}
    //if(gameSession.cores>=maxHealth_upgradeCost)player.maxHP+=maxHealth_upgradeAmnt;gameSession.cores-=maxHealth_upgradeCost;
    public void AddMaxHP(){UpgradeFloat(ref player.maxHP,maxHealth_upgradeAmnt,maxHealth_upgradeCost);}
    public void AddMaxEnergy(){UpgradeFloat(ref player.maxEnergy,maxHealth_upgradeAmnt,maxHealth_upgradeCost);}
}
