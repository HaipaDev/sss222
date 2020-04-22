using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSkinManager : MonoBehaviour{
    public int skinID = 0;
    public bool addOverlay = false;
    [SerializeField] GameObject overlayPrefab;
    [SerializeField] Sprite chameleonOverlay;
    [SerializeField] Color chameleonOvColor;
    [SerializeField] Sprite[] skins;
    //[SerializeField] int chameleonOvAlpha;
    GameObject overlayOBJ;
    SpriteRenderer overlay;
    SpriteRenderer spr;
    SaveSerial saveSerial;
    void Start(){
        spr=GetComponent<SpriteRenderer>();
        saveSerial = FindObjectOfType<SaveSerial>();
        LoadValues();
        if (skinID==0){ addOverlay = true; }
        if(addOverlay==true){
            overlayOBJ = Instantiate(overlayPrefab, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            overlayOBJ.transform.parent = gameObject.transform;
            overlay = overlayOBJ.GetComponent<SpriteRenderer>();
        }
    }

    void Update(){
        if (skinID == 0){
            overlay.sprite = chameleonOverlay;
            //chameleonOvColor.a = chameleonOvAlpha;
            overlay.color = chameleonOvColor;
        }
        else{
            spr.sprite=skins[skinID-1];
        }
    }
    void LoadValues(){
        chameleonOvColor = Color.HSVToRGB(saveSerial.chameleonColor[0], saveSerial.chameleonColor[1], saveSerial.chameleonColor[2]);
    }
}
