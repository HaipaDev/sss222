using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{
    public static Shop instance;
    public static bool shopOpen;
    public static bool shopOpened;
    public GameObject shopMenuUI;
    public GameObject slotsContainer;
    public GameObject slotPrefab;
    public int[] slotUnlock=new int[4];
    public float shopTimeMax=10f;

    public int currentSlotID;
    public int purchases;
    public bool purchased;
    public bool subbed;
    public int purchasedNotTimes;
    public bool repEnabled;
    public int reputation;
    public int reputationSlot;
    public float sum;
    public float shopTimer=-4;
    public LootTableShop lootTable;
    public float purchaseTimer=-4;

    private void Awake(){instance=this;}
    void Start(){
        var g=GameRules.instance;
        if(g!=null){
            lootTable.itemList=g.shopList;
            repEnabled=g.repEnabled;
            if(g.shopTimeLimitEnabled){shopTimeMax=g.shopTimeLimit;}else{shopTimeMax=-5;}
        }
        lootTable=GetComponent<LootTableShop>();
        NewQueue();//First queue
        shopMenuUI.SetActive(false);
        shopTimer=shopTimeMax;
    }

    void Update(){
        if(shopOpen==true){OpenShop();}
        if(Input.GetKeyDown(KeyCode.Escape)){Resume();}
        if(repEnabled)LevelRep();
        if(shopTimeMax!=-5&&shopOpened&&shopTimer>0){shopTimer-=Time.unscaledDeltaTime;}
        if(shopTimeMax!=-5&&shopTimer<=0&&shopTimer!=-4){Resume();}
        if(purchaseTimer>0){purchaseTimer-=Time.unscaledDeltaTime;}
        if(purchaseTimer<=0&&purchaseTimer!=-4){GameSession.instance.gameSpeed=0;foreach(Button bt in GetComponentsInChildren<Button>()){bt.interactable=true;}purchaseTimer=-4;}
    }
    public void SpawnCargo(){
        var cargoDir=dir.up;
        float xx=3.45f;
        float yy=7.4f;
        GameObject go;
        if(UnityEngine.Random.value<0.5f){cargoDir=dir.right;}else{cargoDir=dir.left;}
        if(cargoDir==dir.up){yy=7.4f;}
        if(cargoDir==dir.down){yy=-7.4f;}
        if(cargoDir==dir.left){xx=-4.55f;}
        if(cargoDir==dir.right){xx=4.55f;}
        if(cargoDir==dir.up||cargoDir==dir.down){if(UnityEngine.Random.value<0.5f){xx=-3.45f;}}
        if(cargoDir==dir.left||cargoDir==dir.right){yy=Random.Range(3.6f,-4.15f);}
        go=GameAssets.instance.Make("CargoShip",new Vector2(xx,yy));
        go.GetComponent<CargoShip>().SetCargoSpawnDir(cargoDir);
    }
    public void OpenShop(){
        purchased=false;
        shopMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        //RandomizeShop();
        shopOpen=false;
        GameSession.instance.gameSpeed=0;
        shopOpened=true;
        shopTimer=shopTimeMax;
    }
    public void Resume(){
        shopTimer=-4;
        if(purchased==false&&subbed==false){purchasedNotTimes++;}
        subbed=false;
        if(purchasedNotTimes==2){RepMinus(1);purchasedNotTimes=0;}
        shopMenuUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        shopOpened=false;
        GameSession.instance.gameSpeed=1f;
    }


    public void NewQueue(){
        ClearSlots();
        lootTable.currentQueue=lootTable.GetQueue();
        CreateSlot();
    }
    public void ClearSlots(){
        for(var go=0;go<slotsContainer.transform.childCount;go++){Destroy(slotsContainer.transform.GetChild(go).gameObject);}
    }
    public void CreateSlot(){
        if(currentSlotID<lootTable.currentQueue.slotList.Count){
            var go=Instantiate(slotPrefab,slotsContainer.transform);
            var slot=go.GetComponent<ShopSlot>();
            slot.SetItem(lootTable.currentQueue.GetItem(currentSlotID));
            currentSlotID++;
        }else{Debug.LogWarning("ShopSlot limit");}
        
    }
    public void CreateSlotDelay(){StartCoroutine(CreateSlotDelayI());}
    IEnumerator CreateSlotDelayI(){yield return new WaitForSecondsRealtime(0.1f);CreateSlot();}
    void LevelRep(){
        if(GetComponentInChildren<XPBars>()!=null){
        var bar=GetComponentInChildren<XPBars>();
        for(var i=0;i<slotUnlock.Length;i++){if(reputationSlot==slotUnlock[i]){var xp=slotUnlock[i+1];bar.ID=xp;if(bar.current==null){bar.Recreate();}CreateSlot();reputationSlot=0;}}}
    }

    //[SerializeReference]IEnumerator pco=null;
    public void Purchase(){
        //Actual purchasing is in ShopSlot
        if(purchaseTimer==-4){
            purchases+=1;
            RepPlus(1);
            if(!purchased)purchased=true;
            //GameSession.instance.gameSpeed=0.05f;purchaseTimer=0.3f;foreach(Button bt in GetComponentsInChildren<Button>()){bt.interactable=false;}
        }
    }
        /*if(pco!=null&&purchaseTimer>0){GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=0;StopCoroutine(pco);pco=null;}
        if(pco==null){pco=PurchaseTimeI(0.02f);}if(pco!=null&&purchaseTimer<=0){StartCoroutine(pco);}
    }
    public IEnumerator PurchaseTimeI(float time){
        GameSession.instance.speedChanged=true;GameSession.instance.gameSpeed=0.05f;
        purchaseTimer=0.5f;
        yield return new WaitForSecondsRealtime(time);
        GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=0;
    }*/

    public void RepPlus(int amnt){
        if(repEnabled){
        reputation+=amnt;
        reputationSlot+=amnt;
        }
    }public void RepMinus(int amnt){
        if(repEnabled){
        reputation-=amnt;
        reputationSlot-=amnt;
        }
    }
}
