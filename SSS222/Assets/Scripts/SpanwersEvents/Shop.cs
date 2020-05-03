using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour{
    public static bool shopOpen;
    public static bool shopOpened;
    public GameObject shopMenuUI;
    public ShopSlot[] slots;
    //public GameObject[] slotText;
    public List<ShopSlotID> shopSlotIDs;
    public AudioClip noCoinsSFX;
    public AudioClip buySFX;
    //public List<int> prices;
    //int maxID;
    //public List<Sprite> sprites;
    public float sum;

    LootTableShop lootTable;
    //Player player;
    private void Awake()
    {
        //foreach (ShopSlotID shopSlotID in shopSlotIDs){sum += shopSlotID.spawnRate;}
    }
    void Start(){
        lootTable=GetComponent<LootTableShop>();
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
        //if (player == null) player = FindObjectOfType<Player>();
    }
    public void OpenShop(){
        shopMenuUI.SetActive(true);
        GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        RandomizeShop();
        shopOpen = false;
        shopOpened=true;
    }
    public void Resume()
    {
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
