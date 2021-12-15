using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    static UIManager uiManagerInstance;

    public TextMeshProUGUI orbText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI gameoverText;



    private void Awake()
    {
        if (uiManagerInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        uiManagerInstance = this;
        DontDestroyOnLoad(this);
    }
    
    public static void UpdateOrbUI(int orbCount)
    {
        uiManagerInstance.orbText.text = orbCount.ToString();
    }

    public static void UpdateLifeUI(int lifeCount)
    {
        uiManagerInstance.deathText.text = lifeCount.ToString();
    }

    public static void UpdateEnoughLifeUI()
    {
        uiManagerInstance.gameoverText.text = "You have run out of Life!" + "\n" + "Gain more life in finishing reality tasks!";
        uiManagerInstance.gameoverText.enabled = true;
        uiManagerInstance.Invoke("CancelEnoughLifeUI", 5f);
    }

    public static void UpdateTimeUI(float time)
    {
        int min = (int)(time / 60);
        float sec = time % 60;
        uiManagerInstance.timeText.text = min.ToString("00") + ":" + sec.ToString("00");
    }

    public static void DisplayGameOver()
    {
        uiManagerInstance.gameoverText.text = "Congratulations! Level Pass!";
        uiManagerInstance.gameoverText.enabled = true;
        uiManagerInstance.Invoke("CancelGameOver", 5f);
    }

    private void CancelEnoughLifeUI()
    {
        uiManagerInstance.gameoverText.enabled = false;
    }

    private void CancelGameOver()
    {
        uiManagerInstance.gameoverText.enabled = false;
    }
   
}
