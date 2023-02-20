using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementByForces : MonoBehaviour
{
  [SerializeField] Rigidbody2D myRigidbody;

  [Header("Horizontal Movement")]
  [SerializeField] float moveSpeed = 9f;

  private Vector2 moveInput;

  private void Start()
  {
    myRigidbody = GetComponent<Rigidbody2D>();
  }

  private void OnMove(InputValue value)
  {
    Debug.Log(value);
    moveInput = value.Get<Vector2>();
  }

  private void FixedUpdate()
  {
    Run();
  }

  //REVIEW THIS

  // [Header("Run")]
	// public float runMaxSpeed; //Target speed we want the player to reach.
	// public float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
	// [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	// public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	// [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	// [Space(5)]
	// [Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	// [Range(0f, 1)] public float deccelInAir;
	// [Space(5)]
	// public bool doConserveMomentum = true;

  private void Run()
  {
    float targetSpeed = moveInput.x * moveSpeed;
    float speedDif = targetSpeed - myRigidbody.velocity.x;
    // float accelRate = (Mathf.Abs(speedDif) > 0.01f) ? acceleration : decceleration;

  }
}
