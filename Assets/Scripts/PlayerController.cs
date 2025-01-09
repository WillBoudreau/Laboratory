using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rigidbody playerBody;
    [Header("PlayerStats")]
    public float moveSpeed;
    public bool isGrabbingLedge;
    public bool isGrounded;
    private Vector2 moveDirection;

    void Start()
    {
        playerBody = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        playerBody.AddForce(new Vector3(moveDirection.x * moveSpeed, playerBody.velocity.y,playerBody.velocity.z));
    }

    /// <summary>
    /// Event called by input system when move input is detected.
    /// </summary>
    /// <param name="movementValue"></param>
    void OnMove(InputValue movementValue)
    {
        Vector2 moveVector2 = movementValue.Get<Vector2>();
        //Movement logic
        if(!isGrabbingLedge)
        {
            moveDirection.x = moveVector2.x;
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
        
    }
}
