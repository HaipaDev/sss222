using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeDestroy : MonoBehaviour{
    [SerializeField] bool reverse;
    [SerializeField] string gamemodeName;
    void Start(){
        //yield return new WaitForSecondsRealtime(Random.Range(0.1f,0.25f));//Prevent overload crash?
        if(!reverse&&GameSession.instance.CheckGamemodeSelected(gamemodeName))Destroy(gameObject);
        if(reverse&&!GameSession.instance.CheckGamemodeSelected(gamemodeName))Destroy(gameObject);
    }
}
