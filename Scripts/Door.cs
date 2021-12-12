using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    int openID;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        openID = Animator.StringToHash("Open");

        // register door
        GameManager.RegisterDoor(this);
    }

    public void OpenDoor()
    {
        anim.SetTrigger(openID);

        // play audio
        AudioManager.PlayDoorOpenAudio();
    }
    
}
