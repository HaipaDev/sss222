using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Shop : MonoBehaviour{
    public static Shop instance;
    public static bool shopOpen;
    public static bool shopOpened;
    [Header("Config")]
    [SerializeField]public spawnReqsType shopSpawnReqsType=spawnReqsType.score;
    [SerializeReference]public spawnReqs shopSpawnReqs;
    public GameObject shopMenuUI;
    public GameObject slotsContainer;
    [AssetsOnly]public GameObject slotPrefab;
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
    public float purchaseTimer=-4;
    public LootTableShop lootTable;
    public List<ShopSlot> currentSlotsList;

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
        CheckSpawnReqs();
        if(shopOpen==true){OpenShop();}
        if(Input.GetKeyDown(KeyCode.Escape)){Resume();}
        if(repEnabled)LevelRep();
        if(shopTimeMax!=-5&&shopOpened&&shopTimer>0){shopTimer-=Time.unscaledDeltaTime;}
        if(shopTimeMax!=-5&&shopTimer<=0&&shopTimer!=-4){Resume();}
        if(purchaseTimer>0){purchaseTimer-=Time.unscaledDeltaTime;}
        if(purchaseTimer<=0&&purchaseTimer!=-4){GameSession.instance.gameSpeed=0;foreach(Button bt in GetComponentsInChildren<Button>()){bt.interactable=true;}purchaseTimer=-4;}
        if(currentSlotsList.Count>0)if(currentSlotsList.FindAll(x=>x.limitCount>=x.limit).Count>lootTable.currentQueue.slotList.Count/2){NewQueue();}
    }
    void CheckSpawnReqs(){
        if(shopSpawnReqs!=GameRules.instance.shopSpawnReqs)shopSpawnReqs=GameRules.instance.shopSpawnReqs;
        if(shopSpawnReqsType!=GameRules.instance.shopSpawnReqsType)shopSpawnReqsType=GameRules.instance.shopSpawnReqsType;
        spawnReqs x=shopSpawnReqs;
        spawnReqsType xt=shopSpawnReqsType;
        spawnReqsMono.instance.CheckSpawns(x,xt,this,"CallOpenShop");
    }
    IEnumerator CallOpenShop(){
        //do{//Make it wait for your money to spawn lmao
        if(GameRules.instance.shopOn){//&&GameSession.instance.coins>0){
            if(GameRules.instance.shopCargoOn){Shop.instance.SpawnCargo();}
            else{Shop.shopOpen=true;
            /*foreach(Enemy enemy in FindObjectsOfType<Enemy>()){
                enemy.givePts=false;
                enemy.health=-1;
                enemy.Die();
            }*/
            GameSession.instance.gameSpeed=GameRules.instance.shopOpenGameSpeed;}
            GameSession.instance.RandomizeShopScoreMax();
        }/*}while(GameSession.instance.coins<=0);*/yield return null;
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
        //RandomizeShop();
        shopOpen=false;
        GameSession.instance.gameSpeed=GameRules.instance.shopOpenGameSpeed;
        shopOpened=true;
        shopTimer=shopTimeMax;
    }
    public void Resume(){
        shopTimer=-4;
        if(purchased==false&&subbed==false){purchasedNotTimes++;}
        subbed=false;
        if(purchasedNotTimes==2){RepChange(1,false);purchasedNotTimes=0;}
        shopMenuUI.SetActive(false);
        shopOpened=false;
        GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=1f;
    }

    [Button("NewQueue")][ContextMenu("NewQueue")]public void NewQueue(){StartCoroutine(NewQueueI());}
    IEnumerator NewQueueI(){
        ClearSlots();
        yield return new WaitForSecondsRealtime(0.1f);
        lootTable.currentQueue=lootTable.GetQueue();
        for(var i=0;i<lootTable.currentQueue.preUnlocked;i++){CreateSlot();}
    }
    public void ClearSlots(){
        var slotsCount=slotsContainer.transform.childCount;
        for(var i=0;i<slotsCount;i++){DestroyImmediate(slotsContainer.transform.GetChild(0).gameObject);}//Debug.Log(i+"/Count: "+slotsCount);}
        reputationSlot=0;currentSlotID=0;currentSlotsList.Clear();
    }
    public void CreateSlot(){
        if(currentSlotID<lootTable.currentQueue.slotList.Count){
            var go=Instantiate(slotPrefab,slotsContainer.transform);
            var slot=go.GetComponent<ShopSlot>();
            currentSlotsList.Add(slot);
            slot.SetItem(lootTable.currentQueue.GetItem(currentSlotID));
            slot.SetPrice(lootTable.currentQueue.GetPrice(currentSlotID));
            slot.SetLimit(lootTable.currentQueue.GetLimit(currentSlotID));
            slot.SetRep(lootTable.currentQueue.GetRep(currentSlotID));
            currentSlotID++;
        }else{Debug.LogWarning("ShopSlot limit");}
        
    }
    public void CreateSlotDelay(){StartCoroutine(CreateSlotDelayI());}
    IEnumerator CreateSlotDelayI(){yield return new WaitForSecondsRealtime(0.1f);CreateSlot();}
    void LevelRep(){
        if(GetComponentInChildren<XPBars>()!=null){
        var bar=GetComponentInChildren<XPBars>();
        for(var i=currentSlotID;i<lootTable.currentQueue.slotList.Count;i++){if(reputationSlot==lootTable.currentQueue.GetSlot(currentSlotID).slotUnlock){/*var xp=slotUnlock[i+1];if(xp<IDmax)bar.ID=xp;if(bar.current==null){bar.Recreate();}*/
        CreateSlot();reputationSlot=reputationSlot-lootTable.currentQueue.GetSlot(currentSlotID).slotUnlock;if(reputationSlot<0){reputationSlot=0;}//reputationSlot=0;
        }}}
    }

    IEnumerator pco=null;
    public void Purchase(){
        //Actual purchasing is in ShopSlot
        if(purchaseTimer==-4){
            purchases+=1;
            //RepPlus(1);
            if(!purchased)purchased=true;
            if(GameSession.instance.steamAchievsStatsLeaderboards){Steamworks.SteamUserStats.AddStat("trade",1);Steamworks.SteamUserStats.StoreStats();}
            //GameSession.instance.gameSpeed=0.05f;purchaseTimer=0.3f;foreach(Button bt in GetComponentsInChildren<Button>()){bt.interactable=false;}
        }
    //}
        if(pco!=null&&purchaseTimer>0){GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=0;StopCoroutine(pco);pco=null;}
        if(pco==null){pco=PurchaseTimeI(0.02f);}if(pco!=null&&purchaseTimer<=0){StartCoroutine(pco);}
    }
    public IEnumerator PurchaseTimeI(float time){
        GameSession.instance.speedChanged=true;GameSession.instance.gameSpeed=1f;
        foreach(Button bt in GetComponentsInChildren<Button>()){bt.interactable=false;}
        purchaseTimer=time;
        yield return new WaitForSecondsRealtime(time);
        GameSession.instance.speedChanged=false;GameSession.instance.gameSpeed=0;
        pco=null;
    }

    public void RepChange(int amnt,bool add=true){
    if(repEnabled){
        if(add){
            reputation+=amnt;
            reputationSlot+=amnt;
        }else{
            reputation-=amnt;
            reputationSlot-=amnt;
        }
    }}

    [Button("VaildateShopSpawnReqs")][ContextMenu("VaildateShopSpawnReqs")]void VaildateShopSpawnReqs(){spawnReqsMono.Validate(ref shopSpawnReqs, ref shopSpawnReqsType);}
}
