using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTitleDisplay : MonoBehaviour{
    public void TurnOnBossDisplay(){
        Debug.Log("TurnOnBossDisplay");
        transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite=GameRules.instance.bossInfo.bossTitleSprite;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
