using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerLife : MonoBehaviour
{
    public PlayerMovment script;
    [SerializeField] Image Dashpoint;

    bool death = false;

    private void Update()
    {
       if(transform.position.y< -6f&&death==false)
        {
            Die();
        }

        
        if (script.candash&&!script.canjump)
        {
            Dashpoint.enabled = true;
            Debug.Log("Punkt");
        }

        else
        {
            Dashpoint.enabled = false;
            Debug.Log("Kein punkt");
        }
            

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBody"))
        {
            
            if (script.candestroy == true)
            {
                Destroy(collision.transform.gameObject);
                script.candash = true;
            }
            else
                Die();
        }
        
    }
    void Die()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<PlayerMovment>().enabled = false;
        death = true;
        Invoke(nameof(ReloadLevel), 1.3f);
    }
   
    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
