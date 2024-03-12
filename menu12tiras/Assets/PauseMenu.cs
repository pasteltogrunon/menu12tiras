using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    public GameObject GameUI;
    public GameObject pauseMenuUI;
    public GameObject gameOverUI;

    public CameraManager cameraMan;
    public GameManager gameMan;
    public ObstacleGenerator obsGen;
    public Player player;

    public AudioMixer mixer;
    public AudioSource pauseMusic;
    public AudioSource pauseEnterSource;
    public AudioClip pauseEnter;
    public AudioClip pauseExit;

    public GameObject mobiusCamera;


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

        mixer.SetFloat("GameMusicVolume", 0);
        mixer.SetFloat("PauseMusicVolume", -80);

        pauseMusic.Stop();
        pauseEnterSource.PlayOneShot(pauseExit);


        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        gamePaused = true;

        mixer.SetFloat("GameMusicVolume", -80);
        mixer.SetFloat("PauseMusicVolume", 0);
        pauseMusic.Play();
        pauseEnterSource.PlayOneShot(pauseEnter);

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
        pauseMusic.Play();

        mixer.SetFloat("GameMusicVolume", -80);
        mixer.SetFloat("PauseMusicVolume", 0);

        player.gameObject.SetActive(false);
        GameUI.SetActive(false);
        mobiusCamera.SetActive(true);
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
