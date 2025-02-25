using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ball, startButton, highScoreText, scoreText, quitButton, restartButton;
    [SerializeField] private Rigidbody2D[] leftFlippers;   // Array for all left flippers
    [SerializeField] private Rigidbody2D[] rightFlippers;  // Array for all right flippers
    [SerializeField] private Vector3 startPos;

    [SerializeField] private AudioSource backgroundMusicSource; // Main background music
    [SerializeField] private AudioSource gameOverMusicSource;     // Game Over music
    [SerializeField] private AudioSource buttonClickSource;       // Button click sound source

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip gameOverMusic;
    [SerializeField] private AudioClip buttonClickSound;

    private int score, highScore;
    public int multiplier;
    private bool canPlay;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Time.timeScale = 1;
        score = 0;
        multiplier = 1;
        highScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        highScoreText.GetComponent<Text>().text = "HighScore : " + highScore;
        canPlay = false;

        PlayMusic(menuMusic); // Start with menu music
    }

    private void Update()
    {
        if (!canPlay) return;

        // Flipper Controls (Existing Code)
        if (Input.GetKey(KeyCode.A))
        {
            foreach (Rigidbody2D flipper in leftFlippers)
            {
                flipper.AddTorque(25f);
            }
        }
        else
        {
            foreach (Rigidbody2D flipper in leftFlippers)
            {
                flipper.AddTorque(-20f);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            foreach (Rigidbody2D flipper in rightFlippers)
            {
                flipper.AddTorque(-25f);
            }
        }
        else
        {
            foreach (Rigidbody2D flipper in rightFlippers)
            {
                flipper.AddTorque(20f);
            }
        }
    }

    public void UpdateScore(int point, int mullIncrease)
    {
        multiplier += mullIncrease;
        score += point * multiplier;
        scoreText.GetComponent<Text>().text = "Score : " + score;
    }

    public void GameEnd()
    {
        Time.timeScale = 0;
        highScoreText.SetActive(true);
        quitButton.SetActive(true);
        restartButton.SetActive(true);

        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScore = score;
        }
        highScoreText.GetComponent<Text>().text = "HighScore : " + highScore;

        PlayGameOverMusic(); // Stop main music and play game over music
    }

    public void GameStart()
    {
        PlayButtonClickSound();
        highScoreText.SetActive(false);
        startButton.SetActive(false);
        scoreText.SetActive(true);
        Instantiate(ball, startPos, Quaternion.identity);
        canPlay = true;

        PlayMusic(gameplayMusic); // Switch to gameplay music
    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void GameRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void PlayMusic(AudioClip clip)
    {
        if (backgroundMusicSource != null && clip != null)
        {
            backgroundMusicSource.Stop(); // Stop current music
            backgroundMusicSource.clip = clip;
            backgroundMusicSource.loop = true;
            backgroundMusicSource.Play();
        }
    }

    private void PlayGameOverMusic()
    {
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.Stop(); // Stop gameplay music
        }

        if (gameOverMusicSource != null && gameOverMusic != null)
        {
            gameOverMusicSource.clip = gameOverMusic;
            gameOverMusicSource.loop = true;
            gameOverMusicSource.Play();
        }
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSource != null && buttonClickSound != null)
        {
            buttonClickSource.PlayOneShot(buttonClickSound);
        }
    }
}
