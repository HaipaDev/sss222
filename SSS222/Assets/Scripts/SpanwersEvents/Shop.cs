using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{
    public static Shop instance;
    public static bool shopOpen;
    public static bool shopOpened;
    public GameObject shopMenuUI;
    public GameObject[] slotsChoose;
    public int currentSlotID;
    public GameObject currentSlot;
    public ShopSlot[] slots;
    public GameObject cargoPrefab;
    //public GameObject[] slotText;
    //public List<ShopSlotID> shopSlotIDs;
    //public AudioClip noCoinsSFX;
    //public AudioClip buySFX;
    //public List<int> prices;
    //int maxID;
    //public List<Sprite> sprites;
    public int purchases;
    public bool purchased;
    public int purchasedNotTimes;
    //public int purchasesCurrent;
    public int reputation;
    public int reputationSlot;
    public float sum;

    LootTableShop lootTable;
    //Player player;
    private void Awake()
    {
        //foreach (ShopSlotID shopSlotID in shopSlotIDs){sum += shopSlotID.spawnRate;}
        instance=this;
    }
    void Start(){
        lootTable=GetComponent<LootTableShop>();
        SetSlots();
        //maxID = shopSlotIDs.Count;
        /*slot1 = slotObj[0].GetComponent<ShopSlot>();
        slot2 = slotObj[1].GetComponent<ShopSlot>();
        slot3 = slotObj[2].GetComponent<ShopSlot>();
        slot4 = slotObj[3].GetComponent<ShopSlot>();*/
        //player = FindObjectOfType<Player>();
    }

    void Update(){
        if (shopOpen == true) { OpenShop(); }
        if (Input.GetKeyDown(KeyCode.Escape)){Resume();}
        SetSlots();
        //if (player == null) player = FindObjectOfType<Player>();
        LevelRep();
    }
    public void SpawnCargo(){
        float xx=3.45f;
        if(UnityEngine.Random.value<0.5f){xx=-3.45f;}//else{xx=3.45f;}
        //xx=Random.Range(-3.45f,3.45f);
        GameAssets.instance.Make("CargoShip",new Vector2(xx,7.4f));
    }
    public void OpenShop(){
        purchased=false;
        //purchasesCurrent=purchases;
        shopMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        RandomizeShop();
        shopOpen = false;
        FindObjectOfType<GameSession>().gameSpeed=0;
        shopOpened=true;
    }
    public void Resume(){
        //if(purchasesCurrent==purchases){purchasedNotTimes++;}
        if(purchased==false){purchasedNotTimes++;}
        if(purchasedNotTimes==2){RepMinus(2);purchasedNotTimes=0;}
        shopMenuUI.SetActive(false);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        shopOpened=false;
        FindObjectOfType<GameSession>().gameSpeed = 1f;
    }
    public void RandomizeShop(){
        foreach(ShopSlot slot in slots)
        {
            slot.ID = lootTable.GetItem().ID;//GetRandomID().ID;
            slot.ResetState();
        }
        /*slotObj[1].GetComponent<Image>().sprite = sprites[slot2.ID];
        slotObj[2].GetComponent<Image>().sprite = sprites[slot3.ID];
        slotObj[3].GetComponent<Image>().sprite = sprites[slot4.ID];
        slotText[1].GetComponent<TMPro.TextMeshProUGUI>().text = prices[slot2.ID].ToString();
        slotText[2].GetComponent<TMPro.TextMeshProUGUI>().text = prices[slot3.ID].ToString();
        slotText[3].GetComponent<TMPro.TextMeshProUGUI>().text = prices[slot4.ID].ToString();*/
    }
    public void RandomizeShopDelay(){
        StartCoroutine(RandomizeShopDelayI());
    }
    IEnumerator RandomizeShopDelayI(){
        yield return new WaitForSecondsRealtime(0.1f);
        RandomizeShop();
    }
    void SetSlots(){
        if(currentSlotID>0){currentSlot=slotsChoose[currentSlotID-1];
        if(slots!=currentSlot.GetComponentsInChildren<ShopSlot>()){
            slots=currentSlot.GetComponentsInChildren<ShopSlot>();
            if(currentSlotID>=5){var rt=shopMenuUI.GetComponent<RectTransform>();rt.sizeDelta=new Vector2(1082,1751);
            //shopMenuUI.transform.position=new Vector2(shopMenuUI.transform.position.x,10);
            }
            else{var rt=shopMenuUI.GetComponent<RectTransform>();rt.sizeDelta=new Vector2(1082,1251);
            //shopMenuUI.transform.position=new Vector2(shopMenuUI.transform.position.x,-60);
            }
        }
        }else{Debug.LogError("Shop slots ID is <=0");}
        foreach(GameObject slot in slotsChoose){if(slot!=currentSlot){slot.SetActive(false);}else{slot.SetActive(true);}}
    }
    void LevelRep(){
        var bar=GetComponentInChildren<XPBars>();
        if(currentSlotID==1&&reputationSlot==1){var xp=2;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}RandomizeShopDelay();reputationSlot=0;}
        if(currentSlotID==2&&reputationSlot==2){var xp=3;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}RandomizeShopDelay();reputationSlot=0;}
        if(currentSlotID==3&&reputationSlot==3){var xp=4;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}RandomizeShopDelay();reputationSlot=0;}
        if(currentSlotID==4&&reputationSlot==4){var xp=5;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}RandomizeShopDelay();reputationSlot=0;}
        if(currentSlotID==5&&reputationSlot==5){var xp=6;currentSlotID=xp;bar.ID=xp;bar.created=1;RandomizeShopDelay();reputationSlot=0;}

        if(currentSlotID==6&&reputationSlot<0&&shopOpened==true){var xp=5;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}reputationSlot=xp-FindObjectOfType<CargoShip>().repMinus;}
        if(currentSlotID==5&&reputationSlot<0&&shopOpened==true){var xp=4;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}reputationSlot=xp-FindObjectOfType<CargoShip>().repMinus;}
        if(currentSlotID==4&&reputationSlot<0&&shopOpened==true){var xp=3;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}reputationSlot=xp-FindObjectOfType<CargoShip>().repMinus;}
        if(currentSlotID==3&&reputationSlot<0&&shopOpened==true){var xp=2;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}reputationSlot=xp-FindObjectOfType<CargoShip>().repMinus;}
        if(currentSlotID==2&&reputationSlot<0&&shopOpened==true){var xp=1;currentSlotID=xp;bar.ID=xp;if(bar.current==null){bar.Recreate();}reputationSlot=xp-FindObjectOfType<CargoShip>().repMinus-1;}
    }
    public void Purchase(){
        purchases+=1;
        purchased=true;
        RepPlus(1);
    }
    public void RepPlus(int amnt){
        reputation+=amnt;
        reputationSlot+=amnt;
    }public void RepMinus(int amnt){
        reputation-=amnt;
        reputationSlot-=amnt;
    }
    /*public ShopSlotID GetRandomID(){
        //if (currentWave == null) return 0;
        //else{
            float randomWeight = 0;
            do
            {
                //No weight on any number?
                if (sum == 0) return null;
                randomWeight = Random.Range(0, sum);
            } while (randomWeight == sum);
            foreach (ShopSlotID shopSlotID in shopSlotIDs)
            {
                if (randomWeight < shopSlotID.spawnRate) return shopSlotID;
                randomWeight -= shopSlotID.spawnRate;
            }
            return null;
        //}
    }*/
}
