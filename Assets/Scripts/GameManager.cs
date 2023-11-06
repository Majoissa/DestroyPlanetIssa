using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lives = 3;
    public int score = 0;
    public GameObject planet;
    public TextMeshProUGUI points;
    public TextMeshProUGUI lifeRemaining;
    public AudioClip explosionClip;
    public AudioClip bombDesactivateClip;
    public AudioClip gameOverSound;
    public AudioClip winSound;
    public GameObject gameOverPanel;

    public MainMenuManager menuManager;
    public TextMeshProUGUI scoreText;
    public int bestScore;
    public TextMeshProUGUI wellDoneText; 
    private bool hasPlayedWinSound = false;




    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        score = 0;
        points.text = "Score: " + score;
        lifeRemaining.text = "Lives: " + lives;
         wellDoneText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddScore()
    {
        score++;
        points.text = "Score: " + score;
        UpdateBestScore();
    }
    public void TakeDamage()
    {
        lives--;
        lifeRemaining.text = "Lives: " + lives;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void PlayExplosionSound()
    {
        GetComponent<AudioSource>().PlayOneShot(explosionClip);
    }

    public void PlayBombDesactivateSound()
    {
        GetComponent<AudioSource>().PlayOneShot(bombDesactivateClip);
    }

    public void PlayGameOverSound()
    {
        GetComponent<AudioSource>().PlayOneShot(gameOverSound);
    }

    public void PlayWinSound()
    {
        GetComponent<AudioSource>().PlayOneShot(winSound);
    }
    public void ResetGame()
    {
        // Restablece el juego a su estado inicial
        lives = 3;
        score = 0;
        points.text = "Score: " + score;
        lifeRemaining.text = "Lives: " + lives;
         hasPlayedWinSound = false;

        // Aquí puedes agregar cualquier otra lógica de reinicio que necesites
    }

    public void UpdateBestScore()
{
    bestScore = PlayerPrefs.GetInt("BestScore", 0);

    if (score > bestScore)
    {
        PlayerPrefs.SetInt("BestScore", score);
        PlayerPrefs.Save();

        // Activa el texto de felicitación
        wellDoneText.gameObject.SetActive(true); // Asegúrate de que este TextMeshProUGUI está en tu escena y asignado
        if (!hasPlayedWinSound)
        {
            PlayWinSound();
            hasPlayedWinSound = true; // Establece la bandera para evitar reproducciones futuras
        }
    }
    else
    {
        // Desactiva el texto si el score no es mayor que bestScore
        wellDoneText.gameObject.SetActive(false);
        hasPlayedWinSound = false;
    }
}

    /*
    public void AddScore(int pointsToAdd)
    {
        score += pointsToAdd;
        points.text = "Score: " + score;
        UpdateBestScore();
    }
*/
    private void GameOver()
    {
        // Mostrar el panel de Game Over
        gameOverPanel.SetActive(true);
        menuManager.gameUI.SetActive(false);

        scoreText.text = "Score: " + score;
        PlayGameOverSound();
    }

    // En algún lugar dentro de GameManager
    public void Retry()
    {
        // Opcionalmente, actualiza el mejor puntaje antes de reiniciar
        ResetGame();
        lives = 3;
        score = 0;
        points.text = "Score: " + score;
        menuManager.mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        menuManager.gameUI.SetActive(true);
         wellDoneText.gameObject.SetActive(false);

    }

    public void ReturnToMenu()
    {
        // Opcionalmente, actualiza el mejor puntaje antes de ir al menú

        gameOverPanel.SetActive(false);
        menuManager.gameUI.SetActive(false);
        menuManager.mainMenuPanel.SetActive(true);
        UpdateBestScore();
        menuManager.ShowBestScore();
        menuManager.resetBestScoreButton.SetActive(false);
        
    }
}

