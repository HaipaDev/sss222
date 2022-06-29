using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Module Properties")]
public class ModuleProperties : ScriptableObject{
    [SerializeField] public string name = "";
    [TextArea][SerializeField] public string desc = "";
    [SerializeField] public Color descGradient1 = Color.white;
    [SerializeField] public Color descGradient2 = Color.white;
    [DisableIf("@this.iconsGo!=null")][SerializeField] public Sprite sprite;
    [DisableIf("@this.sprite!=null")][AssetsOnly][SerializeField] public GameObject iconsGo;
}