using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Cinemachine;
using System;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Managers")]
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private LevelManager levelManager;
    [SerializeField]
    private UIManager uIManager;
    [SerializeField]
    private SFXManager sFXManager;
    [Header("Player State")]
    [SerializeField]
    public ActionState actionState;
    private ActionState prevState;
    public enum ActionState{Idle, Moving, Jumping, Falling, Hanging, Climbing}
    [SerializeField]
    private bool isGrounded;
    public bool isFacingLeft;
    [Header("Components")]
    [SerializeField]
    private Rigidbody playerBody;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private GameObject groundChecker;
    [SerializeField]
    private Transform rightFacing;
    [SerializeField]
    private Transform leftFacing;

    [Header("Player Stats")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float holdForce;
    public float gravScale;
    private Vector2 moveDirection;
    public float maxYVelocity;
    public float walkCycleSpeed;
    [Header("Ground Check Properties")]
    public float coyoteTimeLimit;
    public float groundTimer;
    public bool timerActive;
    public RaycastHit groundingHit;
    public float groundingDistance;
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
    private Coroutine climbRoutine;
    [Header("Camera Control Properties")]
    public CinemachineVirtualCamera playerCam;
    public bool isZoomedOut;
    public Collider2D boundingBox;
    public CinemachineConfiner2D confiner;
    public int zoomedInPos;
    public int zoomedOutPos;
    public int menuClipPlane;
    public int gameplayClipPlane;
    [Header("Interaction Properties")]
    public bool interactionPosable;
    [SerializeField]
    private bool isGrabbingIntractable;
    public GameObject interactionTarget;
    public Transform grabPoint;
    public float pushDistance;
    public float currentDistance;
    public float pushForce;
    [Header("Input Properties")]
    public bool debugMode;
    public InputActionAsset playerInputActions;
    public InputAction moveAction;
    private InputAction jumpAction;
    public PlayerInput input;
    public bool isGamepadActive;
    public bool inputEnabled;
    public GameObject promptHolder;
    public Transform promptPos;
    public float jumpHoldTime;
    private float jumpTime;
    private bool jumpPressed;
    private float walkSFXTimer;
    [Header("Fall Check Properties")]
    public float lastFallHight;
    public  Vector3 launchPosition;
    public Vector3 landingPosition;
    public float fallThreshold;
    [Header("Checkpoint system")]
    public Checkpoint activeCheckpoint;

    #endregion
    #region Unity Events
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        uIManager = FindObjectOfType<UIManager>();
        sFXManager = FindObjectOfType<SFXManager>();
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        playerAnim = this.gameObject.GetComponent<Animator>();
        input = this.gameObject.GetComponent<PlayerInput>();
        deathFadeTime = uIManager.deathFadeTime*3;
        confiner.InvalidateCache();
        playerAnim.SetBool("isIdle",true);
        gameObject.SetActive(false);
        jumpPressed = false;
        moveAction = playerInputActions.FindAction("Move");
        jumpAction = playerInputActions.FindAction("Jump");
        walkSFXTimer = walkCycleSpeed;
    }

    void Awake()
    {
        isDead = false;
        inputEnabled = true;
        confiner.InvalidateCache();
    }

    void Update()
    {
        SetCameraClippingPlane();
        if(inputEnabled)
        {
            CheckInputType();
            isGrounded = GroundingCheck(Vector2.down,groundingDistance);
            switch(actionState)
            {
                case ActionState.Hanging:
                    IsHanging();
                    break;
                case ActionState.Idle:
                    playerAnim.SetBool("isIdle", true);
                    break;
                case ActionState.Moving:
                    MovingSFX();
                    break;
            }
            if(moveDirection.x > 0 && !isGrabbingIntractable && actionState != ActionState.Hanging)
            {       
                if(isFacingLeft)
                {
                    isFacingLeft = false;
                    transform.rotation = rightFacing.rotation;
                }
            }
            else if(moveDirection.x < 0 && !isGrabbingIntractable && actionState != ActionState.Hanging)
            {
                if(!isFacingLeft)
                {
                    isFacingLeft = true;
                    transform.rotation = leftFacing.rotation;
                }
            }
            CheckPushPull();
            HurtTimer();
            CameraZoomCheck();
            SoundCheck();
            CheckIfFalling();
            if(actionState == ActionState.Falling)
            {
                playerBody.AddForce(Vector3.down * gravScale);
            }
            if(moveAction.IsPressed())
            {
                MovementLogic(moveDirection);
            }
        }
        CoyoteTimer(); 
        //Debug.Log("Player Y velocity = " + playerBody.velocity.y); 
    }

    void FixedUpdate()
    {
        if(inputEnabled)
        {
            if(actionState != ActionState.Hanging && actionState != ActionState.Climbing)
            {
                if(!isGrabbingIntractable)
                {
                    playerBody.velocity = new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
                    if(actionState == ActionState.Jumping)
                    {
                        playerBody.velocity = new Vector3(playerBody.velocity.x*0.8f, playerBody.velocity.y,playerBody.velocity.z);
                    }
                }
                if(isGrabbingIntractable)
                {
                    playerBody.velocity = new Vector3(moveDirection.x * (moveSpeed*.5f) * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
                }
                
            }
            InteractPrompt();
        }
    }

    #endregion
    #region Input Events
    /// <summary>
    /// Event called by input system when move input is detected.
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {
        
        if(gameManager.gameState == GameManager.GameState.Gameplay && inputEnabled)
        {
            Vector2 moveVector2 = movementValue.Get<Vector2>();
            moveDirection = moveVector2.normalized;
            MovementLogic(moveDirection);
        }
    }

    void MovementLogic(Vector2 moveVector2)
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay && inputEnabled)
        {
            if(actionState == ActionState.Idle)
            {
                ChangeActionState(ActionState.Moving);
            }
            //Movement logic
            if(actionState != ActionState.Hanging && actionState != ActionState.Climbing)
            {
                if(moveVector2.x == 0)
                {
                    ChangeActionState(ActionState.Idle);
                }
            }
            if(actionState == ActionState.Hanging && moveVector2.y > 0)
            {
                climbRoutine = StartCoroutine(LedgeClimb());
            }
            else if(actionState == ActionState.Hanging && moveVector2.y < 0)
            {
                ChangeActionState(ActionState.Falling);
                ledge = null;
                topOfLedge = Vector3.zero;
                activeOffset = Vector3.zero;
            }
        }
    }


    /// <summary>
    /// Event called when input system detects interaction input. 
    /// </summary>
    void OnInteract()
    {
        if(interactionPosable && inputEnabled && interactionTarget != null)
        {
            if(interactionTarget.tag != null && interactionTarget.tag == "Box" || interactionTarget.tag == "ReflectorBox")
            {
                if(isGrabbingIntractable)
                {
                    isGrabbingIntractable = false;
                }
                else if(actionState != ActionState.Hanging && actionState != ActionState.Climbing)
                {
                    isGrabbingIntractable = true;
                    //interactionTarget.transform.position = grabPoint.position;
                }
            }
            else if(interactionTarget.TryGetComponent<LeverBehavior>(out LeverBehavior lever))
            {
                lever.ActivateLever();
                sFXManager.Player2DSFX(sFXManager.leverSFX,false);
            }
        }
    }
    /// <summary>
    /// Event called when input system detects jump input. 
    /// </summary>
    void OnJump()
    {
        if(gameManager.gameState == GameManager.GameState.Gameplay && inputEnabled && actionState != ActionState.Jumping && actionState != ActionState.Falling)
        {
            jumpPressed = true;
            if(isGrounded)
            {
                timerActive = false;
                Debug.Log("Jump Pressed");
                ChangeActionState(ActionState.Jumping);
                playerBody.velocity = playerBody.velocity.normalized;
                playerBody.AddForce(transform.up * jumpForce);
                if(playerBody.velocity.y > maxYVelocity)
                {
                    playerBody.velocity.Set(playerBody.velocity.x,maxYVelocity,playerBody.velocity.z);
                }
            }
            if(actionState == ActionState.Hanging)
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

    #endregion
    #region Collisions
    /// <summary>
    /// Event called when collision starts.
    /// </summary>
    /// <param name="col"></param>
     void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Receiver" || col.gameObject.tag == "Reflector" || col.gameObject.tag == "Platform" || col.gameObject.tag == "Box" && col.gameObject.transform.position.y + col.gameObject.transform.localScale.y/2 < transform.position.y)
        {
            isGrounded = true;
            jumpPressed = false;
            SetLandingPosition(); 
        }
    }

    /// <summary>
    ///  Used for as a backup isGrounded check for things that ignore raycast.
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Receiver" || col.gameObject.tag == "Reflector" || col.gameObject.tag == "Platform" || col.gameObject.tag == "Box" && col.gameObject.transform.position.y + col.gameObject.transform.localScale.y/2 < transform.position.y)
        {
            //isGrounded = true;
        }
    }
    

    /// <summary>
    /// Event called when collision ends 
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.CompareTag("Platform") || col.gameObject.CompareTag("Box"))
        {
            SetLaunchPosition();
        }
        else if(col.gameObject.TryGetComponent<Intractable>(out Intractable other))
        {
            interactionPosable = false;
            interactionTarget = null;
        }
    }
    #endregion
    #region Grab/Climb

    void IsHanging()
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
    }
    /// <summary>
    /// Event for when the grab is triggered by a ledge. 
    /// </summary>
    /// <param name="trigger"></param>
    public void GrabTriggered(GameObject trigger, bool isFreeHanging)
    {
        if(trigger.TryGetComponent<Climbable>(out Climbable other) && trigger.transform.position.y > this.gameObject.transform.position.y && actionState != ActionState.Hanging)
        {
            if(actionState != ActionState.Hanging && actionState != ActionState.Climbing)
            {
                playerAnim.SetTrigger("grab");
            }
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
            topOfLedge.y = ledge.transform.position.y + ledge.transform.localScale.y;
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
            playerAnim.SetBool("isFreeHanging", isFreeHanging);
        }
    }

    /// <summary>
    /// Set player into ledge hanging position. 
    /// </summary>
    /// <param name="ledge"></param>
    void LedgeGrab(GameObject ledge)
    {
        this.ledge = ledge;
        ChangeActionState(ActionState.Hanging);
    }

    /// <summary>
    /// Moves player smoothly to the top of target ledge. Disables input during climb. 
    /// </summary>
    /// <returns></returns>
    IEnumerator LedgeClimb()
    {
        ChangeActionState(ActionState.Climbing);
        if(isFreeHanging)
        {
            climbDuration = freehandClimbAnim.length *.5f;
        }
        else
        {
            climbDuration = climbAnim.length *.5f;
        }
        inputEnabled = false;
        float climbTime = 0f;
        Vector3 startValue = transform.position;
        desiredPosition = topOfLedge;
        while (climbTime <= climbDuration)
        {
            climbTime += Time.deltaTime; 
            topOfLedge = ledge.transform.position;
            topOfLedge.y = ledge.transform.position.y + ledge.transform.localScale.y/2;
            transform.position = Vector3.Lerp(startValue, topOfLedge, climbTime/climbDuration);
            yield return null;
        }
        transform.position = topOfLedge;
        ledge = null;
        topOfLedge = Vector3.zero;
        activeOffset = Vector3.zero;
        moveDirection = Vector2.zero;
        inputEnabled = true;
        if(moveAction.inProgress)
        {
            ChangeActionState(ActionState.Moving);
        }
        else
        {
            ChangeActionState(ActionState.Idle);
        }
    }

    #endregion
    #region Input/Bounding
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
    #endregion
    #region Damage/Death

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
        CheckForFallDamage();
    }

    /// <summary>
    /// Checks if the player has fallen more than the max fall hight, if they have, player takes damage. 
    /// </summary>
    private void CheckForFallDamage()
    {
        if(actionState == ActionState.Falling)
        {
            if(moveDirection.x == 0)
            {
                ChangeActionState(ActionState.Idle);          
            }
            else
            {
                ChangeActionState(ActionState.Moving);          
            }
        }
        lastFallHight = launchPosition.y - landingPosition.y;
        if(launchPosition.y - landingPosition.y >= maxFallHight)
        {
            TakeDamage();
            RollForHurtSFX();
            launchPosition = landingPosition;
        }
    }

    void CheckIfFalling()
    {
        if(actionState != ActionState.Falling && actionState != ActionState.Hanging && actionState != ActionState.Climbing)
        {
            if(playerBody.velocity.y < fallThreshold && !isGrounded)
            {
                ChangeActionState(ActionState.Falling);
            }
        }
    }

    void RollForHurtSFX()
    {
        int roll;
        roll = UnityEngine.Random.Range(0, sFXManager.playerHurtSFX.Count);
        Debug.Log("Hurt roll=" + roll);
        sFXManager.Player2DSFX(sFXManager.playerHurtSFX[roll],false);
    }

    void RollForDeathSFX()
    {
        int roll;
        roll = UnityEngine.Random.Range(0, sFXManager.playerDeathSFX.Count);
        Debug.Log("Death roll=" + roll);
        sFXManager.Player2DSFX(sFXManager.playerDeathSFX[roll],false);
    }

    /// <summary>
    /// Makes the player take damage, if already hurt, trigger player death. 
    /// </summary>
    public void TakeDamage()
    {
        uIManager.playerDamage();
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

    private void HurtTimer()
    {
        if(isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if(hurtTimer <= 0)
            {
                isHurt = false;
            }
        }
    }

    /// <summary>
    /// Kills the player, trigger the fade out fade in, can be timed to death animation. 
    /// </summary>
    /// <returns></returns>
    IEnumerator Death()
    {
        RollForDeathSFX();
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

    #endregion
    #region Ground Check
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
            if(groundingHit.collider.gameObject.tag == "Platform" || groundingHit.collider.gameObject.tag == "Box" || groundingHit.collider.gameObject.tag == "Door" || groundingHit.collider.gameObject.tag == "ReflectorBox" || groundingHit.collider.gameObject.tag == "Reflector")
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

    void CoyoteTimer()
    {
        if(timerActive)
        {
            playerBody.useGravity = false;
            groundTimer -= Time.deltaTime;
            if(groundTimer > 0)
            {
                isGrounded = true;
                jumpPressed = false;
            }
            else
            {
                timerActive = false;   
                playerBody.useGravity = true;
            }
        }
        else
        {
            playerBody.useGravity = true;
        }
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
    #endregion
    #region Misc Methods
    /// <summary>
    /// used by level manager to make sure player faces the right way. 
    /// </summary>
    public void faceLeft()
    {
        isFacingLeft = true;
        transform.rotation = leftFacing.rotation;
    }

    private void CheckPushPull()
    {
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
    }

    void CameraZoomCheck()
    {
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
    }

    void SoundCheck()
    {
        if(sFXManager.playerWalkSource.isPlaying && moveDirection.x == 0)
        {
            sFXManager.playerWalkSource.Stop();
        }
    }

    void MovingSFX()
    {
        if(walkSFXTimer > 0)
        {
            walkSFXTimer -= Time.deltaTime;
        }
        else
        {
            sFXManager.playerWalkSource.PlayOneShot(sFXManager.metalStep);
            walkSFXTimer = walkCycleSpeed;
        }
    }

    void InteractPrompt()
    {
        if(interactionPosable && !isGrabbingIntractable)
        {
            promptHolder.SetActive(true);
            promptHolder.transform.position = promptPos.position;
        }
        else
        {
            promptHolder.SetActive(false);
        }
    }

    void SetCameraClippingPlane()
    {
        if(gameManager.gameState == GameManager.GameState.MainMenu)
        {
            playerCam.m_Lens.FarClipPlane = menuClipPlane;
        }
        else if(gameManager.gameState == GameManager.GameState.Gameplay)
        {
            playerCam.m_Lens.FarClipPlane = gameplayClipPlane;
        }
    }

    #endregion
    #region Debug Controls
    /// <summary>
    /// Debug controls to load level 1.
    /// </summary>
    void OnDebugLoadLevel1()
    {
        if(debugMode)
        {
            levelManager.DebugLoadScene("L_1");
        }
    }

    /// <summary>
    /// Debug controls to load level 2.
    /// </summary>
    void OnDebugLoadLevel2()
    {
        if(debugMode)
        {
            levelManager.DebugLoadScene("L_2");
        }
    }

    /// <summary>
    /// Debug controls to load level 3.
    /// </summary>
    void OnDebugLoadLevel3()
    {
        if(debugMode)
        {
            levelManager.DebugLoadScene("L_3");   
        }
    }
    #endregion
    #region State machine

    public void ChangeActionState(ActionState nextState)
    {
        //Debug.Log("Changing to action state to " + nextState);
        if(prevState != actionState && actionState != ActionState.Hanging)
        {
            prevState = actionState;
        }
        if(actionState != ActionState.Hanging)
        {
            ExitState(prevState);
        }
        EnterState(nextState);    
    }

    private void EnterState(ActionState state)
    {
        switch(state)
        {
            case ActionState.Idle:
                Idle();
                break;
            case ActionState.Moving:
                Moving();
                break;
            case ActionState.Jumping:
                Jumping();
                break;
            case ActionState.Falling:
                Falling();
                break;
            case ActionState.Climbing:
                Climbing();
                break;
            case ActionState.Hanging:
                Hanging();
                break;
            default:
                Idle();
                Debug.Log("Default for Enter state");
                break;
        }
        actionState = state;
    }
    private void ExitState(ActionState state)
    {
        switch(state)
        {
            case ActionState.Idle:
                IdleExit();
                break;
            case ActionState.Moving:
                MovingExit();
                break;
            case ActionState.Jumping:
                JumpingExit();
                break;
            case ActionState.Falling:
                FallingExit();
                break;
            case ActionState.Climbing:
                ClimbingExit();
                break;
            default:
                IdleExit();
                Debug.Log("Default for Exit state");
                break;
        }
    }

    private void Idle()
    {
        playerAnim.SetBool("isHanging", false);
        playerAnim.SetBool("isFreeHanging", false);
        if(prevState == ActionState.Falling)
        {
            playerAnim.SetTrigger("landing");
        }
    }

    private void Moving()
    {
        playerAnim.SetBool("isIdle", false);
        if(prevState == ActionState.Falling)
        {
            playerAnim.SetTrigger("landing");
        }
    }

    private void Jumping()
    {
        playerAnim.SetBool("isIdle", false);
        playerAnim.SetTrigger("jump");
        playerAnim.SetBool("isJumping",true);
        sFXManager.playerWalkSource.Stop();
        sFXManager.Player2DSFX(sFXManager.jumpSFX,false);
    }

    private void Falling()
    {
        playerAnim.SetBool("isIdle", false);
        playerAnim.SetBool("isFalling", true);
        if(prevState != ActionState.Jumping && !jumpPressed)
        {
            groundTimer = coyoteTimeLimit;
            timerActive = true;
        }
    }

    private void Climbing()
    {
        playerAnim.SetTrigger("climb");
        playerAnim.SetBool("isClimbing", true);
        playerAnim.SetBool("isIdle", false);
    }

    private void Hanging()
    {
        playerAnim.SetBool("isHanging",true);
    }

    private void IdleExit()
    {

    }

    private void MovingExit()
    {

    }

    private void JumpingExit()
    {

    }

    private void FallingExit()
    {
        playerAnim.SetBool("isFalling", false);

    }

    private void HangingExit()
    {
        playerAnim.SetBool("isHanging", false);
    }

    private void ClimbingExit()
    {
        playerAnim.SetBool("isClimbing", false);
    }

    #endregion
}
