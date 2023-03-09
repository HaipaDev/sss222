using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class AdventureZonesCanvasMain : MonoBehaviour{
    [ChildGameObjectsOnly][SerializeField] GameObject zonesMapPanel;
    [ChildGameObjectsOnly][SerializeField] GameObject resetAdventurePanel;
    void Start(){zonesMapPanel.GetComponent<AdventureZonesCanvas>().UpdateTravelLine();}
    public void ResetAdventurePanel(){
        resetAdventurePanel.SetActive(true);
        zonesMapPanel.SetActive(false);
    }
    public void BackToZonesMapPanel(){
        zonesMapPanel.SetActive(true);
        resetAdventurePanel.SetActive(false);
        zonesMapPanel.GetComponent<AdventureZonesCanvas>().UpdateTravelLine();
    }
}
