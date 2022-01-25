using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdventureZoneData : MonoBehaviour{
    [SerializeField] int id=1;
    void Start(){
        GetComponent<Button>().onClick.AddListener(LoadZone);
        GetComponentInChildren<TextMeshProUGUI>().text=Mathf.Abs(id).ToString();
    }
    void LoadZone(){
        GSceneManager.instance.LoadAdventureZone(-id);
    }
}
