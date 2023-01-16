using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  //Serialized Fields (public elements)
  [SerializeField] float moveSpeed = 5f;
  [SerializeField] Animator myAnimator;
  //private variables
  private Rigidbody2D myRigidBody;
  private Vector2 moveInput;

  private void Start()
  {
    myRigidBody = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    Run();
    FlipSprite();
  }

  private void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>();
  }

  private void Run()
  {
    //Initializing a vector to move towards given the moveInput change set by the OnMove method
    Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
    //Using that new vector to change the velocity of the actual rigid body in scene
    myRigidBody.velocity = playerVelocity;

    //checking if there is momentum active
    bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

    if(playerHasHorizontalSpeed) 
    {
      myAnimator.SetBool("IsRunning", true);
    }
    else
    {
      myAnimator.SetBool("IsRunning", false);
    }
  }

  private void FlipSprite()
  {
    bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

    if(playerHasHorizontalSpeed)
    {
      transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
    }
  }
}
