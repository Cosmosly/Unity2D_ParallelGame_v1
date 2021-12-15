using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager gameManagerInstance;

    SceneFader fader;

    List<Orb> orbs;

    Door lockedDoor;

    int lifeTimes;

    float gameTime;

    bool gameisOver;
   

    private void Awake()
    {
        if (gameManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        gameManagerInstance = this;

        orbs = new List<Orb>();

        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        if (gameisOver)
            return;


        UIManager.UpdateLifeUI(gameManagerInstance.lifeTimes);
        gameTime += Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);
        UIManager.UpdateOrbUI(gameManagerInstance.orbs.Count);
    }

   

    public static void LifeCount()
    {
        // Update Life times
        UIManager.UpdateLifeUI(gameManagerInstance.lifeTimes);
    }

    public static void RegisterDoor(Door door)
    {
        gameManagerInstance.lockedDoor = door;
    }
    public static void RegisterSceneFader(SceneFader sf)
    {
        gameManagerInstance.fader = sf;

    }
    
    public static void RegisterOrb(Orb orb)
    {
        if (!gameManagerInstance.orbs.Contains(orb))
            gameManagerInstance.orbs.Add(orb);

        // [update orb number at the beginning]
        UIManager.UpdateOrbUI(gameManagerInstance.orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if (!gameManagerInstance.orbs.Contains(orb))
            return;
        // [grab a orb, remove the orb from the orbs list]
        gameManagerInstance.orbs.Remove(orb);

        // [when list orbs has nothing in it, open the door]
        if (gameManagerInstance.orbs.Count == 0)
            gameManagerInstance.lockedDoor.OpenDoor();

        // [Update orb number]
        UIManager.UpdateOrbUI(gameManagerInstance.orbs.Count);


    }

    // Restart the scene when player died
    public static void PlayerDied()
    {
        // [FadeOut effect]
        gameManagerInstance.fader.FadeOut();
        // [Death number -1]
        // Update the death UI
        if(--gameManagerInstance.lifeTimes >= 0)
            UIManager.UpdateLifeUI(gameManagerInstance.lifeTimes);

        // if enough life to play
        NeedMoreLife();

        

        // [Reload Scene after 1.5sec]
        gameManagerInstance.Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        // [Clear orbs list to 0]
        gameManagerInstance.orbs.Clear();

        // [Reload Scene]
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void NeedMoreLife()
    {
        while(gameManagerInstance.lifeTimes < 0){
            // clear orbs
            gameManagerInstance.orbs.Clear();
            UIManager.UpdateEnoughLifeUI();
            gameManagerInstance.Invoke("BackToMainMenu", 5f);
            return;
        }
    }

    public static void PlayerWon()
    {
        gameManagerInstance.gameisOver = true;
        UIManager.DisplayGameOver();
        AudioManager.PlayerWonAudio();
        
    }

    public static bool GameOver()
    {
        return gameManagerInstance.gameisOver;
    }

    private void BackToMainMenu()
    {
        gameManagerInstance.orbs.Clear();
        gameManagerInstance.gameisOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    public static void PassingGameLife(int _gameLifes)
    {
        gameManagerInstance.lifeTimes += _gameLifes;
    }

    public static int ReturenGameLife()
    {
        return gameManagerInstance.lifeTimes;
    }
    
}
