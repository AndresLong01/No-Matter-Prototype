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
      currentComboTimer = 0;
    }
  }

  private PlayerController player;
  private PlayerAbilityTracker abilityTracker;

  [Header("Physics And Animations")]
  [SerializeField] Collider2D myBodyCollider;
  [SerializeField] Animator fighterAnimator;
  [SerializeField] SpriteRenderer mySpriteRenderer, bashEffect;
  [SerializeField] Color bashEffectColor;
  public GameObject shieldObject;

  [Header("Basic Attack")]
  [SerializeField] GameObject[] slashAnimations;
  [SerializeField] float comboTimer = 1f;
  [SerializeField] float attackRecoveryTime = 1f;
  private int comboStringNumber;
  private float currentComboTimer;

  [Header("Ability One")]
  [SerializeField] float bashActiveTimer = 0.3f;
  [SerializeField] float bashCooldownTimer = 3f;
  [SerializeField] float shieldBashDistance = 15f;
  [SerializeField] float bashEffectLifetime, timeBetweenEffects;
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
    if (currentComboTimer > 0)
    {
      currentComboTimer -= Time.deltaTime;
    }
    else
    {
      comboStringNumber = 0;
    }

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
    if (currentComboTimer <= 0)
    {
      player.myAnimator.SetTrigger("Attack");
      currentComboTimer = comboTimer;
      comboStringNumber = 1;
      FindObjectOfType<PlayerAttackController>().PlayerAttackRecovery(attackRecoveryTime);
      return;
    }

    if (currentComboTimer >= 0 && comboStringNumber == 1)
    {
      player.myAnimator.SetTrigger("ComboOne");
      currentComboTimer = comboTimer;
      comboStringNumber = 2;
      FindObjectOfType<PlayerAttackController>().PlayerAttackRecovery(attackRecoveryTime);
      return;
    }

    if (currentComboTimer >= 0 && comboStringNumber == 2)
    {
      player.myAnimator.SetTrigger("Finisher");
      currentComboTimer = 0.1f;
      comboStringNumber = 0;
      FindObjectOfType<PlayerAttackController>().PlayerAttackRecovery(attackRecoveryTime * 1.5f);
      return;
    }
  }

  public void UseAbilityOne()
  {
    if (player.isUsingAbility)
    {
      return;
    }

    StartCoroutine(ShowShield(bashActiveTimer));
    abilityTracker.AbilityOneTrigger(bashActiveTimer, bashCooldownTimer, true);

    //resetting momentum and bashing
    player.myRigidBody.velocity = new Vector2(0f, 0f);
    //preventing gravity falloff
    player.myRigidBody.gravityScale = 0;
    Debug.Log(player.myRigidBody);
    Debug.Log(shieldBashDistance);
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

  IEnumerator ShowShield(float shieldActiveTimer)
  {
    shieldObject.SetActive(true);

    yield return new WaitForSeconds(shieldActiveTimer);
    shieldObject.SetActive(false);
  }
}
