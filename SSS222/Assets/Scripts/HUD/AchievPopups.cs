using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievPopups : MonoBehaviour{  public static AchievPopups instance;
    public List<string> queue;
    [Header("Current achievement")]
    [SerializeField] new TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Image icon;
    [SerializeField] bool epic;

    
    [Header("Values")]
    public bool playing;
    public bool finished;
    public string firstInQueueName;
    
    void Awake(){
        if(AchievPopups.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}
        if(transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(false);
    }
    void Update(){
        if(!playing&&queue.Count>0){playing=true;PopupAchiev();}
        if(finished){RemoveDoneFromQueue();GetComponent<Animator>().ResetTrigger("on");if(transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(false);}
        //else{if(queue.Count>0)firstInQueueName=GetCurrentAchiev();}
    }
    void PopupAchiev(){
        finished=false;
        if(!transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("on");
        name.text=GetCurrentAchiev().displayName;
        desc.text=GetCurrentAchiev().desc;
        icon.sprite=GetCurrentAchiev().icon;
        epic=GetCurrentAchiev().epic;
        if(epic){Color c=StatsAchievsManager.instance.epicCompletedColor;Vector4 _c=new Vector4(c.r,c.g,c.b,100f/255f);
        GetComponentInChildren<Image>().color=_c;}
        else{Color c=StatsAchievsManager.instance.completedColor;Vector4 _c=new Vector4(c.r,c.g,c.b,100f/255f);
        GetComponentInChildren<Image>().color=_c;}
        if(epic){AudioManager.instance.PlayOnce("AchievEpic");}
        else{AudioManager.instance.PlayOnce("Achiev");}
    }
    Achievement GetCurrentAchiev(){return StatsAchievsManager.instance.GetAchievByName(queue[0]);}
    public void AddToQueue(string a){
        if(!queue.Contains(a))queue.Add(a);
    }
    public void RemoveDoneFromQueue(){
        if(queue.Count>0){queue.RemoveAt(0);}finished=false;return;
    }
}
