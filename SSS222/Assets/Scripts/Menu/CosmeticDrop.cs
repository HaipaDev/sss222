using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class CosmeticDrop : MonoBehaviour{
    [SceneObjectsOnly][SerializeField] Image rarityGlow;
    [SceneObjectsOnly][SerializeField] Image dropIcon;
    [SceneObjectsOnly][SerializeField] TextMeshProUGUI dropText;
    [SceneObjectsOnly][SerializeField] TextMeshProUGUI dropTypeText;
    void Start(){
        
    }
    void Update(){
        if(cosmeticAnimSpr!=null){dropIcon.sprite=cosmeticAnimSpr;}
    }
    public void PresetCosmeticDrop(){
        rarityGlow.color=Color.clear;
        dropIcon.color=Color.clear;
        dropText.text="";
        dropTypeText.text="";
        foreach(Transform tr in dropIcon.transform){Destroy(tr.gameObject);}
    }
    
    public void SetSkin(CstmzSkin s){
        PresetCosmeticDrop();
        dropTypeText.text="Skin";
        dropText.text=s.displayName;
        dropIcon.enabled=true;
        if(s.spr!=null){dropIcon.sprite=s.spr;}dropIcon.GetComponent<Image>().color=Color.white;
        if(s.animated){StartCoroutine(AnimateCosmeticDrop(s));}
        else{StopAnimatingCosmeticDrop();}
        rarityGlow.color=GameAssets.instance.GetRarityColor(s.rarity);
    }
    public void SetTrail(CstmzTrail t){
        PresetCosmeticDrop();
        dropTypeText.text="Trail";
        dropText.text=t.displayName;
        dropIcon.enabled=false;
        DropPreviewTrail(t);
        Color c=GameAssets.instance.GetRarityColor(t.rarity);
        rarityGlow.color=new Color(c.r,c.g,c.b,110f/255f);
    }
    public void SetFlares(CstmzFlares f){
        PresetCosmeticDrop();
        dropTypeText.text="Flares";
        dropText.text=f.displayName;
        dropIcon.enabled=false;
        DropPreviewFlares(f);
        Color c=GameAssets.instance.GetRarityColor(f.rarity);
        rarityGlow.color=new Color(c.r,c.g,c.b,110f/255f);
    }
    public void SetDeathFx(CstmzDeathFx d){
        PresetCosmeticDrop();
        dropTypeText.text="Death Fx";
        dropText.text=d.displayName;
        dropIcon.enabled=false;
        DropPreviewDeathFx(d);
        Color c=GameAssets.instance.GetRarityColor(d.rarity);
        rarityGlow.color=new Color(c.r,c.g,c.b,110f/255f);
    }
    public void SetMusic(CstmzMusic m){
        PresetCosmeticDrop();
        dropTypeText.text="Music Disk";
        dropText.text=m.displayName;
        dropIcon.enabled=true;
        dropIcon.sprite=m.icon;dropIcon.GetComponent<Image>().color=Color.white;
        rarityGlow.color=GameAssets.instance.GetRarityColor(m.rarity);
    }

    void DropPreviewTrail(CstmzTrail t){
        GameObject goPt=Instantiate(t.part,dropIcon.transform);goPt.transform.localScale=new Vector2(5,5);
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4);
    }
    void DropPreviewFlares(CstmzFlares f){
        GameObject goPt=Instantiate(GameAssets.instance.GetFlareRandom(f.name),dropIcon.transform);goPt.transform.localPosition=new Vector2(-44*4,0);goPt.transform.localScale=new Vector2(4,4);GameAssets.MakeParticleLooping(goPt.GetComponent<ParticleSystem>());
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4);
        goPt=Instantiate(GameAssets.instance.GetFlareRandom(f.name),dropIcon.transform);goPt.transform.localPosition=new Vector2(44*4,0);goPt.transform.localScale=new Vector2(4,4);GameAssets.MakeParticleLooping(goPt.GetComponent<ParticleSystem>());
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4);
    }
    void DropPreviewDeathFx(CstmzDeathFx d){
        GameObject goPt=Instantiate(d.obj,dropIcon.transform);goPt.transform.localScale=new Vector2(4,4);GameAssets.MakeParticleLooping(goPt.GetComponent<ParticleSystem>());
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4,true,0);
    }

    Coroutine cosmeticDropAnim;int iCosmeticDropAnim=0;Sprite cosmeticAnimSpr=null;
    IEnumerator AnimateCosmeticDrop(CstmzSkin skin){Sprite _spr;
        //Debug.Log("Animating skin: "+skin.name+" | Frame: "+iCosmeticDropAnim);
        if(skin.animSpeed>0){yield return new WaitForSeconds(skin.animSpeed);}
        else{yield return new WaitForSeconds(skin.animVals[iCosmeticDropAnim].delay);}
        _spr=skin.animVals[iCosmeticDropAnim].spr;
        if(iCosmeticDropAnim==skin.animVals.Count-1)iCosmeticDropAnim=0;
        if(iCosmeticDropAnim<skin.animVals.Count)iCosmeticDropAnim++;
        cosmeticAnimSpr=_spr;
        cosmeticDropAnim=StartCoroutine(AnimateCosmeticDrop(skin));
        //if(cosmeticDropAnim!=null)StopCoroutine(cosmeticDropAnim);cosmeticDropAnim=null;iCosmeticDropAnim=0;
    }
    public void StopAnimatingCosmeticDrop(){cosmeticAnimSpr=null;if(cosmeticDropAnim!=null)StopCoroutine(cosmeticDropAnim);cosmeticDropAnim=null;iCosmeticDropAnim=0;}
    public void Stop(){
        StopAnimatingCosmeticDrop();foreach(Transform tr in dropIcon.transform){Destroy(tr.gameObject);}
    }
}
