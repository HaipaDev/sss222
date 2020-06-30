using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlTreeList : MonoBehaviour{
    [SerializeField] public int level;
    [SerializeField] public List<GameObject> elements;
    void Start(){
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Level: "+level.ToString();
    }

    void Update(){
        
    }
}
