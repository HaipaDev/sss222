using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifUI : MonoBehaviour{
    [SerializeField]notifUI_type type;
    void Update(){
        if(type==notifUI_type.shake){
            if(FindObjectOfType<LeechAttach>()!=null&&FindObjectOfType<LeechAttach>().attached==true){GetComponent<TMPro.TextMeshProUGUI>().enabled=true;}
            else{GetComponent<TMPro.TextMeshProUGUI>().enabled=false;}
        }else if(type==notifUI_type.dontShoot){
            if(FindObjectOfType<VortexWheel>()!=null&&FindObjectOfType<VortexWheel>().timer<FindObjectOfType<VortexWheel>().GetTimeToDie()){GetComponent<TMPro.TextMeshProUGUI>().enabled=true;}
            else{GetComponent<TMPro.TextMeshProUGUI>().enabled=false;}
        }else if(type==notifUI_type.lvlUp){
            if(FindObjectOfType<OnScreenButtons>()!=null&&FindObjectOfType<OnScreenButtons>().lvldUp==true){GetComponent<TMPro.TextMeshProUGUI>().enabled=true;}
            else{GetComponent<TMPro.TextMeshProUGUI>().enabled=false;}
        }else if(type==notifUI_type.noEnergy){
            if((GameRules.instance!=null&&Player.instance!=null)&&(GameRules.instance.energyOnPlayer&&Player.instance.energy<=0)){GetComponent<TMPro.TextMeshProUGUI>().enabled=true;}
            else{GetComponent<TMPro.TextMeshProUGUI>().enabled=false;}
        }
    }
}
enum notifUI_type{
    shake,dontShoot,lvlUp,noEnergy
}
