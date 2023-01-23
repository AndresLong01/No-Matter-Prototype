using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererController : MonoBehaviour
{
  private void OnEnable()
  {
    if (player != null)
    {
      player.SetClassPhysics(myBodyCollider, sorcererAnimator);
    }
  }

  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Animator sorcererAnimator;

  // [SerializeField] float attackRecoveryTime = 1f;

  private PlayerController player;
  private PlayerAbilityTracker abilityTracker;

  void Start()
  {
    player = PlayerController.instance;
    abilityTracker = PlayerAbilityTracker.instance;
    player.SetClassPhysics(myBodyCollider, sorcererAnimator);
  }

  void Update()
  {

  }
}
