using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PouseMenue : MonoBehaviour
{

    public static bool isGamePoused = false;

    public GameObject pauseMenuUI;
    public GameObject playerUI;

    private void Start()
    {
        playerUI.SetActive(true);
        pauseMenuUI.SetActive(false);

        isGamePoused = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (isGamePoused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        playerUI.SetActive(true);
        //Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;

        isGamePoused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        playerUI.SetActive(false);
        //Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isGamePoused = true;
    }

    public void LoadMenu()
    {
        //Time.timeScale = 1f;
        GameSetuoController.GSC.DisconnetPlayer();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);
    }

    public void QuitGame()
    {
        GameSetuoController.GSC.DisconnetPlayer();
        Application.Quit();
    }
}