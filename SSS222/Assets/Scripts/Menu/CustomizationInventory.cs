using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class CustomizationInventory : MonoBehaviour{    public static CustomizationInventory instance;
    [HeaderAttribute("Panels")]
    [SceneObjectsOnly][SerializeField] GameObject customizationPanel;
    [SceneObjectsOnly][SerializeField] GameObject lockboxesPanel;
    [SceneObjectsOnly][SerializeField] GameObject lockboxOpeningPanel;
    [HeaderAttribute("Customization Objects")]
    [SceneObjectsOnly][SerializeField] CstmzCategoryDropdown categoriesDropdown;
    [SceneObjectsOnly][SerializeField] RectTransform typesListContent;
    [AssetsOnly][SerializeField] GameObject cstmzElementPrefab;
    [SceneObjectsOnly][SerializeField] RectTransform elementsListContent;
    [SceneObjectsOnly][SerializeField] GameObject variantsPanel;
    [SceneObjectsOnly][SerializeField] RectTransform variantsListContent;
    [SceneObjectsOnly][SerializeField] GameObject colorSliders;
    [SceneObjectsOnly][SerializeField] Slider Hslider;
    [SceneObjectsOnly][SerializeField] Slider Sslider;
    [SceneObjectsOnly][SerializeField] Slider Vslider;
    [SceneObjectsOnly][SerializeField] Image SsliderIMG;
    [SceneObjectsOnly][SerializeField] Image VsliderIMG;
    [AssetsOnly][SerializeField] Material gradientShader;
    [HeaderAttribute("Lockboxes Objects")]
    [AssetsOnly][SerializeField] GameObject lockboxElementPrefab;
    [SceneObjectsOnly][SerializeField] RectTransform lockboxElementListContent;
    [SceneObjectsOnly][SerializeField] TextMeshProUGUI starCraft_costText;
    [SceneObjectsOnly][SerializeField] TextMeshProUGUI starCraft_chanceText;
    [HeaderAttribute("LockboxOpening Objects")]
    [SceneObjectsOnly][SerializeField] Image lockboxIcon;
    [SceneObjectsOnly][SerializeField] Image rarityGlow;
    [SceneObjectsOnly][SerializeField] Image dropIcon;
    [SceneObjectsOnly][SerializeField] TextMeshProUGUI dropText;
    [SceneObjectsOnly][SerializeField] TextMeshProUGUI dropTypeText;
    [HeaderAttribute("Rarity Colors")]    
    public Color defColor=Color.grey;
    public Color commonColor=Color.green;
    public Color rareColor=Color.blue;
    public Color epicColor=Color.magenta;
    public Color legendColor=Color.yellow;
    [HeaderAttribute("Properties")]
    [SerializeField] public CstmzCategory categorySelected=CstmzCategory.twoPiece;
    [SerializeField] public CstmzType typeSelected=CstmzType.skin;
    [SerializeField] public string skinName="def";
    public Color overlayColor=Color.red;
    [SerializeField] public string trailName="def";
    [SerializeField] public string flaresName="def";
    [SerializeField] public string deathFxName="def";
    [SerializeField] public string musicName=CstmzMusic._cstmzMusicDef;
    [SerializeField] public float openingTime=4;
    [SerializeField] public int dynamCelestStar_shardCost=10;
    [SerializeField] public float dynamCelestStar_craftChance=70;
    [DisableInEditorMode] public float openingTimer=-4;
    [DisableInEditorMode] public bool lockboxesPanelOpen;
    bool loaded;
    void Awake(){instance=this;}
    IEnumerator Start(){
        if(String.IsNullOrEmpty(skinName)||GetSkin(skinName)==null||!_isSkinUnlocked(skinName)){skinName="def";}
        if(String.IsNullOrEmpty(trailName)||GetTrail(trailName)==null||!_isTrailUnlocked(trailName)){trailName="def";}
        if(String.IsNullOrEmpty(flaresName)||GetFlares(flaresName)==null||!_isFlaresUnlocked(flaresName)){flaresName="def";}
        if(String.IsNullOrEmpty(deathFxName)||GetDeathFx(deathFxName)==null||!_isDeathFxUnlocked(deathFxName)){deathFxName="def";}
        if(String.IsNullOrEmpty(musicName)||GetMusic(musicName)==null||!_isMusicUnlocked(musicName)){musicName=CstmzMusic._cstmzMusicDef;}
        skinName=SaveSerial.instance.playerData.skinName;
        SetCategory(GameAssets.instance.skins.Find(x=>x.name.Contains(GetSkinName(SaveSerial.instance.playerData.skinName))).category);

        foreach(CstmzSkin s in GameAssets.instance.skins){if(s.name!="def"&&s.rarity==CstmzRarity.def)UnlockSkin(s.name);}
        foreach(CstmzSkin t in GameAssets.instance.skins){if(t.name!="def"&&t.rarity==CstmzRarity.def)UnlockTrail(t.name);}
        foreach(CstmzSkin f in GameAssets.instance.skins){if(f.name!="def"&&f.rarity==CstmzRarity.def)UnlockFlares(f.name);}
        foreach(CstmzSkin d in GameAssets.instance.skins){if(d.name!="def"&&d.rarity==CstmzRarity.def)UnlockDeathFx(d.name);}
        foreach(CstmzSkin m in GameAssets.instance.skins){if(m.name!=CstmzMusic._cstmzMusicDef&&m.rarity==CstmzRarity.def)UnlockMusic(m.name);}

        float[] hsvArr=new float[3]{0,1,1};
        overlayColor = SaveSerial.instance.playerData.overlayColor;
        Color.RGBToHSV(overlayColor,out hsvArr[0],out hsvArr[1],out hsvArr[2]);
        Hslider.value = hsvArr[0];
        Sslider.value = hsvArr[1];
        Vslider.value = hsvArr[2];

        SsliderIMG.material = Instantiate(gradientShader) as Material;
        VsliderIMG.material = SsliderIMG.material;
        SsliderIMG.material.SetColor("_Color2", Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);

        trailName=SaveSerial.instance.playerData.trailName;
        flaresName=SaveSerial.instance.playerData.flaresName;
        deathFxName=SaveSerial.instance.playerData.deathFxName;
        musicName=SaveSerial.instance.playerData.musicName;

        loaded=true;
        SetType(typeSelected);
        yield return new WaitForSecondsRealtime(0.02f);
        RecreateAllLockboxElements();
        yield return new WaitForSecondsRealtime(0.02f);
        OpenCustomizationPanel();
        HighlightSelectedType();

        foreach(CstmzLockbox lb in GameAssets.instance.lockboxes){if(!SaveSerial.instance.playerData.lockboxesInventory.Exists(x=>x.name==lb.name)){SaveSerial.instance.playerData.lockboxesInventory.Add(new LockboxCount{name=lb.name,count=0});}}
    }
    //void OnDestroy(){DestroyImmediate(SsliderIMG.material);}
    //void OnDisable(){DestroyImmediate(SsliderIMG.material);}
    public void ValueChangeCheck(){
        overlayColor=Color.HSVToRGB(Hslider.value,Sslider.value,Vslider.value);

        SsliderIMG.material.SetColor("_Color2",Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB(Hslider.value, 1, 1);
    }
    void Update(){
        if(loaded){
            SaveSerial.instance.playerData.skinName=skinName;
            SaveSerial.instance.playerData.overlayColor=overlayColor;
            SaveSerial.instance.playerData.trailName=trailName;
            SaveSerial.instance.playerData.flaresName=flaresName;
            SaveSerial.instance.playerData.deathFxName=deathFxName;
            SaveSerial.instance.playerData.musicName=musicName;
        }

        if(ShipCustomizationManager.instance!=null){
            ShipCustomizationManager.instance.skinName=skinName;
            ShipCustomizationManager.instance.overlayColor=overlayColor;
            ShipCustomizationManager.instance.trailName=trailName;
            ShipCustomizationManager.instance.flaresName=flaresName;
            ShipCustomizationManager.instance.deathFxName=deathFxName;
        }

        if(openingTimer>0){openingTimer-=Time.unscaledDeltaTime;}
        else if(openingTimer<=0&&openingTimer!=-4){openingTimer=-4;}

        if(skinName!="def"&&trailName!="def"&&flaresName!="def"){StatsAchievsManager.instance.CustomizedAll();}
        RefreshParticles();
        if(GSceneManager.EscPressed()){Back();}
        if(lockboxesPanel.activeSelf||lockboxOpeningPanel.activeSelf){lockboxesPanelOpen=true;}else{lockboxesPanelOpen=false;}
        if(lockboxAnimSpr!=null){dropIcon.sprite=lockboxAnimSpr;}
        if(starCraft_costText!=null){starCraft_costText.text="x"+dynamCelestStar_shardCost.ToString();}
        if(starCraft_chanceText!=null){starCraft_chanceText.text=Math.Round(dynamCelestStar_craftChance,2).ToString()+"%";}
    }
    public void Back(){
        if(variantsPanel.activeSelf){CloseVariants();}
        else{if(!lockboxOpeningPanel.activeSelf){DBAccess.instance.UpdateCustomizationData();GSceneManager.instance.LoadStartMenu();}else{QuitOpeningLockbox();}}
    }
    bool _itemDropped;
    public void OpenCustomizationPanel(){CloseAllPanels();customizationPanel.SetActive(true);RecreateAllElements();}
    public void OpenLockboxesPanel(){CloseAllPanels();lockboxesPanel.SetActive(true);RecreateAllLockboxElements();}
    public void OpenLockboxOpeningPanel(string name){CloseAllPanels();lockboxOpeningPanel.SetActive(true);OpenLockbox(name);}
    public void CloseAllPanels(){customizationPanel.SetActive(false);lockboxesPanel.SetActive(false);lockboxOpeningPanel.SetActive(false);}
    public void OpenLockbox(string name){StartCoroutine(OpenLockboxI(name));}
    IEnumerator OpenLockboxI(string name){
        var lb=GameAssets.instance.lockboxes.Find(x=>x.name==name);
        AudioManager.instance.Play("LockboxOpen");
        lockboxIcon.sprite=lb.icon;
        rarityGlow.color=Color.clear;
        dropIcon.color=Color.clear;
        dropText.text="";
        dropTypeText.text="";
        foreach(Transform tr in dropIcon.transform){Destroy(tr.gameObject);}
        yield return new WaitForSeconds(0.3f);
        lockboxIcon.sprite=lb.iconOpen;
        rarityGlow.color=defColor;
        //dropIcon.color=Color.white;
        SetDroppedItem(name);
        _itemDropped=true;
    }
    public void SetDroppedItem(string lockboxName){
        var lb=GameAssets.instance.lockboxes.Find(x=>x.name==lockboxName);
        LootTableCstmz lt=gameObject.AddComponent<LootTableCstmz>();
        lt.itemList=new List<LootTableEntryCstmz>();
        foreach(CstmzSkin s in GameAssets.instance.skins){if(s.category==lb.category&&s.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=s.name,cstmzType=CstmzType.skin,dropChance=lb.skinDrops.Find(x=>x.rarity==s.rarity).chance});}}
        foreach(CstmzTrail t in GameAssets.instance.trails){if(t.category==lb.category&&t.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=t.name,cstmzType=CstmzType.trail,dropChance=lb.trailDrops.Find(x=>x.rarity==t.rarity).chance});}}
        foreach(CstmzFlares f in GameAssets.instance.flares){if(f.category==lb.category&&f.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=f.name,cstmzType=CstmzType.flares,dropChance=lb.flareDrops.Find(x=>x.rarity==f.rarity).chance});}}
        foreach(CstmzDeathFx d in GameAssets.instance.deathFxs){if(d.category==lb.category&&d.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=d.name,cstmzType=CstmzType.deathFx,dropChance=lb.deathFxDrops.Find(x=>x.rarity==d.rarity).chance});}}
        foreach(CstmzMusic m in GameAssets.instance.musics){if(m.category==lb.category&&m.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=m.name,cstmzType=CstmzType.music,dropChance=lb.musicDrops.Find(x=>x.rarity==m.rarity).chance});}}
        lt.SumUp();

        KeyValuePair<string,CstmzType> i=new KeyValuePair<string,CstmzType>("",0);
        while(i.Key==""){
            LootTableEntryCstmz r=lt.GetItem();
            Debug.Log(r.lootItem+" | "+r.cstmzType+" | "+r.dropChance);
            if(r.cstmzType==CstmzType.skin&&(!_isSkinUnlocked(r.lootItem)||_allCategorySkinsUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzSkin s=GetSkin(i.Key);
                dropText.text=s.displayName;
                dropIcon.enabled=true;
                if(s.spr!=null){dropIcon.sprite=s.spr;}dropIcon.GetComponent<Image>().color=Color.white;
                if(s.animated){StartCoroutine(AnimateLockboxDrop(s));}
                else{StopAnimatingLockboxDrop();}
                rarityGlow.color=GetRarityColor(s.rarity);
                PlayRaritySound(s.rarity);
                UnlockSkin(r.lootItem);
                if(_allCategorySkinsUnlocked(lb.category)&&!_literallyEverythingInCategoryUnlocked(lb.category)&&!(_allCategoryTrailsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){i=new KeyValuePair<string,CstmzType>("",0);Debug.Log("All Skins unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.trail&&(!_isTrailUnlocked(r.lootItem)||_allCategoryTrailsUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzTrail t=GetTrail(i.Key);
                dropText.text=t.displayName;
                dropIcon.enabled=false;
                LockboxDropPreviewTrail(t);
                rarityGlow.color=new Color(GetRarityColor(t.rarity).r,GetRarityColor(t.rarity).g,GetRarityColor(t.rarity).b,110f/255f);
                PlayRaritySound(t.rarity);
                UnlockTrail(r.lootItem);
                if(_allCategoryTrailsUnlocked(lb.category)&&!_literallyEverythingInCategoryUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){i=new KeyValuePair<string,CstmzType>("",0);Debug.Log("All Trails unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.flares&&(!_isFlaresUnlocked(r.lootItem)||_allCategoryFlaresUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzFlares f=GetFlares(i.Key);
                dropText.text=f.displayName;
                dropIcon.enabled=false;
                LockboxDropPreviewFlares(f);
                rarityGlow.color=new Color(GetRarityColor(f.rarity).r,GetRarityColor(f.rarity).g,GetRarityColor(f.rarity).b,110f/255f);
                PlayRaritySound(f.rarity);
                UnlockFlares(r.lootItem);
                if(_allCategoryFlaresUnlocked(lb.category)&&!_literallyEverythingInCategoryUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryTrailsUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){i=new KeyValuePair<string,CstmzType>("",0);Debug.Log("All Flares unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.deathFx&&(!_isDeathFxUnlocked(r.lootItem)||_allCategoryDeathFxUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzDeathFx d=GetDeathFx(i.Key);
                dropText.text=d.displayName;
                dropIcon.enabled=false;
                LockboxDropPreviewDeathFx(d);
                rarityGlow.color=new Color(GetRarityColor(d.rarity).r,GetRarityColor(d.rarity).g,GetRarityColor(d.rarity).b,110f/255f);
                PlayRaritySound(d.rarity);
                UnlockDeathFx(r.lootItem);
                if(_allCategoryDeathFxUnlocked(lb.category)&&!_literallyEverythingInCategoryUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryTrailsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){i=new KeyValuePair<string,CstmzType>("",0);Debug.Log("All DeathFx unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.music&&(!_isMusicUnlocked(r.lootItem)||_allCategoryMusicUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzMusic m=GetMusic(i.Key);
                dropText.text=m.displayName;
                dropIcon.enabled=true;
                dropIcon.sprite=m.icon;dropIcon.GetComponent<Image>().color=Color.white;
                rarityGlow.color=GetRarityColor(m.rarity);
                PlayRaritySound(m.rarity);
                UnlockMusic(r.lootItem);
                if(_allCategoryMusicUnlocked(lb.category)&&!_literallyEverythingInCategoryUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryTrailsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category))){i=new KeyValuePair<string,CstmzType>("",0);Debug.Log("All Music unlocked, trying to find other items");}
            }
            dropTypeText.text=r.cstmzType.ToString();
            if(r.cstmzType!=CstmzType.skin){StopAnimatingLockboxDrop();}
        }
        Destroy(lt);
    }
    void LockboxDropPreviewTrail(CstmzTrail t){
        GameObject goPt=Instantiate(t.part,dropIcon.transform);goPt.transform.localScale=new Vector2(5,5);
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4);
    }
    void LockboxDropPreviewFlares(CstmzFlares f){
        GameObject goPt=Instantiate(GetFlareVFX(f.name),dropIcon.transform);goPt.transform.localPosition=new Vector2(-44*4,0);goPt.transform.localScale=new Vector2(4,4);GameAssets.MakeParticleLooping(goPt.GetComponent<ParticleSystem>());
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4);
        goPt=Instantiate(GetFlareVFX(f.name),dropIcon.transform);goPt.transform.localPosition=new Vector2(44*4,0);goPt.transform.localScale=new Vector2(4,4);GameAssets.MakeParticleLooping(goPt.GetComponent<ParticleSystem>());
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4);
    }
    void LockboxDropPreviewDeathFx(CstmzDeathFx d){
        GameObject goPt=Instantiate(d.obj,dropIcon.transform);goPt.transform.localScale=new Vector2(4,4);GameAssets.MakeParticleLooping(goPt.GetComponent<ParticleSystem>());
        GameAssets.instance.TransformIntoUIParticle(goPt,0,-4,true,0);
    }
    Coroutine lockboxDropAnim;int iLockboxDropAnim=0;Sprite lockboxAnimSpr=null;
    IEnumerator AnimateLockboxDrop(CstmzSkin skin){Sprite _spr;
        Debug.Log("Animating skin: "+skin.name+" | Frame: "+iLockboxDropAnim);
        if(skin.animSpeed>0){yield return new WaitForSeconds(skin.animSpeed);}
        else{yield return new WaitForSeconds(skin.animVals[iLockboxDropAnim].delay);}
        _spr=skin.animVals[iLockboxDropAnim].spr;
        if(iLockboxDropAnim==skin.animVals.Count-1)iLockboxDropAnim=0;
        if(iLockboxDropAnim<skin.animVals.Count)iLockboxDropAnim++;
        lockboxAnimSpr=_spr;
        lockboxDropAnim=StartCoroutine(AnimateLockboxDrop(skin));
        //if(lockboxDropAnim!=null)StopCoroutine(lockboxDropAnim);lockboxDropAnim=null;iLockboxDropAnim=0;
    }
    void StopAnimatingLockboxDrop(){lockboxAnimSpr=null;if(lockboxDropAnim!=null)StopCoroutine(lockboxDropAnim);lockboxDropAnim=null;iLockboxDropAnim=0;}
    public void QuitOpeningLockbox(){if(_itemDropped){OpenLockboxesPanel();_itemDropped=false;StopAnimatingLockboxDrop();foreach(Transform tr in dropIcon.transform){Destroy(tr.gameObject);}}}
    public void CraftCelestStar(){
        if(SaveSerial.instance.playerData.starshards>=dynamCelestStar_shardCost){
            if(GameAssets.CheckChance(dynamCelestStar_craftChance)){
                SaveSerial.instance.playerData.dynamCelestStars++;
                AudioManager.instance.Play("StarCraft");
            }else{AudioManager.instance.Play("StarCraft-Fail");}
            SaveSerial.instance.playerData.starshards-=dynamCelestStar_shardCost;
        }else{AudioManager.instance.Play("Deny");}
    }

    public void RecreateAllElements(){DeleteAllElements();CreateAllElements();HighlightSelectedElement();}
    //public void RecreateAllElements(){StartCoroutine(RecreateAllElementsI());}
    public IEnumerator RecreateAllElementsI(){DeleteAllElements();
        yield return new WaitForSeconds(0.01f);CreateAllElements();
        yield return new WaitForSeconds(0.01f);HighlightSelectedElement();}
    void DeleteAllElements(){foreach(Transform t in elementsListContent){Destroy(t.gameObject);}}
    void CreateAllElements(){
        if(typeSelected==CstmzType.skin){
            var currentCategorySkins=GameAssets.instance.skins.FindAll(x=>x.category==categorySelected);
            foreach(CstmzSkin ge in currentCategorySkins){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="SkinElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                ce.elementPv.GetComponent<Image>().sprite=ge.spr;
                ce.overlayImg.GetComponent<Image>().sprite=ge.sprOverlay;
            }
        }else if(typeSelected==CstmzType.trail){
            var currentCategoryTrails=GameAssets.instance.trails.FindAll(x=>x.category==categorySelected);
            foreach(CstmzTrail ge in currentCategoryTrails){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="TrailElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                Destroy(ce.overlayImg);
                ce.elementPv.GetComponent<Image>().sprite=null;ce.elementPv.GetComponent<Image>().color=new Color(1,1,1,1f/255f);
                GameObject goPt=Instantiate(ge.part,ce.elementPv.transform);
                GameAssets.instance.TransformIntoUIParticle(goPt);
            }
        }else if(typeSelected==CstmzType.flares){
            var currentCategoryFlares=GameAssets.instance.flares.FindAll(x=>x.category==categorySelected);
            foreach(CstmzFlares ge in currentCategoryFlares){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="FlaresElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                Destroy(ce.overlayImg);
                ce.elementPv.GetComponent<Image>().sprite=null;ce.elementPv.GetComponent<Image>().color=new Color(1,1,1,1f/255f);
                foreach(Transform t in ce.elementPv.transform){Destroy(t.gameObject);}
                GameObject goPt=Instantiate(GetFlareVFX(ge.name),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
                GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
                goPt=Instantiate(GetFlareVFX(ge.name),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
                GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
            }
        }else if(typeSelected==CstmzType.deathFx){
            var currentCategoryDeathFxs=GameAssets.instance.deathFxs.FindAll(x=>x.category==categorySelected);
            foreach(CstmzDeathFx ge in currentCategoryDeathFxs){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="DeathFxElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                Destroy(ce.overlayImg);
                ce.elementPv.GetComponent<Image>().sprite=null;ce.elementPv.GetComponent<Image>().color=new Color(1,1,1,1f/255f);
                GameObject goPt=Instantiate(ge.obj,go.transform);
                GameAssets.instance.TransformIntoUIParticle(goPt,0,-1,true);
            }
        }else if(typeSelected==CstmzType.music){
            var currentCategoryMusic=GameAssets.instance.musics.FindAll(x=>x.category==categorySelected);
            foreach(CstmzMusic ge in currentCategoryMusic){
                var go=Instantiate(cstmzElementPrefab,elementsListContent);
                go.name="MusicElement_"+ge.name;
                CstmzElement ce=go.GetComponent<CstmzElement>();
                ce.elementType=typeSelected;
                ce.elementName=ge.name;
                ce.rarity=ge.rarity;
                ce.elementPv.GetComponent<Image>().sprite=ge.icon;
                Destroy(ce.overlayImg);
            }
        }
    }
    void HighlightSelectedElement(){foreach(Transform t in elementsListContent){
        CstmzElement ce=t.GetComponent<CstmzElement>();
        switch(typeSelected){
            case CstmzType.skin:
                if(ce.elementName==GetSkinName(skinName)){
                    ce.selectedBg.SetActive(true);
                    if(skinName.Contains("_"))ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(skinName);
                }else{ce.selectedBg.SetActive(false);}
                break;
            case CstmzType.trail:
                if(ce.elementName==trailName){ce.selectedBg.SetActive(true);}
                else{ce.selectedBg.SetActive(false);}
                break;
            case CstmzType.flares:
                if(ce.elementName==flaresName){ce.selectedBg.SetActive(true);}
                else{ce.selectedBg.SetActive(false);}
                break;
            case CstmzType.deathFx:
                if(ce.elementName==deathFxName){ce.selectedBg.SetActive(true);}
                else{ce.selectedBg.SetActive(false);}
                break;
            case CstmzType.music:
                if(ce.elementName==musicName){ce.selectedBg.SetActive(true);}
                else{ce.selectedBg.SetActive(false);}
                break;
        }
    }}
    void HighlightSelectedType(){foreach(Transform t in typesListContent){
        CstmzTypeElement ce=t.GetComponent<CstmzTypeElement>();
        if(ce.elementType==typeSelected){
            ce.selectedBg.SetActive(true);
            //if(skinName.Contains("_"))ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }
        else{ce.selectedBg.SetActive(false);}

        if(ce.elementType==CstmzType.skin){
            ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(skinName);
        }else if(ce.elementType==CstmzType.trail){
            Destroy(ce.overlayImg);
            ce.elementPv.GetComponent<Image>().sprite=null;ce.elementPv.GetComponent<Image>().color=new Color(1,1,1,1f/255f);
            for(var i=0;i<ce.elementPv.transform.childCount;i++){Destroy(ce.elementPv.transform.GetChild(i).gameObject);}
            GameObject goPt=Instantiate(GetTrail(trailName).part,ce.elementPv.transform);
            GameAssets.instance.TransformIntoUIParticle(goPt);
        }else if(ce.elementType==CstmzType.flares){
            Destroy(ce.overlayImg);
            for(var i=0;i<ce.elementPv.transform.childCount;i++){Destroy(ce.elementPv.transform.GetChild(i).gameObject);}
            GameObject goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
            goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
        }else if(ce.elementType==CstmzType.deathFx){
            Destroy(ce.overlayImg);
            ce.elementPv.GetComponent<Image>().sprite=null;ce.elementPv.GetComponent<Image>().color=new Color(1,1,1,1f/255f);
            GameObject goPt=Instantiate(GetDeathFxObj(ce.elementName),ce.elementPv.transform);
            GameAssets.instance.TransformIntoUIParticle(goPt,0,-1,true);
        }if(ce.elementType==CstmzType.music){
            ce.elementPv.GetComponent<Image>().sprite=GetMusic(musicName).icon;
        }
    }}
    
    [HideInInspector]public string[] _CstmzCategoryNames=new string[]{"Special","Shop","ReOne","TwoPiece"};
    public void ChangeCategory(string str){
        if(str.Contains(_CstmzCategoryNames[0])){categorySelected=CstmzCategory.special;}
        else if(str.Contains(_CstmzCategoryNames[1])){categorySelected=CstmzCategory.shop;}
        else if(str.Contains(_CstmzCategoryNames[2])){categorySelected=CstmzCategory.reOne;}
        else if(str.Contains(_CstmzCategoryNames[3])){categorySelected=CstmzCategory.twoPiece;}
        categoriesDropdown.SetValueFromSelected();
        RecreateAllElements();
        CloseVariants();
    }public void SetCategory(CstmzCategory cat){
        categorySelected=cat;
        categoriesDropdown.SetValueFromSelected();
        RecreateAllElements();
        CloseVariants();
    }public void SetCategoryFromTypeElement(){
        if(typeSelected==CstmzType.skin){categorySelected=GetSkin(skinName).category;}
        else if(typeSelected==CstmzType.trail){categorySelected=GetTrail(trailName).category;}
        else if(typeSelected==CstmzType.flares){categorySelected=GetFlares(flaresName).category;}
        else if(typeSelected==CstmzType.deathFx){categorySelected=GetDeathFx(deathFxName).category;}
        else if(typeSelected==CstmzType.music){categorySelected=GetMusic(musicName).category;}
        categoriesDropdown.SetValueFromSelected();
        RecreateAllElements();
        CloseVariants();
    }


    public void OpenVariants(string str){   string _str=str;
        if(!String.IsNullOrEmpty(_str)){_str=skinName;}
        if(GetSkin(_str).variants.Count>0||GetOverlaySprite(_str)!=null){
            variantsPanel.SetActive(true);
            if(GetOverlaySprite(_str)!=null||GetSkin(_str).variants.Find(x=>x.sprOverlay!=null)!=null){colorSliders.SetActive(true);}
            RecreateAllVariants(_str);
        }
    }
    public void CloseVariants(){variantsPanel.SetActive(false);}
    public void RecreateAllVariants(string str){DeleteAllVariantElements();CreateAllVariantElements(str);HighlightSelectedVariant();}
    void DeleteAllVariantElements(){foreach(Transform t in variantsListContent){Destroy(t.gameObject);}}
    void CreateAllVariantElements(string str){
        //Create first default variant
        var go1=Instantiate(cstmzElementPrefab,variantsListContent);
        go1.name="SkinVariant_-1";
        CstmzElement ce1=go1.GetComponent<CstmzElement>();
        ce1.variant=true;ce1.variantId=-1;
        ce1.elementName=GetSkin(str).name;
        ce1.rarity=GetSkin(str).rarity;
        ce1.elementPv.GetComponent<Image>().sprite=GetSkinSprite(str);
        if(GetOverlaySprite(str)!=null)ce1.overlayImg.GetComponent<Image>().sprite=GetOverlaySprite(str);
        //Create all others
        List<CstmzSkinVariant> variants=GetSkin(str).variants;
        for(int i=0;i<variants.Count;i++){
            var gs=variants[i];
            var go=Instantiate(cstmzElementPrefab,variantsListContent);
            go.name="SkinVariant_"+i;
            var ce=go.GetComponent<CstmzElement>();
            ce.variant=true;ce.variantId=i;
            ce.elementName=GetSkin(str).name;
            ce.rarity=GetSkin(str).rarity;
            ce.elementPv.GetComponent<Image>().sprite=gs.spr;
            //ce.elementPv.GetComponent<Image>().sprite=GetSkinSprite(str);
            if(gs.sprOverlay!=null){ce.overlayImg.GetComponent<Image>().sprite=gs.sprOverlay;}
        }
    }
    void HighlightSelectedVariant(){foreach(Transform t in variantsListContent){
        CstmzElement ce=t.GetComponent<CstmzElement>();
        if((skinName.Contains("_")&&int.Parse(skinName.Split('_')[1])==ce.variantId)||
        (!skinName.Contains("_")&&GetSkinName(skinName)==ce.elementName&&ce.variantId==-1)){ce.selectedBg.SetActive(true);}
        else{ce.selectedBg.SetActive(false);}
    }}

    public void RecreateAllLockboxElements(){DeleteAllLockboxElements();CreateAllLockboxElements();}
    public IEnumerator RecreateAllLockboxElementsI(){DeleteAllLockboxElements();yield return new WaitForSeconds(0.01f);CreateAllElements();}
    void DeleteAllLockboxElements(){foreach(Transform t in lockboxElementListContent){Destroy(t.gameObject);}}
    void CreateAllLockboxElements(){
        foreach(CstmzLockbox lb in GameAssets.instance.lockboxes){
            var go=Instantiate(lockboxElementPrefab,lockboxElementListContent);
            go.name="LockboxElement_"+lb.name;
            LockboxElement le=go.GetComponent<LockboxElement>();
            le.name=lb.name;
            le.titleText.text=lb.displayName+" Lockbox";
            le.iconImg.sprite=lb.icon;
        }
    }

    float refreshTimer;
    void RefreshParticles(){if(refreshTimer>0){refreshTimer-=Time.deltaTime;}if(refreshTimer<=0){
        if(typeSelected==CstmzType.flares){
            foreach(CstmzElement ce in elementsListContent.GetComponentsInChildren<CstmzElement>()){
                if(ce.elementPv.transform.childCount==0){
                    GameObject goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
                    GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
                    goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
                    GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
                }
            }
        }
        CstmzTypeElement typeFlares=Array.Find(typesListContent.GetComponentsInChildren<CstmzTypeElement>(),x=>x.elementType==CstmzType.flares);
        if(typeFlares.elementPv.transform.childCount==0){
            GameObject goPt=Instantiate(GetFlareVFX(flaresName),typeFlares.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
            goPt=Instantiate(GetFlareVFX(flaresName),typeFlares.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
            GameAssets.instance.TransformIntoUIParticle(goPt,0,-1);
        }

        if(typeSelected==CstmzType.deathFx){
            foreach(CstmzElement ce in elementsListContent.GetComponentsInChildren<CstmzElement>()){
                if(ce.elementPv.transform.childCount==0){
                    GameObject goPt=Instantiate(GetDeathFxObj(ce.elementName),ce.elementPv.transform);
                    GameAssets.instance.TransformIntoUIParticle(goPt,0,-1,true);
                }
            }
        }
        CstmzTypeElement typeDeathFx=Array.Find(typesListContent.GetComponentsInChildren<CstmzTypeElement>(),x=>x.elementType==CstmzType.deathFx);
        if(typeDeathFx.elementPv.transform.childCount==0){
            var pt=Instantiate(GetDeathFxObj(deathFxName),typeDeathFx.elementPv.transform);
            GameAssets.instance.TransformIntoUIParticle(pt,0,-1,true);
        }
    }}

    public void PlayRaritySound(CstmzRarity rarity){
        if(rarity==CstmzRarity.epic){AudioManager.instance.Play("DropEpic");}
        else if(rarity==CstmzRarity.legend){AudioManager.instance.Play("DropLegend");}
    }
    public Color GetRarityColor(CstmzRarity rarity){
        var col=Color.white;
        switch(rarity){
            case CstmzRarity.def:col=defColor;break;
            case CstmzRarity.common:col=commonColor;break;
            case CstmzRarity.rare:col=rareColor;break;
            case CstmzRarity.epic:col=epicColor;break;
            case CstmzRarity.legend:col=legendColor;break;
        }
        return col;
    }
    public void SetType(CstmzType type){if(typeSelected!=type){typeSelected=type;HighlightSelectedType();SetCategoryFromTypeElement();RecreateAllElements();CloseVariants();refreshTimer=0.04f;}}

    string GetSkinName(string str){string _str=str;if(skinName.Contains("_")){_str=skinName.Split('_')[0];}return _str;}
    public CstmzSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return GameAssets.instance.GetSkin(_str);}
    CstmzSkin GetSkinCurrent(){return GetSkin(skinName);}
    int GetSkinID(string str){return GameAssets.instance.GetSkinID(str);}
    CstmzSkin GetSkinByID(int i){return GameAssets.instance.GetSkinByID(i);}
    public Sprite GetSkinSprite(string str){return ShipCustomizationManager.instance.GetSkinSprite(str);}
    public Sprite GetOverlaySprite(string str){return ShipCustomizationManager.instance.GetOverlaySprite(str);}
    CstmzSkinVariant GetSkinVariant(string str,int id){return GameAssets.instance.GetSkinVariant(str,id);}
    public bool SkinHasVariants(string str){bool b=false;if(GetSkin(str).variants.Count>0){b=true;}return b;}
    public void SetSkin(string str){skinName=str;if(variantsPanel.activeSelf){variantsPanel.SetActive(false);colorSliders.SetActive(false);}HighlightSelectedElement();HighlightSelectedType();
        if(skinName!="def"){StatsAchievsManager.instance.Customized();}}

    public CstmzTrail GetTrail(string str){return GameAssets.instance.GetTrail(str);}
    public void SetTrail(string str){trailName=str;HighlightSelectedElement();HighlightSelectedType();
        if(trailName!="def"){StatsAchievsManager.instance.Customized();}}

    public CstmzFlares GetFlares(string str){return GameAssets.instance.GetFlares(str);}
    public GameObject GetFlareVFX(string str){GameObject go=null;
        if(GameAssets.instance.GetFlares(str)!=null){go=GameAssets.instance.GetFlareRandom(str);}
        return go;
    }
    public void SetFlares(string str){flaresName=str;HighlightSelectedElement();HighlightSelectedType();
        if(flaresName!="def"){StatsAchievsManager.instance.Customized();}}

    public CstmzDeathFx GetDeathFx(string str){return GameAssets.instance.GetDeathFx(str);}
    public GameObject GetDeathFxObj(string str){GameObject go=null;
        if(GameAssets.instance.GetDeathFx(str)!=null){go=GameAssets.instance.GetDeathFx(str).obj;}
        return go;
    }
    public void SetDeathFx(string str){deathFxName=str;HighlightSelectedElement();HighlightSelectedType();}
    public CstmzMusic GetMusic(string str){return GameAssets.instance.GetMusic(str);}
    public void SetMusic(string str){musicName=str;if(Jukebox.instance==null){Instantiate(GameCreator.instance.GetJukeboxPrefab());}Jukebox.instance.SetMusic(GameAssets.instance.GetMusic(musicName).track,true);HighlightSelectedElement();HighlightSelectedType();}


    public static bool _isSkinUnlocked(string name){return SaveSerial.instance.playerData.skinsUnlocked.Contains(name)||name=="def";}
    public static bool _isTrailUnlocked(string name){return SaveSerial.instance.playerData.trailsUnlocked.Contains(name)||name=="def";}
    public static bool _isFlaresUnlocked(string name){return SaveSerial.instance.playerData.flaresUnlocked.Contains(name)||name=="def";}
    public static bool _isDeathFxUnlocked(string name){return SaveSerial.instance.playerData.deathFxUnlocked.Contains(name)||name=="def";}
    public static bool _isMusicUnlocked(string name){return SaveSerial.instance.playerData.musicUnlocked.Contains(name)||name==CstmzMusic._cstmzMusicDef;}
    public static bool _allCategorySkinsUnlocked(CstmzCategory category){
        var count=0;var _allCategoryItems=GameAssets.instance.skins.FindAll(x=>x.category==category);
        foreach(CstmzSkin s in _allCategoryItems){if(SaveSerial.instance.playerData.skinsUnlocked.Contains(s.name))count++;}
        return count==_allCategoryItems.Count;
    }
    public static bool _allCategoryTrailsUnlocked(CstmzCategory category){
        var count=0;var _allCategoryItems=GameAssets.instance.trails.FindAll(x=>x.category==category);
        foreach(CstmzTrail t in _allCategoryItems){if(SaveSerial.instance.playerData.trailsUnlocked.Contains(t.name))count++;}
        return count==_allCategoryItems.Count;
    }
    public static bool _allCategoryFlaresUnlocked(CstmzCategory category){
        var count=0;var _allCategoryItems=GameAssets.instance.flares.FindAll(x=>x.category==category);
        foreach(CstmzFlares f in _allCategoryItems){if(SaveSerial.instance.playerData.flaresUnlocked.Contains(f.name))count++;}
        return count==_allCategoryItems.Count;
    }
    public static bool _allCategoryDeathFxUnlocked(CstmzCategory category){
        var count=0;var _allCategoryItems=GameAssets.instance.deathFxs.FindAll(x=>x.category==category);
        foreach(CstmzDeathFx d in _allCategoryItems){if(SaveSerial.instance.playerData.deathFxUnlocked.Contains(d.name))count++;}
        return count==_allCategoryItems.Count;
    }
    public static bool _allCategoryMusicUnlocked(CstmzCategory category){
        var count=0;var _allCategoryItems=GameAssets.instance.musics.FindAll(x=>x.category==category);
        foreach(CstmzMusic m in _allCategoryItems){if(SaveSerial.instance.playerData.deathFxUnlocked.Contains(m.name))count++;}
        return count==_allCategoryItems.Count;
    }
    public static bool _literallyEverythingInCategoryUnlocked(CstmzCategory category){return (_allCategorySkinsUnlocked(category)&&_allCategoryTrailsUnlocked(category)&&_allCategoryFlaresUnlocked(category)&&_allCategoryDeathFxUnlocked(category)&&_allCategoryMusicUnlocked(category));}
    public static void UnlockSkin(string name){if(!_isSkinUnlocked(name)){SaveSerial.instance.playerData.skinsUnlocked.Add(name);}}
    public static void UnlockTrail(string name){if(!_isTrailUnlocked(name)){SaveSerial.instance.playerData.trailsUnlocked.Add(name);}}
    public static void UnlockFlares(string name){if(!_isFlaresUnlocked(name)){SaveSerial.instance.playerData.flaresUnlocked.Add(name);}}
    public static void UnlockDeathFx(string name){if(!_isDeathFxUnlocked(name)){SaveSerial.instance.playerData.deathFxUnlocked.Add(name);}}
    public static void UnlockMusic(string name){if(!_isMusicUnlocked(name)){SaveSerial.instance.playerData.musicUnlocked.Add(name);}}
}