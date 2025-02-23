using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private UIManager uIManager;
    [SerializeField]
    private SFXManager sFXManager;
    [Header("Components")]
    [SerializeField]
    private Rigidbody playerBody;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private GameObject groundChecker;
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
    public bool isFacingLeft;
    [SerializeField]
    private float turnTime;
    private Vector2 moveDirection;
    private Quaternion rightFacing;
    private Quaternion leftFacing;
    public float gravScale;
    public float jumpBoost;
    public RaycastHit groundingHit;
    public float groundingDistance;
    public bool isJumping;
    public float jumpThreshold;
    public float groundCheckRadius;
    [Header("Player Damage Properties")]
    public bool isDead;
    public bool isHurt;
    public float maxFallHight;
    public float recoveryTime;
    private float hurtTimer;
    public float deathFadeTime;
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
    private bool isFreeHanging;
    public AnimationClip climbAnim;
    public AnimationClip freehandClimbAnim;
    private bool isClimbing;
    [Header("Camera Control Properties")]
    public CinemachineVirtualCamera playerCam;
    public bool isZoomedOut;
    public Collider2D boundingBox;
    public CinemachineConfiner2D confiner;
    public int zoomedInPos;
    public int zoomedOutPos;
    [Header("Interaction Properties")]
    public GameObject interactionPrompt;
    public Transform promptPosition;
    public bool interactionPosable;
    [SerializeField]
    private bool isGrabbingIntractable;
    public GameObject interactionTarget;
    public Transform grabPoint;
    public TextMeshProUGUI promptText;
    public float pushDistance;
    public float currentDistance;
    public bool isPushing;
    public bool isPulling;
    public float pushForce;
    [Header("Input Properties")]
    public InputActionAsset playerInputActions;
    public PlayerInput input;
    public bool isGamepadActive;
    public bool inputEnabled;
    [Header("Fall Check Properties")]
    public float lastFallHight;
    public  Vector3 launchPosition;
    public Vector3 landingPosition;
    public float fallThreshold;
    [Header("Checkpoint system")]
    public Checkpoint activeCheckpoint;

    

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        uIManager = FindObjectOfType<UIManager>();
        sFXManager = FindObjectOfType<SFXManager>();
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        playerAnim = this.gameObject.GetComponent<Animator>();
        input = this.gameObject.GetComponent<PlayerInput>();
        rightFacing = this.transform.rotation;
        leftFacing = new Quaternion(0,-transform.rotation.y,0,1);
        deathFadeTime = uIManager.deathFadeTime*3;
        confiner.InvalidateCache();
        gameObject.SetActive(false);
    }

    void Awake()
    {
        isDead = false;
        inputEnabled = true;
        confiner.InvalidateCache();
    }

    void Update()
    {
        if(inputEnabled)
        {
            CheckInputType();
            isGrounded = GroundingCheck(Vector2.down,groundingDistance);
            if(moveDirection.x != 0 && isGrounded)
            {
                playerAnim.SetBool("isIdle", false);
                isIdle = false;
            }
            else if(!isGrounded)
            {
                playerAnim.SetBool("isIdle", true);
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
                topOfLedge = ledge.transform.position;
                topOfLedge.y = ledge.transform.position.y + ledge.transform.localScale.y/2;
                this.gameObject.transform.position = activeOffset;
                playerAnim.SetBool("isIdle", true);
                if(isFreeHanging)
                {
                    playerAnim.SetBool("isFreeHanging", true);
                }
                else
                {
                    playerAnim.SetBool("isHanging", true);
                }
                isIdle = true;
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
                currentDistance = Vector3.Distance(transform.position,interactionTarget.transform.position);
                if(Vector3.Distance(transform.position, interactionTarget.transform.position) <= pushDistance)
                {
                    if(moveDirection.x != 0)
                    {
                        interactionTarget.transform.position += new Vector3(moveDirection.x * (Time.deltaTime * pushForce),0,0);
                    }
                    if(moveDirection.x < 0)
                    {
                        if(isFacingLeft)
                        {
                            playerAnim.SetBool("isPushing", true);
                            playerAnim.SetBool("isPulling", false);
                        }
                        else
                        {
                            playerAnim.SetBool("isPushing", false);
                            playerAnim.SetBool("isPulling", true);
                        }
                    }
                    else if(moveDirection.x > 0)
                    {
                        if(isFacingLeft)
                        {
                            playerAnim.SetBool("isPushing", false);
                            playerAnim.SetBool("isPulling", true);
                        }
                        else
                        {
                            playerAnim.SetBool("isPushing", true);
                            playerAnim.SetBool("isPulling", false);
                        }
                    }
                    else
                    {
                        playerAnim.SetBool("isPushing", false);
                        playerAnim.SetBool("isPulling", false);
                    }
                }
            }
            else if(interactionTarget == null)
            {
                isGrabbingIntractable = false;
                playerAnim.SetBool("isPushing", false);
                playerAnim.SetBool("isPulling", false);
            }
            if(interactionTarget != null && Vector3.Distance(transform.position,interactionTarget.transform.position) > pushDistance)
            {
                isGrabbingIntractable = false;
                interactionTarget = null;
                playerAnim.SetBool("isPushing", false);
                playerAnim.SetBool("isPulling", false);
            }
            if(isGamepadActive)
            {
                promptText.text = "B";
            }
            else
            {
                promptText.text = "E";
            }
            if(isHurt)
            {
                hurtTimer -= Time.deltaTime;
                if(hurtTimer <= 0)
                {
                    isHurt = false;
                }
            }
            if(isZoomedOut)
            {
                if(playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance < zoomedOutPos)
                {
                    playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance += Time.deltaTime *2; 
                }
            }
            else
            {
                if(playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance > zoomedInPos)
                {
                    playerCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= Time.deltaTime *2;
                }
            }
            if(!sFXManager.source2D.isPlaying && moveDirection.x != 0 && isGrounded)
            {
                sFXManager.Player2DSFX(sFXManager.metalStep,true);
            }
            else if(sFXManager.source2D.isPlaying && moveDirection.x == 0)
            {
                sFXManager.source2D.Stop();
            }
            interactionPrompt.transform.position = promptPosition.position;
            //Debug.Log(playerBody.velocity.y + " = player Y velocities");
            if(playerBody.velocity.y < fallThreshold && !isGrabbingLedge)
            {
                playerBody.AddForce(Vector3.down * gravScale);
            }
            if(playerBody.velocity.y > jumpThreshold)
            {
                isJumping = true;
            }
            else
            {
                isJumping = false;
            }
        }  
    }

    void FixedUpdate()
    {
        if(inputEnabled)
        {
            if(!isGrabbingLedge)
            {
                if(!isGrabbingIntractable)
                {
                    playerBody.velocity = new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
                    if(isJumping)
                    {
                        playerBody.velocity = new Vector3(playerBody.velocity.x, playerBody.velocity.y*jumpBoost,playerBody.velocity.z);
                    }
                }
                if(isGrabbingIntractable)
                {
                    playerBody.velocity = new Vector3(moveDirection.x * (moveSpeed*.5f) * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
                }
                
            }
        }
    }

    /// <summary>
    /// Event called by input system when move input is detected.
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay && inputEnabled)
        {
            //Movement logic
            Vector2 moveVector2 = movementValue.Get<Vector2>();
            if(!isGrabbingLedge)
            {
                moveDirection.x = moveVector2.x;
                playerAnim.SetBool("isIdle",false);
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
        if(col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Box"))
        {
            SetLandingPosition();
        }
    }

    /// <summary>
    ///  Used for isGrounded check.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay(Collision col)
    {
        // if(col.gameObject.CompareTag("Box") && col.gameObject.transform.position.y + col.gameObject.transform.localScale.y/2 < transform.position.y)
        // {
        //     isGrounded = true;
        // }
        // if(col.gameObject.CompareTag("Platform"))
        // {
        //     isGrounded = true;
        // }
    }

    /// <summary>
    /// Event called when collision ends 
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Box"))
        {
            //isGrounded = false;
            SetLaunchPosition();
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
        if(interactionPosable && inputEnabled)
        {
            if(isGrabbingIntractable)
            {
                isGrabbingIntractable = false;
            }
            else if(!isGrabbingLedge)
            {
                isGrabbingIntractable = true;
                //interactionTarget.transform.position = grabPoint.position;
            }
        }
    }
    /// <summary>
    /// Event called when input system detects jump input. 
    /// </summary>
    void OnJump()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay && inputEnabled)
        {
            if(isGrounded)
            {
                playerAnim.SetTrigger("jump");
                playerAnim.SetBool("isJumping",true);
                playerBody.AddForce(transform.up * jumpForce);
                sFXManager.source2D.Stop();
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
    /// Event for when the grab is triggered by a ledge. 
    /// </summary>
    /// <param name="trigger"></param>
    public void GrabTriggered(GameObject trigger, bool isFreeHanging)
    {
        if(trigger.TryGetComponent<Climbable>(out Climbable other) && trigger.transform.position.y > this.gameObject.transform.position.y)
        {
            this.isFreeHanging = isFreeHanging;
            if(isFacingLeft && other.gameObject.transform.position.x < transform.position.x)
            {
                ledge = trigger;
                LedgeGrab(ledge);
            }
            if(!isFacingLeft && other.gameObject.transform.position.x > transform.position.x)
            {
                ledge = trigger;
                LedgeGrab(ledge);
            }
            topOfLedge = ledge.transform.position;
            topOfLedge.y = ledge.transform.position.y + ledge.transform.localScale.y/2;
            if(isFreeHanging)
            {
                if(isFacingLeft)
                {
                    topOfLedge.x = ledge.transform.position.x - ledge.transform.localScale.x/2;
                }
                else
                {
                    topOfLedge.x = ledge.transform.position.x + ledge.transform.localScale.x/2;
                }
            }
        }
    }

    /// <summary>
    /// Set player into ledge hanging position. 
    /// </summary>
    /// <param name="ledge"></param>
    void LedgeGrab(GameObject ledge)
    {
        if(!isClimbing)
        {
            playerAnim.SetTrigger("grab");
        }
        this.ledge = ledge;
        isGrabbingLedge = true;
    }

    /// <summary>
    /// Moves player smoothly to the top of target ledge. Disables input during climb. 
    /// </summary>
    /// <returns></returns>
    IEnumerator LedgeClimb()
    {
        if(isFreeHanging)
        {
            climbDuration = freehandClimbAnim.length *.8f;
        }
        else
        {
            climbDuration = climbAnim.length *.8f;
        }
        isClimbing = true;
        playerAnim.SetTrigger("climb");
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
        playerAnim.SetBool("isFreeHanging", false);
        ledge = null;
        topOfLedge = Vector3.zero;
        activeOffset = Vector3.zero;
        moveDirection = Vector2.zero;
        isGrabbingLedge = false;
        input.enabled = true;
        isClimbing = false;
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
            confiner.InvalidateCache();
        }
    }

    /// <summary>
    /// Used to se the start position for a fall check.
    /// </summary>
    private void SetLaunchPosition()
    {
        launchPosition = transform.position;
    }

    /// <summary>
    /// used to set the end position for the fall check.
    /// </summary>
    private void SetLandingPosition()
    {
        landingPosition = transform.position;
        playerAnim.SetBool("isJumping", false);
        CheckForFall();
    }

    /// <summary>
    /// Checks if the player has fallen more than the max fall hight, if they have, player takes damage. 
    /// </summary>
    private void CheckForFall()
    {
        lastFallHight = launchPosition.y - landingPosition.y;
        if(launchPosition.y - landingPosition.y >= maxFallHight)
        {
            TakeDamage();
            launchPosition = landingPosition;
        }
    }

    /// <summary>
    /// Makes the player take damage, if already hurt, trigger player death. 
    /// </summary>
    public void TakeDamage()
    {
        if(isHurt)
        {
            StartCoroutine(Death());
        }
        else
        {
            isHurt = true;
            hurtTimer = recoveryTime;
        }
    }

    /// <summary>
    /// Used to set the new checkpoint when reached
    /// </summary>
    /// <param name="checkpoint"></param>
    public void CheckpointHit(Checkpoint checkpoint)
    {
        activeCheckpoint = checkpoint;
        activeCheckpoint.canBeActivated = false;
    }

    /// <summary>
    /// Kills the player, trigger the fade out fade in, can be timed to death animation. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Death()
    {
        StartCoroutine(uIManager.DeathUIFadeIN());
        yield return new WaitForSeconds(deathFadeTime);
        if(activeCheckpoint != null)
        {
            transform.position = activeCheckpoint.transform.position;
            isFacingLeft = activeCheckpoint.leftLevelFlow;
        }
        else
        {
            transform.position = levelManager.spawn.transform.position;
        }
        StartCoroutine(uIManager.DeathUIFadeOut());
    }

    /// <summary>
    /// Checks a small area around the bottom of the player to check for grounding. 
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool GroundingCheck(Vector2 direction, float distance)
    {
        Ray ray = new Ray(groundChecker.transform.position,direction); 
        
        if(Physics.SphereCast(ray.origin,groundCheckRadius,ray.direction, out groundingHit, distance))
        {
            if(groundingHit.collider.gameObject.tag == "Platform" || groundingHit.collider.gameObject.tag == "Box" || groundingHit.collider.gameObject.tag == "Door")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //If the logic makes it to hear, then there aren't any layers that whatever child script called this method should be looking out for and returns false back to that child script
        return false;
    }

    /// <summary>
    /// Draws a gizmo, curently showing the grounding check area.
    /// </summary>
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundChecker.transform.position,groundCheckRadius);
    }
}
