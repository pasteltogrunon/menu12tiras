using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gamePaused = false;
    public bool gameFinished = false;
    public bool GameFinished
    {
        get => gameFinished;
        set => gameFinished = value;
    }

    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    public CameraManager cameraMan;
    public GameManager gameMan;
    public ObstacleGenerator obsGen;
    public Player player;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameFinished) GoToMenu();
            else if (gamePaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        gamePaused = false;
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        gamePaused = true;
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoToMenu()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(0);
    }

    public void GameOver()
    {
        gameFinished = true;

        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        gameOverUI.SetActive(false);
        /*
        cameraMan.Start();
        gameMan.Restart();
        obsGen.Restart();
        player.Start();
        */
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync(1);
    }
}
