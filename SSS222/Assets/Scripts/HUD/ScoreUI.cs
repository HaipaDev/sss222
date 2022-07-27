using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour{
    [SerializeField] Vector2 scale=Vector2.one;
    Vector2 scalePre=Vector2.one;
    float _delay;
    void Update(){
        if(_delay>0){_delay-=Time.unscaledDeltaTime;}
        if(_delay<=0){scalePre=transform.localScale;
            if(GameSession.instance!=null){
                if(GameSession.instance.gamemodeSelected<=0){Destroy(this);}
                else if(GameSession.instance.score>GameSession.instance.GetHighscoreCurrent().score){GetComponent<Animator>().SetTrigger("beaten");Destroy(this);}
            }
            transform.localScale=scale*scalePre;
        }
    }
}
