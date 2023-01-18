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
  [SerializeField] float moveSpeed = 5f;
  [SerializeField] float jumpSpeed = 20f;
  [SerializeField] float classSwapCDTimer= 5f;

  [SerializeField] GameObject[] availableClasses;
  [SerializeField] GameObject[] activeClasses;

  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Collider2D myFeetCollider;

  [SerializeField] Animator myAnimator;

  //private variables
  [HideInInspector]
  public bool isUsingMovementSkill;

  public Rigidbody2D myRigidBody;
  private Vector2 moveInput;
  private int selectedClassIndex;
  private bool canSwapClass;
  private bool isGrounded;
  private bool canDoubleJump;

  private void Start()
  {
    myRigidBody = GetComponent<Rigidbody2D>();
    //initializing class
    selectedClassIndex = 0;
    canSwapClass = true;
  }

  private void Update()
  {
    Run();
    GroundCheck();
  }

  private void OnMove(InputValue value)
  {
    moveInput = value.Get<Vector2>();
    FlipSprite();
  }

  private void OnJump(InputValue value)
  {
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
      canDoubleJump = false;
      Jump();
    }
  }

  private void OnSwitchClass(InputValue value)
  {
    if (activeClasses.Length == 0)
    {
      return;
    }

    if (value.isPressed && canSwapClass)
    {
      canSwapClass = false;
      activeClasses[selectedClassIndex].SetActive(!activeClasses[selectedClassIndex].activeSelf);
      
      if (selectedClassIndex < 1)
      {
        selectedClassIndex = 1;
      }
      else
      {
        selectedClassIndex = 0;
      }

      activeClasses[selectedClassIndex].SetActive(!activeClasses[selectedClassIndex].activeSelf);
      StartCoroutine(ClassSwapCooldown());
    }
  }

  //TODO: change this in the future in accordance to multiple skills used
  private void OnSkillUse(InputValue value)
  {
    //maybe object mapping, not sure yet
    if (value.isPressed)
    {
      if(activeClasses[selectedClassIndex].name == "Fighter")
      {
        activeClasses[selectedClassIndex].GetComponent<FighterController>().UseSkillOne();
      }
      else if (activeClasses[selectedClassIndex].name == "Dwight")
      {
        activeClasses[selectedClassIndex].GetComponent<DwightController>().UseSkillOne();
      }
    }
  }

  private void Run()
  {
    //Initializing a vector to move towards given the moveInput change set by the OnMove method
    Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidBody.velocity.y);
    //Using that new vector to change the velocity of the actual rigid body in scene
    if (!isUsingMovementSkill)
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

  private void GroundCheck()
  {
    //checking if the character is touching the ground 
    isGrounded = myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));

    if (!isUsingMovementSkill)
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

  IEnumerator ClassSwapCooldown()
  {
    yield return new WaitForSeconds(classSwapCDTimer);

    canSwapClass = true;
  }
}
