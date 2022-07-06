using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XPBars : MonoBehaviour{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] public int ID;
    [SerializeField] public int IDmax;
    [SerializeField] string valueName;
    [SerializeField] bool shop;
    //int value;
    [SerializeField] public GameObject current;
    [SerializeField][Range(1,3)] public int created;
    Vector2 prevPos;
    private void OnValidate(){
        #if UNITY_EDITOR
        IDmax=prefabs.Length;
        if(created==1){
            prevPos=current.transform.localPosition;
            UnityEditor.EditorApplication.delayCall+=()=>{
                if(current!=null)DestroyImmediate(current);
                created=3;
            };
        }
        if(prefabs[ID-1]!=null&&ID>-1&&ID<=IDmax&&created==2){
            if(current==null)current=(GameObject)PrefabUtility.InstantiatePrefab(prefabs[ID-1],transform);current.transform.localPosition=prevPos;
            if(current!=null){
                var ch=current.transform.Find("Fill");
                foreach(Transform go in ch){
                    go.GetComponent<XPFill>().valueName=valueName;
                    go.GetComponent<XPFill>().shop=shop;
                }
            }
            created=3;
        }    
        #endif
    }

    void Update(){
        IDmax=prefabs.Length;
        if(created==1){
            prevPos=current.transform.localPosition;
            if(current!=null)DestroyImmediate(current);
            created=3;
        }
        if(prefabs[ID-1]!=null&&ID>-1&&ID<=IDmax&&created==2){
            if(current==null)current=(GameObject)Instantiate(prefabs[ID-1],transform);current.transform.localPosition=prevPos;
            if(current!=null){
                var ch=current.transform.Find("Fill");
                foreach(Transform go in ch){
                    go.GetComponent<XPFill>().valueName=valueName;
                    go.GetComponent<XPFill>().shop=shop;
                }
            }
            created=3;
        }
        if(current!=null&&!current.name.Contains(prefabs[ID-1].name))Recreate(ID);
    }
    public void Recreate(int id){ID=id;StartCoroutine(RecreateI());}
    IEnumerator RecreateI(){
        yield return new WaitForSecondsRealtime(0.005f);
        created=1;
        yield return new WaitForSecondsRealtime(0.005f);
        created=3;
        yield return new WaitForSecondsRealtime(0.005f);
        created=2;
        yield return new WaitForSecondsRealtime(0.005f);
        created=3;
    }
}
