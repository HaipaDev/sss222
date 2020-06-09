using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour{
    Player player;
    float playerEnergy;
    float playerMaxEnergy;
    // Start is called before the first frame update
    void Start(){
        player = FindObjectOfType<Player>();
        playerMaxEnergy = player.GetComponent<Player>().maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if(player!=null){
        playerEnergy = player.GetComponent<Player>().energy;
        playerMaxEnergy = player.GetComponent<Player>().maxEnergy;
        transform.localScale = new Vector2(playerEnergy / playerMaxEnergy, 1);
        }
    }
}

