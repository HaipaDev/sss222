using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlTree_ListElement : MonoBehaviour{
    [SerializeField] public int level;
    [SerializeField] public List<GameObject> elements;
    int maxHSlots=7;
    float yy;
    RectTransform rt;
    void Start(){
        rt=GetComponent<RectTransform>();

        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text="Level: "+level.ToString();
        var l=1;//20>13 l=1+1=2 // 40>13 l=3+1=4
        if(elements.Count>maxHSlots){l=(int)System.Math.Truncate((decimal)elements.Count/maxHSlots)+1;}
        yy=rt.sizeDelta.y;
        if(rt!=null)rt.sizeDelta=new Vector2(rt.sizeDelta.x, yy*l);
    }
    void Update(){
        if(rt==null)rt=GetComponent<RectTransform>();

        transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text="Level: "+level.ToString();
        var l=1;//20>13 l=1+1=2 // 40>13 l=3+1=4
        if(elements.Count>maxHSlots){l=(int)System.Math.Truncate((decimal)elements.Count/maxHSlots)+1;}
        if(rt!=null)rt.sizeDelta=new Vector2(rt.sizeDelta.x, yy*l);
    }
}
