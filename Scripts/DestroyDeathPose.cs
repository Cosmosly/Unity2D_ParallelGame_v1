using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDeathPose : MonoBehaviour
{

    private GameObject[] Ds;
    private void OnEnable()
    {
        Ds = GameObject.FindGameObjectsWithTag("DeathPose");
        foreach (GameObject D in Ds)
            Destroy(D);
    }
}
