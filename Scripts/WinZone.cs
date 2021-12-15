using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{

    int playerLayer;
  

    void Start()
    {
        playerLayer = LayerMask.NameToLayer("Player");   
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            GameManager.PlayerWon();
            Invoke("BackToMainMenu", 3f);
        }
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }


}
