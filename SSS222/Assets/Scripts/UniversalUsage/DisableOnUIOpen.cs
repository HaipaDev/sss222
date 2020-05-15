using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnUIOpen : MonoBehaviour{
    [SerializeField] GameObject obj;
    [SerializeField] Canvas canvas;
    void Start(){
        
    }
    void Update(){
        if(PauseMenu.GameIsPaused==true || Shop.shopOpened==true || UpgradeMenu.UpgradeMenuIsOpen==true){
            obj.SetActive(false);
            canvas.enabled=false;
        }else{
            obj.SetActive(true);
            canvas.enabled=true;
        }
    }
}
