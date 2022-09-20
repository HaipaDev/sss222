using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CosmeticPopups : MonoBehaviour{  public static CosmeticPopups instance;
    public List<_CstmzItemNameAndType> queue;
    [Header("Current achievement")]
    [SerializeField] CosmeticDrop cosmeticDropParent;

    
    [Header("Values")]
    public bool playing;
    public bool finished;
    public _CstmzItemNameAndType firstInQueue;
    
    void Awake(){
        if(CosmeticPopups.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);gameObject.name=gameObject.name.Split('(')[0];}
        if(transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(false);
    }
    void Update(){
        if(!playing&&!GetComponent<Animator>().GetBool("on")&&queue.Count>0&&GSceneManager.CheckScene("Menu")){playing=true;PopupCosmetic();}
        if(finished&&GetComponent<Animator>().GetBool("on")){playing=false;GetComponent<Animator>().SetBool("on",false);RemoveDoneFromQueue();if(transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(false);}
    }
    void PopupCosmetic(){
        finished=false;
        if(!transform.GetChild(0).gameObject.activeSelf)transform.GetChild(0).gameObject.SetActive(true);
        if(queue[0].type==CstmzType.skin){cosmeticDropParent.SetSkin(GameAssets.instance.GetSkin(queue[0].name));}
        if(queue[0].type==CstmzType.trail){cosmeticDropParent.SetTrail(GameAssets.instance.GetTrail(queue[0].name));}
        if(queue[0].type==CstmzType.flares){cosmeticDropParent.SetFlares(GameAssets.instance.GetFlares(queue[0].name));}
        if(queue[0].type==CstmzType.deathFx){cosmeticDropParent.SetDeathFx(GameAssets.instance.GetDeathFx(queue[0].name));}
        if(queue[0].type==CstmzType.music){cosmeticDropParent.SetMusic(GameAssets.instance.GetMusic(queue[0].name));}
        GetComponent<Animator>().SetBool("on",true);
    }
    public void AddToQueue(string n,CstmzType t){
        if(!queue.Exists(x=>x.name==n&&x.type==t)){queue.Add(new _CstmzItemNameAndType(){name=n,type=t});if(firstInQueue.name=="")firstInQueue=new _CstmzItemNameAndType(){name=n,type=t};}
    }
    void RemoveDoneFromQueue(){
        if(_removeFromQueueCor==null){_removeFromQueueCor=RemoveDoneFromQueueI();StartCoroutine(_removeFromQueueCor);}//else{Debug.Log("RemoveDoneFromQueue() called");}
    }
    IEnumerator _removeFromQueueCor;
    IEnumerator RemoveDoneFromQueueI(){
        //Debug.Log("RemoveDoneFromQueueI coroutine starting");
        if(queue.Count>0&&firstInQueue.name!=""){/*Debug.Log("Removing from queue: "+firstInQueue.name);*/queue.Remove(queue.Find(x=>x.name==firstInQueue.name&&x.type==firstInQueue.type));firstInQueue.name="";}
        if(queue.Count>0&&firstInQueue.name==""){finished=false;firstInQueue.name=queue[0].name;firstInQueue.type=queue[0].type;/*Debug.Log("Setting firstInQueue to: "+firstInQueue.name);*/yield return new WaitForSeconds(0.02f);_removeFromQueueCor=null;}else{finished=false;firstInQueue.name="";yield return new WaitForSeconds(0.03f);_removeFromQueueCor=null;}
    }
}
