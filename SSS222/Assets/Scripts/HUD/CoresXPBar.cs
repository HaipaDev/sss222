using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoresXPBar : MonoBehaviour{
    GameSession gameSession;
    float currentXp;
    float maxXp;
    // Start is called before the first frame update
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
        maxXp = gameSession.xp_forCore;
    }

    // Update is called once per frame
    void Update()
    {
        currentXp = gameSession.coresXp;
        maxXp = gameSession.xp_forCore;
        transform.localScale = new Vector2((currentXp / maxXp), 1);
    }
}

