using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievPopups : MonoBehaviour{  public static AchievPopups instance;
    public List<Achievement> queue;
    [Header("Current achievement")]
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Image icon;
    [SerializeField] bool epic;

    
    [Header("Values")]
    public bool playing;
    public bool finished;
    public string firstInQueueName;
    
    void Awake(){
        if(AchievPopups.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        if(transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(false);
    }
    void Update(){
        if(!playing&&queue.Count>0){playing=true;PopupAchiev();}
        if(finished){RemoveDoneFromQueue();GetComponent<Animator>().ResetTrigger("Play");if(transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(false);}
        else{if(queue.Count>0)firstInQueueName=queue[0].displayName;}
    }
    void PopupAchiev(){
        finished=false;
        if(!transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("Play");
        name.text=queue[0].displayName;
        desc.text=queue[0].desc;
        icon.sprite=queue[0].icon;
        epic=queue[0].epic;
        if(epic){Color c=StatsAchievsManager.instance.epicCompletedColor;Vector4 _c=new Vector4(c.r,c.g,c.b,100f/255f);
        GetComponentInChildren<Image>().color=_c;}
        else{Color c=StatsAchievsManager.instance.completedColor;Vector4 _c=new Vector4(c.r,c.g,c.b,100f/255f);
        GetComponentInChildren<Image>().color=_c;}
        if(epic){AudioManager.instance.PlayOnce("AchievEpic");}
        else{AudioManager.instance.PlayOnce("Achiev");}
    }
    public void AddToQueue(Achievement a){
        if(!queue.Contains(a))queue.Add(a);
    }
    public void RemoveDoneFromQueue(){
        if(queue.Count>0){if(queue[0].displayName==firstInQueueName)queue.RemoveAt(0);}finished=false;return;
    }
}
