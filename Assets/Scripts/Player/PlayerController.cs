using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private GameManager gameManager;
    [Header("Components")]
    [SerializeField]
    private Rigidbody playerBody;
    [SerializeField]
    private Animator playerAnim;
    [Header("PlayerStats")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private bool isIdle;
    [SerializeField]
    private bool isGrabbingLedge;
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool isFacingLeft;
    [SerializeField]
    private float turnTime;
    private Vector2 moveDirection;
    private Quaternion rightFacing;
    private Quaternion leftFacing;
    [Header("Ledge Grab Properties")]
    [SerializeField]
    private Vector3 activeOffset;
    [SerializeField]
    private Vector3 leftOffset;
    [SerializeField]
    private Vector3 rightOffset;
    private GameObject ledge;
    [SerializeField]
    private float climbDuration;
    [SerializeField]
    private Vector3 topOfLedge;
    private Vector3 desiredPosition;
    [Header("Camera Control Properties")]
    public Collider2D boundingBox;
    public CinemachineConfiner2D confiner;
    [Header("Interaction Properties")]
    public GameObject interactionPrompt;
    public Transform promptPosition;
    public bool interactionPosable;
    [SerializeField]
    private bool isGrabbingIntractable;
    public Transform heldObjectLocation;
    public GameObject interactionTarget;
    public TextMeshProUGUI promptText;
    [Header("Input Properties")]
    public InputActionAsset playerInputActions;
    public PlayerInput input;
    public bool isGamepadActive;
    

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        playerAnim = this.gameObject.GetComponent<Animator>();
        input = this.gameObject.GetComponent<PlayerInput>();
        rightFacing = this.transform.rotation;
        leftFacing = new Quaternion(0,-rightFacing.y,0,1);
    }

    void Update()
    {
        CheckInputType();
        if(moveDirection.x != 0)
        {
            playerAnim.SetBool("isIdle", false);
            isIdle = false;
        }
        else if(moveDirection.x == 0 && moveDirection.y == 0)
        {
            playerAnim.SetBool("isIdle", true);
            isIdle = true;
        }
        if(moveDirection.x > 0 && !isGrabbingIntractable)
        {
            if(isFacingLeft)
            {
                isFacingLeft = false;
                transform.rotation = rightFacing;
            }
        }
        else if(moveDirection.x < 0 && !isGrabbingIntractable)
        {
            if(!isFacingLeft)
            {
                isFacingLeft = true;
                transform.rotation = leftFacing;
            }
        }
        if(isGrabbingLedge == true)
        {
            topOfLedge = ledge.transform.position;
            topOfLedge.y = ledge.transform.position.y * 1.1f;
            if(ledge.transform.position.x < this.gameObject.transform.position.x)
            {
                activeOffset = leftOffset + ledge.transform.position;
                //used to make sure player is facing ledge
            }
            if(ledge.transform.position.x > this.gameObject.transform.position.x)
            {
                activeOffset = rightOffset + ledge.transform.position;
                //used to make sure player is facing ledge
            }
            this.gameObject.transform.position = activeOffset;
            playerAnim.SetBool("isIdle", true);
            isIdle = true;
            playerAnim.SetBool("isHanging", true);
        }
        if(interactionPosable && !isGrabbingIntractable)
        {
            interactionPrompt.SetActive(true);
        }
        else
        {
            interactionPrompt.SetActive(false);
        }
        if(isGrabbingIntractable && interactionTarget != null)
        {
            interactionTarget.transform.position = heldObjectLocation.position;
        }
        if(isGamepadActive)
        {
            promptText.text = "B";
        }
        else
        {
            promptText.text = "E";
        }
        interactionPrompt.transform.position = promptPosition.position;
    }

    void FixedUpdate()
    {
        if(!isGrabbingLedge)
        {
            if(!isGrabbingIntractable)
            {
                playerBody.velocity = new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
            }
            if(isGrabbingIntractable)
            {
                playerBody.velocity = new Vector3(moveDirection.x * (moveSpeed*.8f) * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
            }
        }
    }

    /// <summary>
    /// Event called by input system when move input is detected.
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            //Movement logic
            Vector2 moveVector2 = movementValue.Get<Vector2>();
            if(!isGrabbingLedge)
            {
                moveDirection.x = moveVector2.x;
            }
            else if(isGrabbingLedge)
            {
                moveDirection.y = moveVector2.y;
                if(moveDirection.y > 0)
                {
                    StartCoroutine(LedgeClimb());
                }
                if(moveDirection.y < 0)
                {
                    playerAnim.SetBool("isHanging", false);
                    isGrabbingLedge = false;
                    ledge = null;
                    topOfLedge = Vector3.zero;
                    activeOffset = Vector3.zero;
                }
            }
        }
    }

    /// <summary>
    /// Event called when collision starts.
    /// </summary>
    /// <param name="col"></param>
     void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.TryGetComponent<Climbable>(out Climbable other) && col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            if(isFacingLeft && other.gameObject.transform.position.x < transform.position.x)
            {
                ledge = other.gameObject;
                LedgeGrab(ledge);
            }
            if(!isFacingLeft && other.gameObject.transform.position.x > transform.position.x)
            {
                ledge = other.gameObject;
                LedgeGrab(ledge);
            }
        }
    }

    /// <summary>
    ///  Used for isGrounded check.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// Event called when collision ends 
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
        else if(col.gameObject.TryGetComponent<Intractable>(out Intractable other))
        {
            interactionPosable = false;
            interactionTarget = null;
        }
    }
    /// <summary>
    /// Event called when input system detects interaction input. 
    /// </summary>
    void OnInteract()
    {
        if(interactionPosable)
        {
            if(isGrabbingIntractable)
            {
                isGrabbingIntractable = false;
            }
            else if(!isGrabbingLedge)
            {
                isGrabbingIntractable = true;
            }
        }
    }
    /// <summary>
    /// Event called when input system detects jump input. 
    /// </summary>
    void OnJump()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            if(isGrounded)
            {
                playerBody.AddForce(transform.up * jumpForce);
            }
            if(isGrabbingLedge)
            {
                StartCoroutine(LedgeClimb());
            }
        }
    }
    /// <summary>
    /// Event called when pause input is detected. 
    /// </summary>
    void OnPause()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            if(gameManager.isPaused)
            {
                gameManager.ResumeGame();
            }   
            else
            {
                gameManager.PauseGame();
            }   
        }
    }

    /// <summary>
    /// Set player into ledge hanging position. 
    /// </summary>
    /// <param name="ledge"></param>
    void LedgeGrab(GameObject ledge)
    {
        this.ledge = ledge;
        isGrabbingLedge = true;
    }

    /// <summary>
    /// Moves player smoothly to the top of target ledge. Disables input during climb. 
    /// </summary>
    /// <returns></returns>
    IEnumerator LedgeClimb()
    {
        input.enabled = false;
        float climbTime = 0f;
        Vector3 startValue = transform.position;
        desiredPosition = topOfLedge;
        while (climbTime <= climbDuration)
        {
            climbTime += Time.deltaTime;  
            transform.position = Vector3.Lerp(startValue, topOfLedge, climbTime/climbDuration);
            yield return null;
        }
        transform.position = topOfLedge;
        playerAnim.SetBool("isHanging", false);
        ledge = null;
        topOfLedge = Vector3.zero;
        activeOffset = Vector3.zero;
        moveDirection = Vector2.zero;
        isGrabbingLedge = false;
        input.enabled = true;
    }

    /// <summary>
    /// Checks for active input device between keyboard/mouse and gamepad. 
    /// </summary>
    void CheckInputType()
    {
        foreach (InputDevice device in input.devices)
        {
            if (device is Mouse || device is Keyboard)
            {
                isGamepadActive = false;
            }
            else if (device is Gamepad)
            {
                isGamepadActive = true;
            }
        }   
    } 

    /// <summary>
    /// Sets camera bounding shape.
    /// </summary>
    public void SetBoundingBox()
    {
        if(boundingBox != null)
        {
            confiner.m_BoundingShape2D = boundingBox;
        }
    }   
}
