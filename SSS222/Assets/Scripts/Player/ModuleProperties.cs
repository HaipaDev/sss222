using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Module Properties")]
public class ModuleProperties : ScriptableObject{
    [SerializeField] public new string name = "";
    [SerializeField] public string displayName = "";
    [TextArea][SerializeField] public string desc = "";
    [SerializeField] public TMPro.VertexGradient descGradient;
    [DisableIf("@this.iconsGo!=null")][SerializeField] public Sprite sprite;
    [DisableIf("@this.sprite!=null")][AssetsOnly][SerializeField] public GameObject iconsGo;
}