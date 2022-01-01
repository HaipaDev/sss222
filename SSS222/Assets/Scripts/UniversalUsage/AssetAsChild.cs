using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetAsChild : MonoBehaviour{
    [SerializeField] string assetName;
    [SerializeField] Vector2 offset;
    void Start(){
        GameObject go=Instantiate(GameAssets.instance.Get(assetName),transform);
        go.transform.localPosition=offset;
    }
}
