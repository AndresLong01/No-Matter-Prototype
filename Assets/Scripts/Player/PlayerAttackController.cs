using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : MonoBehaviour
{
  private PlayerController player;
  private PlayerAbilityTracker abilityTracker;

  private bool isPlayerAttacking;

  private void Start()
  {
    player = PlayerController.instance;
    abilityTracker = PlayerAbilityTracker.instance;
  }

  private void OnAttack(InputValue value)
  {
    if (value.isPressed)
    {
      if (abilityTracker.GetCurrentClass().name == "Fighter" && !isPlayerAttacking)
      {
        abilityTracker.GetCurrentClass().GetComponent<FighterController>().UseBasicAttack();
      }
    }
  }

  public void PlayerAttackRecovery(float recoveryTime)
  {
    StartCoroutine(AttackPeriod(recoveryTime));
  }

  IEnumerator AttackPeriod(float recoveryTime)
  {
    isPlayerAttacking = true;
    yield return new WaitForSeconds(recoveryTime);

    isPlayerAttacking = false;
  }
}