using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour{
    [SerializeField] UnityEngine.UI.Slider Hslider;
    [SerializeField] UnityEngine.UI.Slider Sslider;
    [SerializeField] UnityEngine.UI.Slider Vslider;
    [SerializeField] UnityEngine.UI.Image SsliderIMG;
    [SerializeField] UnityEngine.UI.Image VsliderIMG;
    [SerializeField] UnityEngine.UI.Image chameleonOverlay;
    public Color chameleonColor;
    public float[] chameleonColorArr = new float[3];

    SaveSerial saveSerial;
    public void Start()
    {
        saveSerial = FindObjectOfType<SaveSerial>();
        chameleonColorArr[0] = saveSerial.chameleonColor[0];
        chameleonColorArr[1] = saveSerial.chameleonColor[1];
        chameleonColorArr[2] = saveSerial.chameleonColor[2];
        chameleonColor = Color.HSVToRGB(chameleonColorArr[0], chameleonColorArr[1], chameleonColorArr[2]);
        chameleonOverlay.color = chameleonColor;
        //float H;float S=1; float V=1;
        //Color.RGBToHSV(chameleonColor,out H,out S,out V);
        Hslider.value = chameleonColorArr[0];
        Sslider.value = chameleonColorArr[1];
        Vslider.value = chameleonColorArr[2];
        Hslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Sslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        Vslider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheck()
    {
        chameleonColor = Color.HSVToRGB(Hslider.value, Sslider.value, Vslider.value);
        chameleonOverlay.color = chameleonColor;
        SsliderIMG.color = Color.HSVToRGB(Hslider.value,1,1);
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
        Color.RGBToHSV(chameleonColor, out chameleonColorArr[0], out chameleonColorArr[1], out chameleonColorArr[2]);
    }
}
