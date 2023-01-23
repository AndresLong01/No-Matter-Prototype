using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
  private void OnEnable()
  {
    if (player != null)
    {
      player.SetClassPhysics(myBodyCollider, fighterAnimator);
    }
  }

  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Animator fighterAnimator;
  [SerializeField] SpriteRenderer mySpriteRenderer, bashEffect;
  [SerializeField] Color bashEffectColor;
  public GameObject shieldObject;

  [SerializeField] float attackRecoveryTime = 1f;
  [SerializeField] float bashActiveTimer = 0.3f;
  [SerializeField] float bashCooldownTimer = 3f;
  [SerializeField] float shieldBashDistance = 15f;
  [SerializeField] float bashEffectLifetime, timeBetweenEffects;

  private PlayerController player;
  private PlayerAbilityTracker abilityTracker;
  private float bashEffectTimer;

  private void Start()
  {
    //accessing the player controller instance
    player = PlayerController.instance;
    abilityTracker = PlayerAbilityTracker.instance;
    player.SetClassPhysics(myBodyCollider, fighterAnimator);
  }

  private void Update()
  {
    if (player.isUsingAbility)
    {
      bashEffectTimer -= Time.deltaTime;
      if (bashEffectTimer <= 0)
      {
        ShowBashEffects();
      }
    }
  }

  public void UseBasicAttack()
  {
    player.myAnimator.SetTrigger("Attack");
    FindObjectOfType<PlayerAttackController>().PlayerAttackRecovery(attackRecoveryTime);
  }

  public void UseAbilityOne()
  {
    if(player.isUsingAbility)
    {
      return;
    }

    StartCoroutine(ShowShield(bashActiveTimer));
    abilityTracker.AbilityOneTrigger(bashActiveTimer, bashCooldownTimer, true);

    //resetting momentum and bashing
    player.myRigidBody.velocity = new Vector2(0f, 0f);
    //preventing gravity falloff
    player.myRigidBody.gravityScale = 0;
    player.myRigidBody.velocity += new Vector2(shieldBashDistance * player.transform.localScale.x * 1f, 0f);
  }

  private void ShowBashEffects()
  {
    SpriteRenderer image = Instantiate(bashEffect, player.transform.position, player.transform.rotation);
    image.sprite = mySpriteRenderer.sprite;
    image.transform.localScale = player.transform.localScale;
    image.color = bashEffectColor;

    Destroy(image.gameObject, bashEffectLifetime);

    bashEffectTimer = timeBetweenEffects;
  }

  IEnumerator ShowShield (float shieldActiveTimer)
  {
    shieldObject.SetActive(true);

    yield return new WaitForSeconds(shieldActiveTimer);
    shieldObject.SetActive(false);
  }
}
