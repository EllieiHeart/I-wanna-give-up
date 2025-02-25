using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ball, startButton, highScoreText, scoreText, quitButton, restartButton;
    [SerializeField] private Rigidbody2D left, right;
    [SerializeField] private Vector3 startPos;

    [SerializeField] private AudioSource backgroundMusicSource; // Main background music
    [SerializeField] private AudioSource gameOverMusicSource;  // Game Over music

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private AudioClip gameOverMusic;

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

        if (Input.GetKey(KeyCode.A))
        {
            left.AddTorque(25f);
        }
        else
        {
            left.AddTorque(-20f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            right.AddTorque(-25f);
        }
        else
        {
            right.AddTorque(20f);
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
}
