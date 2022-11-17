using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ItemCollector : MonoBehaviour
{
    int ncoins = 0;
    [SerializeField] TextMeshProUGUI CoinsText;
    
    

     
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            ncoins++;
            CoinsText.text = "Coins:" + ncoins;
        }
        
    }
}
