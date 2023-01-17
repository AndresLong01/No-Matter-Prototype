using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  //Serialized Fields (public elements)
  [SerializeField] float moveSpeed = 5f;
  [SerializeField] float jumpSpeed = 20f;
  [SerializeField] float shieldBashDistance = 15f;
  [SerializeField] float bashTimer = 0.3f;
  [SerializeField] float bashCooldownTimer = 3f;

  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Collider2D myFeetCollider;
  [SerializeField] GameObject shieldObject;

  [SerializeField] Animator myAnimator;

  //private variables
  private Rigidbody2D myRigidBody;
  private Vector2 moveInput;
  private bool canDoubleJump;
  private bool canBash, isBashing;

  private void Start()
  {
    myRigidBody = GetComponent<Rigidbody2D>();
    canBash = true;
  }

  private void Update()
  {
    Run();
    FlipSprite();
  }

  private void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>();
  }

  private void OnJump(InputValue value)
  {
    //checking if the character is touching the ground 
    bool isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

    //jumping and enabling double jump
    if (value.isPressed && isGrounded)
    {
      canDoubleJump = true;
      Jump();
      return;
    }

    //using the doublejump and disabling it so people can't spam
    if (value.isPressed && canDoubleJump)
    {
      canDoubleJump = false;
      Jump();
    }
  }

  private void OnFire(InputValue value)
  {
    if(!canBash)
    {
      return;
    }

    shieldObject.SetActive(true);
    isBashing = true;
    
    if(value.isPressed)
    {
      StartCoroutine(ShieldBash(bashTimer));
      StartCoroutine(ShieldBashCooldown(bashCooldownTimer));

      myRigidBody.velocity += new Vector2(shieldBashDistance * transform.localScale.x * 1f, 0f);
    }
  }

  private void Run()
  {
    //Initializing a vector to move towards given the moveInput change set by the OnMove method
    Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
    //Using that new vector to change the velocity of the actual rigid body in scene
    if(!isBashing)
    {
      myRigidBody.velocity = playerVelocity;
    }

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

  private void FlipSprite()
  {
    bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

    if (playerHasHorizontalSpeed)
    {
      transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
    }
  }

  IEnumerator ShieldBash(float bashTimer)
  {
    yield return new WaitForSeconds(bashTimer);

    isBashing = false;
    canBash = false;
    shieldObject.SetActive(false);
  }

  IEnumerator ShieldBashCooldown(float bashCooldownTimer)
  {
    yield return new WaitForSeconds(bashCooldownTimer);

    canBash = true;
  }
}
