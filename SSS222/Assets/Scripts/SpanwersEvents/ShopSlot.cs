using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour{
    public int ID;
    public GameObject slotText;
    public Shop shopMenu;
    public LootTableShop lootTable;
    Player player;
    GameSession gameSession;
    void Start()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (shopMenu == null) shopMenu = FindObjectOfType<Shop>();
        if (lootTable == null) lootTable = FindObjectOfType<LootTableShop>();
        if (gameSession == null) gameSession = FindObjectOfType<GameSession>();
    }

    void Update()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (shopMenu == null) shopMenu = FindObjectOfType<Shop>();
        if (lootTable == null) lootTable = FindObjectOfType<LootTableShop>();
        if (gameSession == null) gameSession = FindObjectOfType<GameSession>();
    }
    public void ResetState()
    {
        //GetComponent<Image>().sprite = shopMenu.shopSlotIDs[ID].sprite;
        //slotText.GetComponent<TMPro.TextMeshProUGUI>().text = shopMenu.shopSlotIDs[ID].price.ToString();
        GetComponent<Image>().sprite = lootTable.itemList[ID].lootItem.sprite;
        slotText.GetComponent<TMPro.TextMeshProUGUI>().text = lootTable.itemList[ID].lootItem.price.ToString();
    }
    public void Buy()
    {
        //if (gameSession.coins >= shopMenu.shopSlotIDs[ID].price)
        if (gameSession.coins >= lootTable.itemList[ID].lootItem.price)
        {
            //gameSession.coins -= shopMenu.shopSlotIDs[ID].price;
            gameSession.coins -= lootTable.itemList[ID].lootItem.price;
            AudioSource.PlayClipAtPoint(shopMenu.buySFX,player.transform.position);
            //if(ID<=shopMenu.shopSlotIDs.Count){
                player.energy+=10f;
                if(ID!=4 && ID!=8 && ID!=9){
                    player.powerup=lootTable.itemList[ID].lootItem.pwrupName;
                    player.energy += player.pwrupEnergyGet;
                }else if(ID==4){
                    if (player.health>(player.maxHP-30)) { gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP-player.health)*2); }
                    player.health += player.medkitUHpAmnt;
                    player.energy += player.medkitUEnergyGet;
                }else if(ID==8){
                    player.magnetized=true;
                    player.magnetTimer=player.magnetTime;
                }else if(ID==9){
                    Instantiate(player.GetComponent<PlayerCollider>().GetRandomizerPwrup(),player.transform.position,Quaternion.identity);
                }
                /*if (ID == 0) { player.powerup = "mlaser"; player.energy += player.pwrupEnergyGet; }
                if (ID==1) { player.powerup = "phaser"; player.energy += player.pwrupEnergyGet; }
                if (ID==2) { player.powerup = "hrockets"; player.energy +=player.pwrupEnergyGet; }
                if (ID==3) { player.powerup = "laser3"; player.energy += player.pwrupEnergyGet; }
                if (ID==4) { if (player.health>(player.maxHP-30)) { gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP-player.health)*2); } player.health += player.medkitUHpAmnt; player.energy += player.medkitUEnergyGet; }
                if (ID == 5) { player.powerup = "lsaber"; }*/
            //} else { Debug.LogError("Shop Slot ID out of bounds"); }
        }else{
            //shopMenu.GetComponent<AudioSource>().Play();//PlayClipAtPoint(player.noEnergySFX,transform.position,100);
            AudioSource.PlayClipAtPoint(shopMenu.noCoinsSFX,player.transform.position);
        }
    }
}
