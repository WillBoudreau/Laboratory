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
    public float moveSpeed;
    public float jumpForce;
    public bool isIdle;
    public bool isGrabbingLedge;
    public bool isGrounded;
    public bool isFacingLeft;
    public float turnTime;
    private Vector2 moveDirection;
    private Quaternion rightFacing;
    private Quaternion leftFacing;
    [Header("Ledge Grab Properties")]
    public Vector3 activeOffset;
    public Vector3 leftOffset;
    public Vector3 rightOffset;
    public GameObject ledge;
    public float climbDuration;
    public Vector3 topOfLedge;
    private Vector3 desiredPosition;
    [Header("Camera Control Properties")]
    public Vector3 followPosition;
    public Vector3 rightCameraOffset;
    public Vector3 leftCameraOffset;
    public Vector3 upCameraOffset;
    public Vector3 downCameraOffset;
    public float hightOffset;

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
        if(moveDirection.x > 0)
        {
            if(isFacingLeft)
            {
                isFacingLeft = false;
                transform.rotation = rightFacing;
            }
        }
        else if(moveDirection.x < 0)
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
    }

    void FixedUpdate()
    {
        SetCameraPosition();
        if(!isGrabbingLedge)
        {
            playerBody.velocity = new Vector3(moveDirection.x * moveSpeed * Time.deltaTime, playerBody.velocity.y,playerBody.velocity.z);
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
    }
    /// <summary>
    /// Event called when input system detects interaction input. 
    /// </summary>
    void OnInteract()
    {

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

     void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.TryGetComponent<Climbable>(out Climbable thing) && col.gameObject.transform.position.y > this.gameObject.transform.position.y)
        {
            ledge = thing.gameObject;
            LedgeGrab(ledge);
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
