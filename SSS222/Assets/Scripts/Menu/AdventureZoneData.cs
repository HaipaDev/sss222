using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdventureZoneData : MonoBehaviour{
    [SerializeField] int id=0;
    void Start(){
        GetComponent<Button>().onClick.AddListener(OnClickButtonEvent);
        GetComponentInChildren<TextMeshProUGUI>().text=(id+1).ToString();
    }
    void OnClickButtonEvent(){
        GSceneManager.instance.LoadAdventureZone(id);
    }
}
