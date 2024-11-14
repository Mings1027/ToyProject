using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public string gameScreen;

    public GameObject optionScreen;

    public EventSystem eventSystem = EventSystem.current;
    public GameObject startButton;
    public GameObject optionCloseButton;
    public GameObject optionButton;

    public bool openOption;

    private void Start()
    {
        eventSystem.SetSelectedGameObject(startButton);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (openOption) CloseOptions();
        }
    }

    public void StartGame() => SceneManager.LoadScene(gameScreen);
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
    public void QuitGame()
    {
        Application.Quit();
        //Debug.Log("Quitting");
    }
}
