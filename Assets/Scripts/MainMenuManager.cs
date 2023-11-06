using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject mainMenuPanel; // Panel del menú principal
    public GameObject gameUI;
    public GameObject resetBestScoreButton; 

    //public AudioSource audioSource;
    //public AudioClip menuMusicClip;

    public TextMeshProUGUI bestScoreText;

    private void Start()
    {
        /* if (menuMusicClip != null)
         {
             audioSource.clip = menuMusicClip;
             audioSource.loop = true;
             audioSource.Play();
         }
 */
        GoToMainMenu();
    }

    public void PlayGame()
    {
        mainMenuPanel.SetActive(false); // Desactiva el menú principal
        gameManager.gameOverPanel.SetActive(false);
        gameUI.SetActive(true); // Activa la UI del juego

        // Restablece el estado del juego si es necesario
        gameManager.ResetGame(); // Asegúrate de que esta función exista y haga lo que necesitas

        // Opcional: Detener música del menú si la hay
        //audioSource.Stop();
    }

    public void GoToMainMenu()
    {
        // Configura la vista para mostrar el menú principal
        mainMenuPanel.SetActive(true); // Activa el menú principal
        gameUI.SetActive(false); // Desactiva la UI del juego
        gameManager.gameOverPanel.SetActive(false);
        ShowBestScore();

        // Opcional: Reproducir música de fondo para el menú
        /*if (menuMusicClip != null && !audioSource.isPlaying)
        {
            audioSource.clip = menuMusicClip;
            audioSource.loop = true;
            audioSource.Play();
        }*/
    }

    public void ShowBestScore()
    {
        gameManager.UpdateBestScore();
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        bestScoreText.text = "Best score: " + bestScore.ToString();
    }

     public void ResetBestScore()
    {
        // Resetea la mejor puntuación a 0
        PlayerPrefs.SetInt("BestScore", 0);
        PlayerPrefs.Save();

        // Actualiza la visualización de la mejor puntuación en la UI
        ShowBestScore();
    }
}
