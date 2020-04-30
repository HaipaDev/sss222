using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    GameSession gameSession;
    //float prevGameSpeed;
    private void Awake()
    {
        gameSession.gameSpeed=1f;
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
        FindObjectOfType<GameSession>().SaveHighscore();
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene("Menu");
        gameSession.gameSpeed=1f;
        Time.timeScale = 1f;
        //FindObjectOfType<GameSession>().savableData.Save();
        //FindObjectOfType<SaveSerial>().Save();
    }
    public void LoadGameScene(){
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetScore();
        gameSession.gameSpeed=1f;
        Time.timeScale = 1f;
    }
    public void LoadGamrModeChooseScene(){SceneManager.LoadScene("GameModeChoose");}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadInventoryScene(){SceneManager.LoadScene("Inventory");}
    public void RestartGame(){
        FindObjectOfType<GameSession>().SaveHighscore();
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameSession.gameSpeed=1f;
        Time.timeScale = 1f;
    }public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameSession.gameSpeed=1f;
        Time.timeScale = 1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
}
