using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExhaust : MonoBehaviour{
    [Sirenix.OdinInspector.SceneObjectsOnly][SerializeField]GameObject exhaustColliderObj;
    void Start(){
        //exhaustColliderObj.transform.localPosition=GetComponent<TrailVFX>().trailVFX.transform.localPosition;
    }
    void Update(){
        if(GameRules.instance.levelingOn&&UpgradeMenu.instance!=null){
                if(((UpgradeMenu.instance.total_UpgradesLvl<Player.instance.bflameDmgTillLvl||Player.instance.bflameDmgTillLvl==-4))
                //||(UpgradeMenu.instance.total_UpgradesLvl>=Player.instance.bflameDmgTillLvl&&Player.instance.bflameDmgTillLvl>0))
                &&(!exhaustColliderObj.activeSelf)){
                    exhaustColliderObj.SetActive(true);
                }else if(((UpgradeMenu.instance.total_UpgradesLvl>=Player.instance.bflameDmgTillLvl)||Player.instance.bflameDmgTillLvl==0)
                //||(UpgradeMenu.instance.total_UpgradesLvl<=Player.instance.bflameDmgTillLvl))
                &&(exhaustColliderObj.activeSelf)){
                    exhaustColliderObj.SetActive(false);
                }
        }
    }
    public void DestroyExhaust(){Destroy(exhaustColliderObj);Destroy(this);}
}
