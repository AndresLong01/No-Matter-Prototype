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

  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Animator dwightAnimator;

  private PlayerController player;

  private void Start()
  {
    player = PlayerController.instance;
    player.SetClassPhysics(myBodyCollider, dwightAnimator);
  }

  public void UseSkillOne()
  {
    Debug.Log("I am Dwight");
  }
}
