using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rigidbody playerBody;
    [SerializeField]
    private Animator playerAnim;
    [SerializeField]
    private Transform mainCam;
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
    private Vector3 followPosition;
    [SerializeField]
    private float hightOffset;
    [Header("Interaction Properties")]
    public GameObject interactionPrompt;
    public Transform promptPosition;
    [SerializeField]
    private bool interactionPosable;
    [SerializeField]
    private bool isGrabbingIntractable;
    private GameObject interactionTarget;
    

    void Start()
    {
        playerBody = this.gameObject.GetComponent<Rigidbody>();
        playerAnim = this.gameObject.GetComponent<Animator>();
        rightFacing = this.transform.rotation;
        leftFacing = new Quaternion(0,-rightFacing.y,0,1);
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
    }

    void Update()
    {
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
        interactionPrompt.transform.position = promptPosition.position;
    }

    void FixedUpdate()
    {
        SetCameraPosition();
        if(!isGrabbingLedge)
        {
            if(!isGrabbingIntractable)
            {
                playerBody.velocity = new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
            }
            if(isGrabbingIntractable)
            {
                playerBody.velocity = new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
            }
        }
    }

    /// <summary>
    /// Event called by input system when move input is detected.
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {
        Vector2 moveVector2 = movementValue.Get<Vector2>();
        moveDirection.y = moveVector2.y;
        //Movement logic
        if(!isGrabbingLedge)
        {
            moveDirection.x = moveVector2.x;
        }
        else if(isGrabbingLedge)
        {
            if(moveVector2.y > 0)
            {
                StartCoroutine(LedgeClimb());
            }
            if(moveVector2.y < 0)
            {
                playerAnim.SetBool("isHanging", false);
                isGrabbingLedge = false;
                ledge = null;
                topOfLedge = Vector3.zero;
                activeOffset = Vector3.zero;
            }
        }
    }

     void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.TryGetComponent<Climbable>(out Climbable other) && col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            ledge = other.gameObject;
            LedgeGrab(ledge);
        }
        else if(col.gameObject.TryGetComponent<Intractable>(out Intractable other1))
        {
            interactionPosable = true;
            interactionTarget = other1.gameObject;
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
            if(interactionTarget.gameObject.TryGetComponent<Intractable>(out Intractable other))
            {
                if(isGrabbingIntractable)
                {
                    other.transform.parent = null;
                    isGrabbingIntractable = false;
                }
                else if(!isGrabbingLedge)
                {
                    other.transform.parent = this.transform;
                    isGrabbingIntractable = true;
                }
            }
        }
    }
    /// <summary>
    /// Event called when input system detects jump input. 
    /// </summary>
    void OnJump()
    {
        if(isGrounded)
        {
            playerBody.AddForce(transform.up * jumpForce);
        }
    }


    void LedgeGrab(GameObject ledge)
    {
        topOfLedge = ledge.transform.position;
        topOfLedge.y = ledge.transform.position.y * 1.1f;
        topOfLedge.x = ledge.transform.position.x * 1.05f;
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
        isGrabbingLedge = true;
    }

    IEnumerator LedgeClimb()
    {
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
        isGrabbingLedge = false;
        ledge = null;
        topOfLedge = Vector3.zero;
        activeOffset = Vector3.zero;
        moveDirection = Vector2.zero;
    }

    void SetCameraPosition()
    {
        followPosition = new Vector3 (transform.position.x,transform.position.y + hightOffset, mainCam.transform.position.z);  
        if(mainCam.position != followPosition)
        {
            mainCam.position = followPosition;
        }
    }
}