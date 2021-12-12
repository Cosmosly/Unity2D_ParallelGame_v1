using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // [obtain the components]
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("Movement Parameters")]
    [SerializeField] private float defaultSpeed;        // player default speed
    [SerializeField] private float crouchSpeedDivisor;  // division the speed when player crouch
    [SerializeField] private float jumpForce;           // basic jump force
    [SerializeField] private float jumpHoldForce;       // additional jump force while holding jump buttom
    [SerializeField] private float jumpHoldDuration;    // max hold jump time
    [SerializeField] private float crouchJumpBoost;     // addtional jump foce using crouch jump
    [SerializeField] private float jumpTime;            // hom much time holding jump buttom is working
    [SerializeField] private float hangingJumpForce;    // a force upwards press space while hanging on the wall

    [Header("Movement States")]
    [SerializeField] public bool isCrouch;             // Wheather player is crouching or not
    [SerializeField] public bool isOnGround;           // Wheather player is on the ground
    [SerializeField] public bool isJump;               // Wheather player is jumping or not
    [SerializeField] private bool isHeadBlocked;        // Wheather player's head is touching sth
    [SerializeField] public bool isHanging;            // Wheather player is hanging on the wall

    [Header("Movement Figure")]
    [SerializeField] public float xVelocity;               // determin the force from x axis
    [SerializeField] private Vector2 colliderStandSize;     // original box collider's size
    [SerializeField] private Vector2 colliderStandOffset;   // original box collider's offset
    [SerializeField] private Vector2 colliderCrouchSize;    // box collider's size when crouching(half original)
    [SerializeField] private Vector2 colliderCrouchOffset;  // box collider's offset when crouching(half original)

    [Header("Environment Detection")]
    public LayerMask groundLayer;                            // the Ground Layer
    private float footOffset;                                // the distance between two legs
    private float headClearance;                             // the distance above head                       
    private float groundDistance;                            // the distance between rb and ground
    private float playerHeight;                              // the height of the player
    private float eyeHeight;                                 // the height of the player's eye
    private float grabDistance;                              // the farest distance player can grab wall
    private float reachOffset;                               // to determin that above the player has not wall, below the player has wall


    [Header("Button Detection")]
    public bool jumpPressed;                                 // when pressed down space
    public bool jumpHeld;                                    // when held space
    public bool crouchHeld;                                  // when held s
    public bool crouchPressed;                               // when pressed s

    void Start()
    {

        // [Initialize movement parameters]
        InitializePlayerMovement(8f, 0.3f,                 // speed, crouchSpeedDivisor
            6.3f, 1.9f, 0.1f, 2.5f, 15f,                       // jumpForce, jumpHoldForce, jumpHoldDuration, crouchJumpBoost, hangingJumpForce
            0.4f, 0.5f, 0.2f, 1.5f, 0.4f, 0.7f);           // footOffset, headClearance, groundDistance, eyeHeight, grabDistance, reachOffset
    }

   
    void Update()
    {
        if (GameManager.GameOver())
            return;

        // all relative with input button should place in the Update function not in the fixedUpdate
        // [Input button assignment]
        // press jump one time, you need to do this because the fixUpdate and Update
        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        if (Input.GetButtonDown("Crouch"))
            crouchPressed = true;

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
        // back to Update
        if (isJump)
            jumpPressed = false;
        if (isCrouch)
            crouchPressed = false;
    }

    /*
     * Initialize the parameter
     */
    public void InitializePlayerMovement(float _speed, float _crouchSpeedDivisor,
        float _jumpForce, float _jumpHoldForce, float _jumpHoldDuration, float _crouchJumpBoost, float _hangingJumpForce,
        float _footOffset, float _headClearance, float _groundDistance, float _eyeHeight, float _grabDistance, float _reachOffset)
    {

        // [Get component]
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();

        // [Player State]
        playerHeight = coll.size.y;

        // [Speed parameters]
        defaultSpeed = _speed;
        crouchSpeedDivisor = _crouchSpeedDivisor;

        // [Jump parameters]
        jumpForce = _jumpForce;
        jumpHoldForce = _jumpHoldForce;
        jumpHoldDuration = _jumpHoldDuration;
        crouchJumpBoost = _crouchJumpBoost;
        hangingJumpForce = _hangingJumpForce;

        // [Crouch setting]
        // store the original stand state
        colliderStandSize = coll.size;
        colliderStandOffset = coll.offset;
        // set up the crouch size
        colliderCrouchSize = new Vector2(colliderStandSize.x, colliderStandSize.y * 0.5f);
        colliderCrouchOffset = new Vector2(colliderStandOffset.x, colliderStandOffset.y * 0.5f);

        // [Environment detect]
        footOffset = _footOffset;
        headClearance = _headClearance;
        groundDistance = _groundDistance;
        eyeHeight = _eyeHeight;
        grabDistance = _grabDistance;
        reachOffset = _reachOffset;
    }


    /*
     * Using Physics to check player states
     */
     private void PhysicsCheck()
    {
        RaycastHit2D leftFootCheck = Rb_Raycast(new Vector2(-footOffset, 0), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightFootCheck = Rb_Raycast(new Vector2(footOffset, 0), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D headCheck = Rb_Raycast(new Vector2(0, playerHeight), Vector2.up, headClearance, groundLayer);

        // [Check if player is on the ground]
        if (leftFootCheck || rightFootCheck)
            isOnGround = true;
        else isOnGround = false;

        // [Check if player's head is blocked]
        if (headCheck)
            isHeadBlocked = true;
        else isHeadBlocked = false;

        // [Hanging raycast]
        // direction
        float playerDirection = transform.localScale.x;             // the 1D direction player is facing
        Vector2 grabDirection = new Vector2(playerDirection, 0);    // the 2D direction player is facing

        RaycastHit2D playerHeightCheck = Rb_Raycast(new Vector2(footOffset * playerDirection, playerHeight), grabDirection, grabDistance, groundLayer);
        RaycastHit2D eyeHeightCheck = Rb_Raycast(new Vector2(footOffset * playerDirection, eyeHeight), grabDirection, grabDistance, groundLayer);
        RaycastHit2D ledgeCheck = Rb_Raycast(new Vector2(reachOffset * playerDirection, playerHeight), Vector2.down, grabDistance, groundLayer);

        if(!isOnGround && rb.velocity.y < 0f && ledgeCheck && eyeHeightCheck && !playerHeightCheck)
        {
            // [Let Hanging position be parallel to the top ground]
            Vector3 pos = transform.position;
            pos.x += (eyeHeightCheck.distance - 0.05f) * playerDirection;
            pos.y -= ledgeCheck.distance;
            transform.position = pos;

            // [Hanging]
            rb.bodyType = RigidbodyType2D.Static;
            isHanging = true;
        }
    }


    /*
     * Control player when moving on the ground
     */
    private void GroundMovement()
    {
        // [While Hanging]
        if (isHanging)
            return; // do nothing about the GroundMovement

        // [Consider crouch-S]
        // if player is in the crouch state, execute crouch movement
        if (crouchHeld && !isCrouch && isOnGround)
            Crouch();
        // if player is in the crouch state and loose the s buttom, player stand up
        else if (!crouchHeld && isCrouch && !isHeadBlocked)
            StandUp();
        else if (!isOnGround && isCrouch)
            StandUp();

        // [Get input and set up velocity]
        xVelocity = Input.GetAxis("Horizontal");         // between -1 ~ 1 , default 0; Input.GetAxisRaw() -1,1,0 imediately stop
        // if player in the crouch state, the speed is decrease by the crouchSpeedDivisor
        if (isCrouch)
            xVelocity *= crouchSpeedDivisor;
        
        rb.velocity = new Vector2(xVelocity * defaultSpeed, rb.velocity.y);

        // [Changing facing direction]
        FlipDirection();
    }


    /*
     *  Control the jumping movement
     */
     private void MidAirMovement()
    {
        // [Hanging Jump]
        if (isHanging)
        {
            if (jumpPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForce(new Vector2(0f, hangingJumpForce), ForceMode2D.Impulse);
                isHanging = false;
                isJump = true;
            }
            
            if (crouchPressed)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                isHanging = false;
            }
        }


        // [jump single press]
        if(jumpPressed && isOnGround && !isJump && !isHeadBlocked)
        {
            if(isCrouch)
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
            // [Audio Play]
            AudioManager.PlayJumpAudio();
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
            transform.localScale = new Vector3(-1, 1, 1);
        if (xVelocity > 0)
            transform.localScale = new Vector3(1, 1, 1);

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


    /*
     *  Manage Raycast in order to reference rb position state
     */
     RaycastHit2D Rb_Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask layer)
    {
        Vector2 position = transform.position;   // rb's position

        // [generate raycast]
        RaycastHit2D hit2D = Physics2D.Raycast(position + offset, rayDirection, length, layer);
        // [draw raycast]
        Color color = hit2D ? Color.red : Color.green;
        Debug.DrawRay(position + offset, rayDirection * length, color);
        

        return hit2D;
    }
}
