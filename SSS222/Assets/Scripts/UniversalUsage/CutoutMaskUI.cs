using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image{
    public override Material material{
        get{
            Material material = new Material(base.material);
            material.SetInt("_StencilComp",(int)CompareFunction.NotEqual);
            return material;
        }
    }
}
