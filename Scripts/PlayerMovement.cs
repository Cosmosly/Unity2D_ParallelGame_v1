using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // [obtain the components]
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;               // player speed
    [SerializeField] private float crouchSpeedDivisor;  // division the speed when player crouch
    [SerializeField] private float jumpForce;           // basic jump force
    [SerializeField] private float jumpHoldForce;       // additional jump force while holding jump buttom
    [SerializeField] private float jumpHoldDuration;    // max hold jump time
    [SerializeField] private float crouchJumpBoost;     // addtional jump foce using crouch jump
    [SerializeField] private float jumpTime;            // hom much time holding jump buttom is working

    [Header("Movement States")]
    [SerializeField] private bool isCrouch;             // Wheather player is crouching or not
    [SerializeField] private bool isOnGround;           // Wheather player is on the ground
    [SerializeField] private bool isJump;               // Wheather player is jumping or not

    [Header("Movement Figure")]
    [SerializeField] private float xVelocity;               // determin the force from x axis
    [SerializeField] private Vector2 colliderStandSize;     // original box collider's size
    [SerializeField] private Vector2 colliderStandOffset;   // original box collider's offset
    [SerializeField] private Vector2 colliderCrouchSize;    // box collider's size when crouching(half original)
    [SerializeField] private Vector2 colliderCrouchOffset;  // box collider's offset when crouching(half original)

    [Header("Environment Detection")]
    public LayerMask groundLayer;

    [Header("Button Detection")]
    public bool jumpPressed;
    public bool jumpHeld;
    public bool crouchHeld;

    void Start()
    {

        // [Initialize movement parameters]
        InitializePlayerMovement(8f, 0.3f, 6.3f, 1.9f, 0.1f, 2.5f);
    }

   
    void Update()
    {
        // all relative with input button should place in the Update function not in the fixedUpdate
        // [Input button assignment]
        // press jump one time
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
        jumpHeld = Input.GetButton("Jump");            // held the jump button
        crouchHeld = Input.GetButton("Crouch");        // held the crouch button



    }

    private void FixedUpdate()
    {
        // [Checking player's physic state]
        PhysicsCheck();
        // [Controlling the movement on the ground]
        GroundMovement();
        // [Controlling the movement in the mid air]
        MidAirMovement();
        if (isJump)
            jumpPressed = false;
    }

    /*
     * Initialize the parameter
     */
    public void InitializePlayerMovement(float _speed, float _crouchSpeedDivisor, 
        float _jumpForce, float _jumpHoldForce, float _jumpHoldDuration, float _crouchJumpBoost)
    {
        // [Get component]
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        // [Speed parameters]
        speed = _speed;
        crouchSpeedDivisor = _crouchSpeedDivisor;

        // [Jump parameters]
        jumpForce = _jumpForce;
        jumpHoldForce = _jumpHoldForce;
        jumpHoldDuration = _jumpHoldDuration;
        crouchJumpBoost = _crouchJumpBoost;

        // [Crouch setting]
        // store the original stand state
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        // set up the crouch size
        colliderCrouchSize = new Vector2(colliderStandSize.x, colliderStandSize.y * 0.5f);
        colliderCrouchOffset = new Vector2(colliderStandOffset.x, colliderStandOffset.y * 0.5f);

        
    }


    /*
     * Using Physics to check player states
     */
     private void PhysicsCheck()
    {
        //[Check if player is on the ground]
        if (coll.IsTouchingLayers(groundLayer))
            isOnGround = true;
        else isOnGround = false;
    }


    /*
     * Control player when moving on the ground
     */
    private void GroundMovement()
    {
        // [Consider crouch-S]
        // if player is in the crouch state, execute crouch movement
        if (crouchHeld && !isCrouch && isOnGround)
            Crouch();
        // if player is in the crouch state and loose the s buttom, player stand up
        else if (!crouchHeld && isCrouch)
            StandUp();
        else if (!isOnGround && isCrouch)
            StandUp();

        // [Get input and set up velocity]
        xVelocity = Input.GetAxis("Horizontal");         // between -1 ~ 1 , default 0; Input.GetAxisRaw() -1,1,0 imediately stop
        // if player in the crouch state, the speed is decrease by the crouchSpeedDivisor
        if (isCrouch)
            xVelocity *= crouchSpeedDivisor;
        
        rb.velocity = new Vector2(xVelocity * speed, rb.velocity.y);

        // [Changing facing direction]
        FlipDirection();
    }


    /*
     *  Control the jumping movement
     */
     private void MidAirMovement()
    {
        // [jump single press]
        if(jumpPressed && isOnGround && !isJump)
        {
            if(isCrouch && isOnGround)
            {
                StandUp();
                rb.AddForce(new Vector2(0, crouchJumpBoost), ForceMode2D.Impulse);
            }
            // [change state]
            isOnGround = false;
            isJump = true;
            // [calculate jump time]
            jumpTime = Time.time + jumpHoldDuration;
            // [implment force]
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse); // force is the new vector, mode is impluse a suddent force
        }
        // [held jump button while jumping]
        else if (isJump)
        {
            if (jumpHeld)
                rb.AddForce(new Vector2(0, jumpHoldForce),ForceMode2D.Impulse);
            // when exceed jumpTime
            if (jumpTime < Time.time)
                isJump = false;
        }
    }


    /*
     * Change the player's facing direction when moving 
     */
     private void FlipDirection()
    {
        // if -> facing is oringinal, if <- facing opposite, if 0 do nothing
        if (xVelocity < 0)
            transform.localScale = new Vector2(-1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector2(1, 1);

    }


    /*
     * Manager the crouch movement
     */
    private void Crouch()
    {
        isCrouch = true;

        // [When crouching]
        // change size and offset
        coll.size = colliderCrouchSize;
        coll.offset = colliderCrouchOffset;
    }


    /*
     *  Manager stand up movment after crouch
     */
     private void StandUp()
    {
        isCrouch = false;

        // [When standing up from crouching]
        // change the size and offset
        coll.size = colliderStandSize;
        coll.offset = colliderStandOffset;
    }

}
