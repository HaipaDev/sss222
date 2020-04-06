using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    GameSession gameSession;
    //float prevGameSpeed;
    private void Awake()
    {
        Time.timeScale = 1f;
    }
    void Start(){
        gameSession = FindObjectOfType<GameSession>();
        //prevGameSpeed = gameSession.gameSpeed;
    }
    void Update()
    {
        gameSession = FindObjectOfType<GameSession>();
    }

    public void LoadStartMenu(){
        Time.timeScale = 1f;
        FindObjectOfType<GameSession>().SaveHighscore();
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene("Menu");
        //FindObjectOfType<GameSession>().savableData.Save();
        //FindObjectOfType<SaveSerial>().Save();
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetScore();
    }
    public void LoadGamrModeChooseScene(){SceneManager.LoadScene("GameModeChoose");}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadInventoryScene(){SceneManager.LoadScene("Inventory");}
    public void RestartGame(){
        Time.timeScale = 1f;
        FindObjectOfType<GameSession>().SaveHighscore();
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }public void RestartScene(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame(){
        Application.Quit();
    }
}
