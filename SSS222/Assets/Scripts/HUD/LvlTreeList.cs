using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlTreeList : MonoBehaviour{
    [SerializeField] public int level;
    [SerializeField] public List<GameObject> elements;
    int maxHSlots=7;
    float yy;
    void Start(){
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Level: "+level.ToString();
        var l=1;//20>13 l=1+1=2 // 40>13 l=3+1=4
        if(elements.Count>maxHSlots){l=(int)System.Math.Truncate((decimal)elements.Count/maxHSlots)+1;}
        RectTransform rt = GetComponent<RectTransform>();
        yy=rt.sizeDelta.y;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, yy*l);
    }

    void Update(){
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Level: "+level.ToString();
        var l=1;//20>13 l=1+1=2 // 40>13 l=3+1=4
        if(elements.Count>maxHSlots){l=(int)System.Math.Truncate((decimal)elements.Count/maxHSlots)+1;}
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, yy*l);
    }
}
