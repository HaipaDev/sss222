using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SceneObjectsOnly][SerializeField] CosmeticDrop cosmeticDropParent;
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
    [DisableInEditorMode] public string _lockboxSelected;
    [ReadOnly][SerializeField]bool loaded;
    void Awake(){instance=this;}
    IEnumerator Start(){
        yield return new WaitForSecondsRealtime(0.02f);
        if(String.IsNullOrEmpty(skinName)||GetSkin(skinName)==null||!_isSkinUnlocked(skinName)){skinName="def";}
        if(String.IsNullOrEmpty(trailName)||GetTrail(trailName)==null||!_isTrailUnlocked(trailName)){trailName="def";}
        if(String.IsNullOrEmpty(flaresName)||GetFlares(flaresName)==null||!_isFlaresUnlocked(flaresName)){flaresName="def";}
        if(String.IsNullOrEmpty(deathFxName)||GetDeathFx(deathFxName)==null||!_isDeathFxUnlocked(deathFxName)){deathFxName="def";}
        if(String.IsNullOrEmpty(musicName)||GetMusic(musicName)==null||!_isMusicUnlocked(musicName)){musicName=CstmzMusic._cstmzMusicDef;}
        skinName=SaveSerial.instance.playerData.skinName;
        SetCategory(AssetsManager.instance.skins.Find(x=>x.name.Contains(GetSkinName(SaveSerial.instance.playerData.skinName))).category);

        foreach(CstmzSkin s in AssetsManager.instance.skins){if(s.name!="def"&&s.rarity==CstmzRarity.def)UnlockSkin(s.name);}
        foreach(CstmzTrail t in AssetsManager.instance.trails){if(t.name!="def"&&t.rarity==CstmzRarity.def)UnlockTrail(t.name);}
        foreach(CstmzFlares f in AssetsManager.instance.flares){if(f.name!="def"&&f.rarity==CstmzRarity.def)UnlockFlares(f.name);}
        foreach(CstmzDeathFx d in AssetsManager.instance.deathFxs){if(d.name!="def"&&d.rarity==CstmzRarity.def)UnlockDeathFx(d.name);}
        foreach(CstmzMusic m in AssetsManager.instance.musics){if(m.name!=CstmzMusic._cstmzMusicDef&&m.rarity==CstmzRarity.def)UnlockMusic(m.name);}

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

        foreach(CstmzLockbox lb in AssetsManager.instance.lockboxes){if(!SaveSerial.instance.playerData.lockboxesInventory.Exists(x=>x.name==lb.name)){SaveSerial.instance.playerData.lockboxesInventory.Add(new LockboxCount{name=lb.name,count=0});}}
    }
    //void OnDestroy(){DestroyImmediate(SsliderIMG.material);}
    //void OnDisable(){DestroyImmediate(SsliderIMG.material);}
    public void ValueChangeCheck(){
        overlayColor=Color.HSVToRGB((float)System.Math.Round(Hslider.value,2),(float)System.Math.Round(Sslider.value,2),(float)System.Math.Round(Vslider.value,2));

        SsliderIMG.material.SetColor("_Color2",Color.HSVToRGB(Hslider.value,1,1));
        VsliderIMG.color = Color.HSVToRGB((float)System.Math.Round(Hslider.value,2), 1, 1);
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
    public void OpenLockboxOpeningPanel(string name){CloseAllPanels();lockboxOpeningPanel.SetActive(true);_lockboxSelected=name;PreLockboxOpen();OpenLockbox(name);}
    public void CloseAllPanels(){customizationPanel.SetActive(false);lockboxesPanel.SetActive(false);lockboxOpeningPanel.SetActive(false);}
    public void OpenLockbox(string name){_lockboxSelected=name;StartCoroutine(OpenLockboxI());}
    void PreLockboxOpen(){
        lockboxIcon.sprite=AssetsManager.instance.lockboxes.Find(x=>x.name==_lockboxSelected).icon;
        cosmeticDropParent.PresetCosmeticDrop();
    }
    IEnumerator OpenLockboxI(){
        AudioManager.instance.Play("LockboxOpen");
        yield return new WaitForSeconds(0.3f);
        var lb=AssetsManager.instance.lockboxes.Find(x=>x.name==_lockboxSelected);
        lockboxIcon.sprite=lb.iconOpen;
        SetDroppedItem();
        _itemDropped=true;
    }
    public void SetDroppedItem(){
        var lb=AssetsManager.instance.lockboxes.Find(x=>x.name==_lockboxSelected);
        LootTableCstmz lt=gameObject.AddComponent<LootTableCstmz>();
        lt.itemList=new List<LootTableEntryCstmz>();
        foreach(CstmzSkin s in AssetsManager.instance.skins){if(s.category==lb.category&&s.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=s.name,cstmzType=CstmzType.skin,dropChance=lb.skinDrops.Find(x=>x.rarity==s.rarity).chance});}}
        foreach(CstmzTrail t in AssetsManager.instance.trails){if(t.category==lb.category&&t.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=t.name,cstmzType=CstmzType.trail,dropChance=lb.trailDrops.Find(x=>x.rarity==t.rarity).chance});}}
        foreach(CstmzFlares f in AssetsManager.instance.flares){if(f.category==lb.category&&f.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=f.name,cstmzType=CstmzType.flares,dropChance=lb.flareDrops.Find(x=>x.rarity==f.rarity).chance});}}
        foreach(CstmzDeathFx d in AssetsManager.instance.deathFxs){if(d.category==lb.category&&d.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=d.name,cstmzType=CstmzType.deathFx,dropChance=lb.deathFxDrops.Find(x=>x.rarity==d.rarity).chance});}}
        foreach(CstmzMusic m in AssetsManager.instance.musics){if(m.category==lb.category&&m.rarity!=0){lt.itemList.Add(new LootTableEntryCstmz{lootItem=m.name,cstmzType=CstmzType.music,dropChance=lb.musicDrops.Find(x=>x.rarity==m.rarity).chance});}}
        lt.SumUp();

        KeyValuePair<string,CstmzType> i=new KeyValuePair<string,CstmzType>("",0);
        while(i.Key==""&&!_literallyEverythingInCategoryUnlocked(lb.category)){
            LootTableEntryCstmz r=lt.GetItem();
            Debug.Log(r.lootItem+" | "+r.cstmzType+" | "+r.dropChance);
            if(r.cstmzType==CstmzType.skin&&(!_isSkinUnlocked(r.lootItem)||_allCategorySkinsUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzSkin s=GetSkin(i.Key);
                cosmeticDropParent.SetSkin(s);
                PlayRaritySound(s.rarity);
                UnlockSkin(r.lootItem);
                if(_allCategorySkinsUnlocked(lb.category)&&!(_allCategoryTrailsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){/*i=new KeyValuePair<string,CstmzType>("",0);*/Debug.Log("All Skins unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.trail&&(!_isTrailUnlocked(r.lootItem)||_allCategoryTrailsUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzTrail t=GetTrail(i.Key);
                cosmeticDropParent.SetTrail(t);
                PlayRaritySound(t.rarity);
                UnlockTrail(r.lootItem);
                if(_allCategoryTrailsUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){/*i=new KeyValuePair<string,CstmzType>("",0);*/Debug.Log("All Trails unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.flares&&(!_isFlaresUnlocked(r.lootItem)||_allCategoryFlaresUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzFlares f=GetFlares(i.Key);
                cosmeticDropParent.SetFlares(f);
                PlayRaritySound(f.rarity);
                UnlockFlares(r.lootItem);
                if(_allCategoryFlaresUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryTrailsUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){/*i=new KeyValuePair<string,CstmzType>("",0);*/Debug.Log("All Flares unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.deathFx&&(!_isDeathFxUnlocked(r.lootItem)||_allCategoryDeathFxUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzDeathFx d=GetDeathFx(i.Key);
                cosmeticDropParent.SetDeathFx(d);
                PlayRaritySound(d.rarity);
                UnlockDeathFx(r.lootItem);
                if(_allCategoryDeathFxUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryTrailsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryMusicUnlocked(lb.category))){/*i=new KeyValuePair<string,CstmzType>("",0);*/Debug.Log("All DeathFx unlocked, trying to find other items");}
            }
            else if(r.cstmzType==CstmzType.music&&(!_isMusicUnlocked(r.lootItem)||_allCategoryMusicUnlocked(lb.category))){
                i=new KeyValuePair<string,CstmzType>(r.lootItem,r.cstmzType);
                CstmzMusic m=GetMusic(i.Key);
                cosmeticDropParent.SetMusic(m);
                PlayRaritySound(m.rarity);
                UnlockMusic(r.lootItem);
                if(_allCategoryMusicUnlocked(lb.category)&&!(_allCategorySkinsUnlocked(lb.category)||_allCategoryTrailsUnlocked(lb.category)||_allCategoryFlaresUnlocked(lb.category)||_allCategoryDeathFxUnlocked(lb.category))){/*i=new KeyValuePair<string,CstmzType>("",0);*/Debug.Log("All Music unlocked, trying to find other items");}
            }
            //dropTypeText.text=r.cstmzType.ToString();
            if(r.cstmzType!=CstmzType.skin){cosmeticDropParent.StopAnimatingCosmeticDrop();}
        }
        if(_literallyEverythingInCategoryUnlocked(lb.category)){QuitOpeningLockbox();SaveSerial.instance.playerData.dynamCelestStars+=lb.cost;}
        Destroy(lt);
    }
    public void OpenCloseLockboxButton(){QuitOpeningLockbox();}//if(!_itemDropped){OpenLockbox(_lockboxSelected);}else{QuitOpeningLockbox();}}
    public void QuitOpeningLockbox(){if(_itemDropped){OpenLockboxesPanel();_itemDropped=false;cosmeticDropParent.Stop();}}
    public void CraftCelestStar(){
        if(SaveSerial.instance.playerData.starshards>=dynamCelestStar_shardCost){
            if(AssetsManager.CheckChance(dynamCelestStar_craftChance)){
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
            var currentCategorySkins=AssetsManager.instance.skins.FindAll(x=>x.category==categorySelected);
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
            var currentCategoryTrails=AssetsManager.instance.trails.FindAll(x=>x.category==categorySelected);
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
                AssetsManager.instance.TransformIntoUIParticle(goPt);
            }
        }else if(typeSelected==CstmzType.flares){
            var currentCategoryFlares=AssetsManager.instance.flares.FindAll(x=>x.category==categorySelected);
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
                AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
                goPt=Instantiate(GetFlareVFX(ge.name),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
                AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
            }
        }else if(typeSelected==CstmzType.deathFx){
            var currentCategoryDeathFxs=AssetsManager.instance.deathFxs.FindAll(x=>x.category==categorySelected);
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
                AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1,true);
            }
        }else if(typeSelected==CstmzType.music){
            var currentCategoryMusic=AssetsManager.instance.musics.FindAll(x=>x.category==categorySelected);
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
            AssetsManager.instance.TransformIntoUIParticle(goPt);
        }else if(ce.elementType==CstmzType.flares){
            Destroy(ce.overlayImg);
            for(var i=0;i<ce.elementPv.transform.childCount;i++){Destroy(ce.elementPv.transform.GetChild(i).gameObject);}
            GameObject goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
            AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
            goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
            AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
        }else if(ce.elementType==CstmzType.deathFx){
            Destroy(ce.overlayImg);
            ce.elementPv.GetComponent<Image>().sprite=null;ce.elementPv.GetComponent<Image>().color=new Color(1,1,1,1f/255f);
            GameObject goPt=Instantiate(GetDeathFxObj(ce.elementName),ce.elementPv.transform);
            AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1,true);
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
        foreach(CstmzLockbox lb in AssetsManager.instance.lockboxes){
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
                    AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
                    goPt=Instantiate(GetFlareVFX(ce.elementName),ce.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
                    AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
                }
            }
        }
        CstmzTypeElement typeFlares=Array.Find(typesListContent.GetComponentsInChildren<CstmzTypeElement>(),x=>x.elementType==CstmzType.flares);
        if(typeFlares.elementPv.transform.childCount==0){
            GameObject goPt=Instantiate(GetFlareVFX(flaresName),typeFlares.elementPv.transform);goPt.transform.localPosition=new Vector2(-44,0);
            AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
            goPt=Instantiate(GetFlareVFX(flaresName),typeFlares.elementPv.transform);goPt.transform.localPosition=new Vector2(44,0);
            AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1);
        }

        if(typeSelected==CstmzType.deathFx){
            foreach(CstmzElement ce in elementsListContent.GetComponentsInChildren<CstmzElement>()){
                if(ce.elementPv.transform.childCount==0){
                    GameObject goPt=Instantiate(GetDeathFxObj(ce.elementName),ce.elementPv.transform);
                    AssetsManager.instance.TransformIntoUIParticle(goPt,0,-1,true);
                }
            }
        }
        CstmzTypeElement typeDeathFx=Array.Find(typesListContent.GetComponentsInChildren<CstmzTypeElement>(),x=>x.elementType==CstmzType.deathFx);
        if(typeDeathFx.elementPv.transform.childCount==0){
            var pt=Instantiate(GetDeathFxObj(deathFxName),typeDeathFx.elementPv.transform);
            AssetsManager.instance.TransformIntoUIParticle(pt,0,-1,true);
        }
    }}

    public void PlayRaritySound(CstmzRarity rarity){
        if(rarity==CstmzRarity.epic){AudioManager.instance.Play("DropEpic");}
        else if(rarity==CstmzRarity.legend){AudioManager.instance.Play("DropLegend");}
    }
    public void SetType(CstmzType type){if(typeSelected!=type){typeSelected=type;HighlightSelectedType();SetCategoryFromTypeElement();RecreateAllElements();CloseVariants();refreshTimer=0.04f;}}

    string GetSkinName(string str){string _str=str;if(skinName.Contains("_")){_str=skinName.Split('_')[0];}return _str;}
    public CstmzSkin GetSkin(string str){string _str=str;if(_str.Contains("_")){_str=_str.Split('_')[0];}return AssetsManager.instance.GetSkin(_str);}
    CstmzSkin GetSkinCurrent(){return GetSkin(skinName);}
    int GetSkinID(string str){return AssetsManager.instance.GetSkinID(str);}
    CstmzSkin GetSkinByID(int i){return AssetsManager.instance.GetSkinByID(i);}
    public Sprite GetSkinSprite(string str){return ShipCustomizationManager.instance.GetSkinSprite(str);}
    public Sprite GetOverlaySprite(string str){return ShipCustomizationManager.instance.GetOverlaySprite(str);}
    CstmzSkinVariant GetSkinVariant(string str,int id){return AssetsManager.instance.GetSkinVariant(str,id);}
    public bool SkinHasVariants(string str){bool b=false;if(GetSkin(str).variants.Count>0){b=true;}return b;}
    public void SetSkin(string str){skinName=str;if(variantsPanel.activeSelf){variantsPanel.SetActive(false);colorSliders.SetActive(false);}HighlightSelectedElement();HighlightSelectedType();
        if(skinName!="def"){StatsAchievsManager.instance.Customized();}}

    public CstmzTrail GetTrail(string str){return AssetsManager.instance.GetTrail(str);}
    public void SetTrail(string str){trailName=str;HighlightSelectedElement();HighlightSelectedType();
        if(trailName!="def"){StatsAchievsManager.instance.Customized();}}

    public CstmzFlares GetFlares(string str){return AssetsManager.instance.GetFlares(str);}
    public GameObject GetFlareVFX(string str){GameObject go=null;
        if(AssetsManager.instance.GetFlares(str)!=null){go=AssetsManager.instance.GetFlareRandom(str);}
        return go;
    }
    public void SetFlares(string str){flaresName=str;HighlightSelectedElement();HighlightSelectedType();
        if(flaresName!="def"){StatsAchievsManager.instance.Customized();}}

    public CstmzDeathFx GetDeathFx(string str){return AssetsManager.instance.GetDeathFx(str);}
    public GameObject GetDeathFxObj(string str){GameObject go=null;
        if(AssetsManager.instance.GetDeathFx(str)!=null){go=AssetsManager.instance.GetDeathFx(str).obj;}
        return go;
    }
    public void SetDeathFx(string str){deathFxName=str;HighlightSelectedElement();HighlightSelectedType();}
    public CstmzMusic GetMusic(string str){return AssetsManager.instance.GetMusic(str);}
    public void SetMusic(string str){musicName=str;if(Jukebox.instance==null){Instantiate(CoreSetup.instance.GetJukeboxPrefab());}Jukebox.instance.SetMusic(AssetsManager.instance.GetMusic(musicName).track,true);HighlightSelectedElement();HighlightSelectedType();}

    /*private static HashSet<string> unlockedSkins = new HashSet<string>(SaveSerial.instance.playerData.skinsUnlocked);
    private static HashSet<string> unlockedTrail = new HashSet<string>(SaveSerial.instance.playerData.trailsUnlocked);
    private static HashSet<string> unlockedFlares = new HashSet<string>(SaveSerial.instance.playerData.flaresUnlocked);
    private static HashSet<string> unlockedDeathFx = new HashSet<string>(SaveSerial.instance.playerData.deathFxUnlocked);
    private static HashSet<string> unlockedMusic = new HashSet<string>(SaveSerial.instance.playerData.musicUnlocked);
    public static bool _isSkinUnlocked(string name){return unlockedSkins.Contains(name)||name=="def";}
    public static bool _isTrailUnlocked(string name){return unlockedTrail.Contains(name)||name=="def";}
    public static bool _isFlaresUnlocked(string name){return unlockedFlares.Contains(name)||name=="def";}
    public static bool _isDeathFxUnlocked(string name){return unlockedDeathFx.Contains(name)||name=="def";}
    public static bool _isMusicUnlocked(string name){return unlockedMusic.Contains(name)||name==CstmzMusic._cstmzMusicDef;}*/
    public static bool _isSkinUnlocked(string name){return SaveSerial.instance.playerData.skinsUnlocked.Contains(name)||name=="def";}

    public static bool _isTrailUnlocked(string name){return SaveSerial.instance.playerData.trailsUnlocked.Contains(name)||name=="def";}

    public static bool _isFlaresUnlocked(string name){return SaveSerial.instance.playerData.flaresUnlocked.Contains(name)||name=="def";}

    public static bool _isDeathFxUnlocked(string name){return SaveSerial.instance.playerData.deathFxUnlocked.Contains(name)||name=="def";}

    public static bool _isMusicUnlocked(string name){return SaveSerial.instance.playerData.musicUnlocked.Contains(name)||name==CstmzMusic._cstmzMusicDef;}
    
    public static bool _allCategorySkinsUnlocked(CstmzCategory category){
        var unlockedSet=new HashSet<string>(SaveSerial.instance.playerData.skinsUnlocked);
        var allCategoryItems=AssetsManager.instance.skins.FindAll(x=>x.category==category);
        foreach (CstmzSkin s in allCategoryItems){
            if(!unlockedSet.Contains(s.name)){
                return false;// optimization: stop early if an item is not unlocked
            }
        }
        return true;
    }
    public static bool _allCategoryTrailsUnlocked(CstmzCategory category){
        var unlockedSet = new HashSet<string>(SaveSerial.instance.playerData.trailsUnlocked);
        var allCategoryItems = AssetsManager.instance.trails.FindAll(x => x.category == category);
        foreach(CstmzTrail t in allCategoryItems){
            if(!unlockedSet.Contains(t.name)){
                return false; // optimization: stop early if an item is not unlocked
            }
        }
        return true;
    }

    public static bool _allCategoryFlaresUnlocked(CstmzCategory category){
        var unlockedSet = new HashSet<string>(SaveSerial.instance.playerData.flaresUnlocked);
        var allCategoryItems = AssetsManager.instance.flares.FindAll(x => x.category == category);
        foreach(CstmzFlares f in allCategoryItems){
            if(!unlockedSet.Contains(f.name)){
                return false; // optimization: stop early if an item is not unlocked
            }
        }
        return true;
    }

    public static bool _allCategoryDeathFxUnlocked(CstmzCategory category){
        var unlockedSet = new HashSet<string>(SaveSerial.instance.playerData.deathFxUnlocked);
        var allCategoryItems = AssetsManager.instance.deathFxs.FindAll(x => x.category == category);
        foreach(CstmzDeathFx d in allCategoryItems){
            if(!unlockedSet.Contains(d.name)){
                return false; // optimization: stop early if an item is not unlocked
            }
        }
        return true;
    }

    public static bool _allCategoryMusicUnlocked(CstmzCategory category){
        var unlockedSet = new HashSet<string>(SaveSerial.instance.playerData.musicUnlocked);
        var allCategoryItems = AssetsManager.instance.musics.FindAll(x => x.category == category);
        foreach (CstmzMusic m in allCategoryItems){
            if(!unlockedSet.Contains(m.name)){
                return false; // optimization: stop early if an item is not unlocked
            }
        }
        return true;
    }
    public static bool _literallyEverythingInCategoryUnlocked(CstmzCategory category){return (_allCategorySkinsUnlocked(category)&&_allCategoryTrailsUnlocked(category)&&_allCategoryFlaresUnlocked(category)&&_allCategoryDeathFxUnlocked(category)&&_allCategoryMusicUnlocked(category));}
    public static void UnlockSkin(string name,bool _outsideOfLockbox=false){if(!_isSkinUnlocked(name)){SaveSerial.instance.playerData.skinsUnlocked.Add(name);if(_outsideOfLockbox)CosmeticPopups.instance.AddToQueue(name,CstmzType.skin);}}
    public static void UnlockTrail(string name,bool _outsideOfLockbox=false){if(!_isTrailUnlocked(name)){SaveSerial.instance.playerData.trailsUnlocked.Add(name);if(_outsideOfLockbox)CosmeticPopups.instance.AddToQueue(name,CstmzType.trail);}}
    public static void UnlockFlares(string name,bool _outsideOfLockbox=false){if(!_isFlaresUnlocked(name)){SaveSerial.instance.playerData.flaresUnlocked.Add(name);if(_outsideOfLockbox)CosmeticPopups.instance.AddToQueue(name,CstmzType.flares);}}
    public static void UnlockDeathFx(string name,bool _outsideOfLockbox=false){if(!_isDeathFxUnlocked(name)){SaveSerial.instance.playerData.deathFxUnlocked.Add(name);if(_outsideOfLockbox)CosmeticPopups.instance.AddToQueue(name,CstmzType.deathFx);}}
    public static void UnlockMusic(string name,bool _outsideOfLockbox=false){if(!_isMusicUnlocked(name)){SaveSerial.instance.playerData.musicUnlocked.Add(name);if(_outsideOfLockbox)CosmeticPopups.instance.AddToQueue(name,CstmzType.music);}}

    public static void _unlockGoldenMoyai(){CustomizationInventory.UnlockSkin("moyaiGold",true);CustomizationInventory.UnlockTrail("goldenflame",true);CustomizationInventory.UnlockFlares("golden",true);}
    public static void _unlockStargazer(){CustomizationInventory.UnlockSkin("stargazer",true);CustomizationInventory.UnlockTrail("stardust",true);}
    public static void _unlockMOL(){CustomizationInventory.UnlockSkin("maniac",true);}
    public static void _unlockSteam(){CustomizationInventory.UnlockSkin("chameleon",true);CustomizationInventory.UnlockMusic("one",true);}
}