using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwightController : MonoBehaviour
{
  private void OnEnable() {
    if(player != null)
    {
      player.SetClassPhysics(myBodyCollider, dwightAnimator);
    }
  }

  private PlayerController player;
  private PlayerAbilityTracker abilityTracker;

  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Animator dwightAnimator;
  
  [SerializeField] float dwightSkillOneActiveTime = 0f;
  [SerializeField] float dwightCooldownTimer = 25f;

  private void Start()
  {
    player = PlayerController.instance;
    abilityTracker = PlayerAbilityTracker.instance;
    player.SetClassPhysics(myBodyCollider, dwightAnimator);
  }

  public void UseAbilityOne()
  {
    Debug.Log("I am Dwight");
    abilityTracker.AbilityOneTrigger(dwightSkillOneActiveTime, dwightCooldownTimer, false);
  }

  public void UseBasicAttack()
  {
    StartCoroutine(Test());
  }

  IEnumerator Test() {
    player.myAnimator.SetBool("IsHoldingAttack", true);
    yield return new WaitForSeconds(1);
    player.myAnimator.SetBool("IsHoldingAttack", false);
    player.myAnimator.SetTrigger("ReleasedAttack");
  }
}
