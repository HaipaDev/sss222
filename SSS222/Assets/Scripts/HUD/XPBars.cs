using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class XPBars : MonoBehaviour{
    [SerializeField] GameObject[] prefabs;
    [SerializeField] public int ID;
    [SerializeField] string valueName;
    //int value;
    [SerializeField] public GameObject current;
    [SerializeField][Range(1,3)] public int created;
    //UpgradeMenu upgradeMenu;
    //#if UnityEditor
    void Start(){
        //upgradeMenu=FindObjectOfType<UpgradeMenu>();
    }
    //public override void OnInspectorGUI() {
    private void OnValidate(){
        #if UNITY_EDITOR
        if(created==1){
            UnityEditor.EditorApplication.delayCall += () =>
            {
                if(current!=null)DestroyImmediate( current );
                created=3;
                //StartCoroutine(Recreate());
            }; 
        }
        //if(current!=null)Destroy(current);
        if(prefabs[ID-1]!=null&&ID>-1&&ID<=prefabs.Length&&created==2){
            if(current==null)current=(GameObject)PrefabUtility.InstantiatePrefab(prefabs[ID-1],transform);//PrefabUtility.InstantiatePrefab
            //value=(int)upgradeMenu.GetType().GetField(valueName).GetValue(upgradeMenu);
            if(current!=null){
                var ch=current.transform.Find("Fill");
                foreach(Transform go in ch){
                    go.GetComponent<XPFill>().valueName=valueName;
                }
                //GetComponentInChildren<XPFill>().valueName=valueName;
            }
            created=3;
        }
        #else
        
        #endif
    }

    void Update(){
        if(created==1){
            if(current!=null)DestroyImmediate( current );
            created=3;
        }
        if(prefabs[ID-1]!=null&&ID>-1&&ID<=prefabs.Length&&created==2){
            if(current==null)current=(GameObject)Instantiate(prefabs[ID-1],transform);//PrefabUtility.InstantiatePrefab
            if(current!=null){
                var ch=current.transform.Find("Fill");
                foreach(Transform go in ch){
                    go.GetComponent<XPFill>().valueName=valueName;
                }
            }
            created=3;
        }
    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }
    IEnumerator Recreate(){
        yield return new WaitForSeconds(0.05f);
        created=2;
    }
     //#endif
}
