using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  public static PlayerController instance;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  //Serialized Fields (public elements)
  [Header("Player Mechanics")]
  [SerializeField] float moveSpeed = 5f;
  [SerializeField] float jumpSpeed = 20f;
  [HideInInspector]
  public bool isPlayerRecovering;

  [Header("Physics")]
  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Collider2D myFeetCollider;
  public Rigidbody2D myRigidBody;

  [Header("Animation And Effects")]
  public Animator myAnimator;
  [SerializeField] GameObject doubleJumpEffect;
  [SerializeField] GameObject dustEffect;

  //private variable
  public bool isUsingAbility;

  private Vector2 moveInput;
  private bool isGrounded;
  private bool canDoubleJump;

  private void Start()
  {
    myRigidBody = GetComponent<Rigidbody2D>();
    //initializing class
  }

  private void Update()
  {
    Run();
    GroundCheck();
    FlipSprite();
  }

  private void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>(); 
  }

  private void OnJump(InputValue value)
  {
    if(isUsingAbility || isPlayerRecovering)
    {
      return;
    }

    //jumping and enabling double jump
    if (value.isPressed && isGrounded)
    {
      canDoubleJump = true;
      //set animator to jump
      Jump();
      return;
    }

    //using the doublejump and disabling it so people can't spam
    if (value.isPressed && canDoubleJump)
    {
      Instantiate(doubleJumpEffect, transform.position - new Vector3(0, 1f, 0), Quaternion.identity);
      canDoubleJump = false;
      Jump();
    }
  }

  //TODO: Fix getting stuck on corners, consider: Making it a box collider instead of capsule collider
  private void Run()
  {
    if(isUsingAbility || isPlayerRecovering)
    {
      return;
    }

    //Initializing a vector to move towards given the moveInput change set by the OnMove method
    Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
    //Using that new vector to change the velocity of the actual rigid body in scene
    myRigidBody.velocity = playerVelocity;

    //checking if there is momentum active
    bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

    if (playerHasHorizontalSpeed)
    {
      myAnimator.SetBool("IsRunning", true);
    }
    else
    {
      myAnimator.SetBool("IsRunning", false);
    }
  }

  private void Jump()
  {
    //resetting vertical momentum
    myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 0f);

    //jumping with a certain amount of speed
    myRigidBody.velocity += new Vector2(0f, jumpSpeed);
  }

  private void GroundCheck()
  {
    //checking if the character is touching the ground 
    isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

    if (!isUsingAbility)
    {
      myAnimator.SetBool("IsJumping", !isGrounded);
    }
    else
    {
      myAnimator.SetBool("IsJumping", false);
    }
  }

  private void FlipSprite()
  {
    if(isUsingAbility || isPlayerRecovering || FindObjectOfType<PlayerAttackController>().isPlayerAttacking)
    // if(isUsingAbility || isPlayerRecovering)
    {
      return;
    }

    bool playerHasHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

    if (playerHasHorizontalSpeed)
    {
      transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
    }
  }

  public void SetClassPhysics(Collider2D newBodyCollider, Animator newAnimator)
  {
    myBodyCollider = newBodyCollider;
    myAnimator = newAnimator;
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if(isGrounded)
    {
      return;
    }

    if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
    {
      Instantiate(dustEffect, transform.position - new Vector3(0f, 1f, 0f), Quaternion.identity);
    }
  }
}
