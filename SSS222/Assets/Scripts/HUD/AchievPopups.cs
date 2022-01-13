using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievPopups : MonoBehaviour{
    public static AchievPopups instance;
    public List<Achievement> queue;
    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Image icon;
    public bool playing;
    public bool finished;
    void Awake(){if(AchievPopups.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Update(){
        if(!playing&&queue.Count>0){PopupAchiev();playing=true;}
        if(finished){RemoveDoneFromQueue();GetComponent<Animator>().ResetTrigger("Play");finished=false;}
    }
    void PopupAchiev(){
        finished=false;
        GetComponent<Animator>().SetTrigger("Play");
        name.text=queue[0].name;
        desc.text=queue[0].desc;
        icon.sprite=queue[0].icon;
    }
    public void AddToQueue(Achievement a){
        if(!queue.Contains(a))queue.Add(a);
    }
    public void RemoveDoneFromQueue(){
        if(queue.Count>0){queue.RemoveAt(0);}return;
    }
}
