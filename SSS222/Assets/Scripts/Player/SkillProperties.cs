﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Skill Properties")]
public class SkillProperties : ScriptableObject{
    [SerializeField] public string name = "";
    [TextArea][SerializeField] public string desc = "";
    [SerializeField] public TMPro.VertexGradient descGradient;
    [DisableIf("@this.iconsGo!=null")][SerializeField] public Sprite sprite;
    [DisableIf("@this.sprite!=null")][AssetsOnly][SerializeField] public GameObject iconsGo;
}