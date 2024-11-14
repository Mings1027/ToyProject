using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ScreenController : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem = EventSystem.current;
    [SerializeField] PlayerController playerController;
    [SerializeField] GameController gameController;

    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject optionScreen;
    [SerializeField] GameObject controllScreen;
    [SerializeField] GameObject youDiedScreen;

    [SerializeField] GameObject optionButton;
    [SerializeField] GameObject controllButton;
    [SerializeField] GameObject optionCloseButton;
    [SerializeField] GameObject controllCloseButton;

    [SerializeField] string mainMenu, gameScreen;

    public bool pause, openOption, openControll;


    private void Awake()
    {
        playerController.gameStart = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openOption) CloseOptions();
            else if (openControll) CloseControll();
            else
            {
                if (pause) Resume();
                else Pause();
            }
        }
        if (playerController.isDead) youDiedScreen.SetActive(true);

    }

    private void Pause()
    {
        playerController.gameStart = false;
        pause = true;
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(optionButton);
    }
    private void Resume()
    {
        playerController.gameStart = true;
        pause = false;
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(null);
    }
    public void OpenOptions()
    {
        openOption = true;
        optionScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(optionCloseButton);
    }
    public void CloseOptions()
    {
        openOption = false;
        optionScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(optionButton);
    }
    public void OpenControll()
    {
        openControll = true;
        controllScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(controllCloseButton);
    }
    public void CloseControll()
    {
        openControll = false;
        controllScreen.SetActive(false);
        eventSystem.SetSelectedGameObject(controllButton);
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);
    }
    public void CloseYouDiedScreen()
    {
        SceneManager.LoadScene(gameScreen);
    }
}
