using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SimpleBlur : MonoBehaviour
{
    public Material background;

	void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
        Graphics.Blit(src, dst, background);
    }
}