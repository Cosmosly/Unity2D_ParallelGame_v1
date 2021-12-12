using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement movementScript;
    Rigidbody2D rb;

    int animator_isOnGround_ID;
    int animator_isCrouching_ID;
    int animator_isHanging_ID;
    int animator_isJumping_ID;
    int animator_verticalVelocity_ID;
    int animator_speed_ID;


    void Start()
    {
        // [Get component] In paraent
        anim = GetComponent<Animator>();
        movementScript = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        // [make Animator parameters to numeric, good for some platform]
        animator_speed_ID = Animator.StringToHash("speed");
        animator_isOnGround_ID = Animator.StringToHash("isOnGround");
        animator_isCrouching_ID = Animator.StringToHash("isCrouching");
        animator_isHanging_ID = Animator.StringToHash("isHanging");
        animator_isJumping_ID = Animator.StringToHash("isJumping");
        animator_verticalVelocity_ID = Animator.StringToHash("verticalVelocity");

    }

    
    void Update()
    {
        // [set Animator parameters]
        anim.SetFloat(animator_speed_ID, Mathf.Abs(movementScript.xVelocity));
        anim.SetBool(animator_isOnGround_ID, movementScript.isOnGround);
        anim.SetBool(animator_isCrouching_ID, movementScript.isCrouch);
        anim.SetBool(animator_isHanging_ID, movementScript.isHanging);
        anim.SetBool(animator_isJumping_ID, movementScript.isJump);
        anim.SetFloat(animator_verticalVelocity_ID, rb.velocity.y);

    }

    // audio while running
    public void StepAudio()
    {
        AudioManager.PlayFootstepAudio();
    }

    // audio while crouch and move
    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstepAudio();
    }
}
