using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour{
    [HideInInspector]public int ID;
    [HideInInspector]public int price;
    public GameObject slotText;
    Shop shopMenu;
    LootTableShop2 lootTable;
    Player player;
    GameSession gameSession;
    GameObject shopVFX;
    UpgradeMenu umenu;
    ShopSlotID item;
    void Start()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (shopMenu == null) shopMenu = FindObjectOfType<Shop>();
        if (lootTable == null) lootTable = Shop.instance.lootTable;
        if (gameSession == null) gameSession = FindObjectOfType<GameSession>();
        shopVFX=GameAssets.instance.GetVFX("ShopVFX");
    }

    void Update()
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (shopMenu == null) shopMenu = FindObjectOfType<Shop>();
        if (lootTable == null) lootTable = Shop.instance.lootTable;
        if (gameSession == null) gameSession = FindObjectOfType<GameSession>();
        if (umenu == null) umenu = FindObjectOfType<UpgradeMenu>();
    }
    public void ResetState()
    {
        //GetComponent<Image>().sprite = shopMenu.shopSlotIDs[ID].sprite;
        //slotText.GetComponent<TMPro.TextMeshProUGUI>().text = shopMenu.shopSlotIDs[ID].price.ToString();
        if(ID!=-1)item=(ShopSlotID)lootTable.itemList[ID];
        GetComponent<Image>().sprite = item.sprite;
        //transform.localScale = new Vector3(lootTable.itemList[ID].lootItem.scaleX,lootTable.itemList[ID].lootItem.scaleY,1);
        var rt=GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(300*item.scaleX, 300*item.scaleY);
        if(item.price!=-1){price=item.price;}
        else{price=Random.Range(item.priceS,item.priceE);}
        slotText.GetComponent<TMPro.TextMeshProUGUI>().text = price.ToString();
    }
    public void Buy(){
        //if (gameSession.coins >= shopMenu.shopSlotIDs[ID].price)
        if (gameSession.coins >= price){
            //gameSession.coins -= shopMenu.shopSlotIDs[ID].price;
            gameSession.gameSpeed=0.05f;
            //var price = item.lootItem.price;
            gameSession.coins-=price;

            shopVFX=GameAssets.instance.GetVFX("ShopVFX");
            var pos=transform;
            foreach(Transform child in transform){if(GetComponent<TMPro.TextMeshProUGUI>()!=null){pos=child.transform;}}
            var pt=Instantiate(shopVFX, pos);
            var ps=pt.GetComponent<ParticleSystem>();
            var pm=ps.main;pm.maxParticles=price;
            //var pe=ps.emission;pe.rateOverTime=Mathf.RoundToInt(price);
            Destroy(pt,0.4f);
            //AudioSource.PlayClipAtPoint(shopMenu.buySFX,player.transform.position);
            AudioManager.instance.Play("Ding");
            //if(ID<=shopMenu.shopSlotIDs.Count){
                player.energy+=10f;
                if(ID!=10)gameSession.AddXP(gameSession.xp_shop);//XP For purchase
                shopMenu.Purchase();
                //if(ID!=4 && ID!=8 && ID!=9 && ID!=10){
                if(ID!=-1){
                if(item.pwrupName!=""){
                    player.powerup=item.pwrupName;
                    player.energy += player.pwrupEnergyGet;
                }else if(ID==4){
                    if (player.health>(player.maxHP-30)) { gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP-player.health)*2); }
                    player.health += player.medkitUHpAmnt;
                    player.energy += player.medkitUEnergyGet;
                }else if(ID==8){
                    player.SetStatus("magnet");
                }else if(ID==9){
                    Instantiate(GameAssets.instance.Get("RandomizerPwrup"),player.transform.position,Quaternion.identity);
                }else if(ID==10){
                    gameSession.cores++;
                    //AudioSource.PlayClipAtPoint(gameSession.lvlUpSFX,player.transform.position);
                    AudioManager.instance.Play("LvlUp");
                }else if(ID==11){
                    umenu.AddSpeedBypass();
                    //AudioSource.PlayClipAtPoint(gameSession.lvlUpSFX,player.transform.position);
                    AudioManager.instance.Play("LvlUp");
                }else if(ID==12){
                    player.health += 12;
                }else if(ID==13){
                    player.energy += 24;
                }
                }else{ResetState();}

                gameSession.gameSpeed=0f;
                /*if (ID == 0) { player.powerup = "mlaser"; player.energy += player.pwrupEnergyGet; }
                if (ID==1) { player.powerup = "phaser"; player.energy += player.pwrupEnergyGet; }
                if (ID==2) { player.powerup = "hrockets"; player.energy +=player.pwrupEnergyGet; }
                if (ID==3) { player.powerup = "laser3"; player.energy += player.pwrupEnergyGet; }
                if (ID==4) { if (player.health>(player.maxHP-30)) { gameSession.AddToScoreNoEV(Mathf.RoundToInt(player.maxHP-player.health)*2); } player.health += player.medkitUHpAmnt; player.energy += player.medkitUEnergyGet; }
                if (ID == 5) { player.powerup = "lsaber"; }*/
            //} else { Debug.LogError("Shop Slot ID out of bounds"); }
        }else{
            //shopMenu.GetComponent<AudioSource>().Play();//PlayClipAtPoint(player.noEnergySFX,transform.position,100);
            gameSession.gameSpeed=0.001f;
            //AudioSource.PlayClipAtPoint(shopMenu.noCoinsSFX,player.transform.position);
            AudioManager.instance.Play("Deny");
            gameSession.gameSpeed=0f;
        }
    }
}
