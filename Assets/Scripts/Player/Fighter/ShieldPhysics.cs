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

    player.GetComponent<PlayerHealthController>().PlayerKnockback(-8f, 10f);

    PlayerAbilityTracker.instance.StartRecovery(recoveryPeriod);
  }
}
