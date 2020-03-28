using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    GameSession gameSession;
    // Start is called before the first frame update
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update(){
        
    }
    public void LoadStartMenu(){
        SceneManager.LoadScene("Menu");
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");
        gameSession.ResetScore();
    }
    public void LoadOptionsScene()
    {
        SceneManager.LoadScene("Options");
    }
    public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameSession.ResetScore();
    }
    public void QuitGame(){
        Application.Quit();
    }
}
