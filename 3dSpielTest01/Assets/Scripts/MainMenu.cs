using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenu : MonoBehaviour
{
    public Animator transition;
    private IEnumerator coroutine;

    public void QuitGame()
    {
        Debug.Log("Quiting..");
        Application.Quit();
    }

    public void PlayGame()
    {

        coroutine = Animation();
        StartCoroutine(coroutine);


    }
    private IEnumerator Animation()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
