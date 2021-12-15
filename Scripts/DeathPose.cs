using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathPose : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
}
