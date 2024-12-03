using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;


public class GamePauseManager : MonoBehaviour
{
    public static bool isGamePaused = false; 
    public GameObject pauseMenuUI; 
    public TextMeshProUGUI tapToScreenText; 

    void Start()
    {
        
        pauseMenuUI.SetActive(false); ; 
        if (tapToScreenText != null)
        {
            tapToScreenText.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo); 
        }
    }

    void Update()
    {
       
        if (isGamePaused && Input.GetMouseButtonDown(0))
        {
            ResumeGame();
        }
    }

    
    public void OnPauseButtonPressed()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Oyunu devam ettir
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f; 
        isGamePaused = false; 
        AudioListener.pause = false; 
    }

    // Oyunu durdur
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); 
        Time.timeScale = 0f; 
        isGamePaused = true; 
        AudioListener.pause = true; 
    }

   
}
