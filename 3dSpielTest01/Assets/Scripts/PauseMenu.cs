using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject SignText;
    public GameObject player;
    bool Signshow=false;
    public PlayerMovment script;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                
                
            }
            else
            {
                Pause();
            }
        }




        if (Input.GetKeyDown("r")&& script.signfront && !Signshow)
        {

            SignText.SetActive(true);
            Signshow = true;
            Debug.Log(SignText);
        }


        if (Input.GetKeyDown("x") && Signshow || !script.signfront)
        {
            SignText.SetActive(false);
            Signshow = false;
        }


    }

    public void Resume()
    {

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quiting Game....");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

 
}


    

