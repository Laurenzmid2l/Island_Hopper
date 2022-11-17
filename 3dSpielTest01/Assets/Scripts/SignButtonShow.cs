using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignButtonShow : MonoBehaviour
{


    public GameObject player;
    public GameObject SignButtonText;
    public PlayerMovment script;
    

    void Update()
    {
      

        if (script.signfront)
        {

            SignButtonText.SetActive(true);
            
        }
        else
        {
            SignButtonText.SetActive(false);
        }
    }
}
