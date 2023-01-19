using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPhysics : MonoBehaviour
{
  private PlayerController player;

  [SerializeField] float recoveryPeriod = .5f;

  private void Start() {
    player = PlayerController.instance;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    PlayerAbilityTracker.instance.stopShieldBashEarly();

    float currentPlayerVelocityVector = Mathf.Sign(player.myRigidBody.velocity.x);
    player.myRigidBody.velocity = new Vector2(0f, 0f);
    player.myRigidBody.velocity += new Vector2(currentPlayerVelocityVector * -8f, 10f);

    PlayerAbilityTracker.instance.StartRecovery(.5f);
  }
}
