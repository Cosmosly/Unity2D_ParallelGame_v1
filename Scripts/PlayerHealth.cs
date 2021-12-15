using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public GameObject deathVFXPrefab;
    public GameObject deathVFXPrefab2;
    int trapsLayer;

    void Start()
    {
        trapsLayer = LayerMask.NameToLayer("Traps");
    }


    // While player hit the any collider(TrapsLayer) can make them died
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == trapsLayer)
        {
            // cloud effect
            Instantiate(deathVFXPrefab, transform.position, transform.rotation);
            //
            Instantiate(deathVFXPrefab2, transform.position, Quaternion.Euler(0,0,Random.Range(-120,120)));

            // set gameObject inactive
            gameObject.SetActive(false);

            // play death audio
            AudioManager.PlayDeathAudio();

            // reload the scene
            GameManager.PlayerDied();
           
        }
    }
    


}
