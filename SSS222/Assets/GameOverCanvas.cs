using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour{
    public static GameOverCanvas instance;
    public bool gameOver;
    private void Awake() {
        instance=this;
    }
    public void OpenGameOverCanvas(bool open){
        gameOver=open;
        transform.GetChild(0).gameObject.SetActive(open);
    }
}
