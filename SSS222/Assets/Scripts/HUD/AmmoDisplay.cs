using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDisplay : MonoBehaviour{
    TMPro.TextMeshProUGUI scoreText;
    Player player;

    // Start is called before the first frame update
    void Start(){
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update(){
        scoreText.text = player.GetEnergy().ToString();
    }
}
