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
  [SerializeField] GameObject shieldObject;
  [SerializeField] Animator fighterAnimator;
  [SerializeField] SpriteRenderer mySpriteRenderer, bashEffect;
  [SerializeField] Color bashEffectColor;

  [SerializeField] float shieldBashDistance = 15f;
  [SerializeField] float bashEffectLifetime, timeBetweenEffects;

  private PlayerController player;
  private PlayerAbilityTracker abilityTracker;
  private float bashEffectTimer;

  private void Start()
  {
    //accessing the player controller instance
    player = PlayerController.instance;
    abilityTracker = FindObjectOfType<PlayerAbilityTracker>();
    player.SetClassPhysics(myBodyCollider, fighterAnimator);
    abilityTracker.canBash = true;
  }

  private void Update()
  {
    if (player.isUsingMovementSkill && !player.isPlayerRecovering)
    {
      bashEffectTimer -= Time.deltaTime;
      if (bashEffectTimer <= 0)
      {
        ShowBashEffects();
      }
    }
  }

  public void UseSkillOne()
  {
    if (!abilityTracker.canBash)
    {
      return;
    }

    shieldObject.SetActive(true);
    player.isUsingMovementSkill = true;

    //TODO: Cooldown Issue
    //Possible Solutions to cooldown problem: 
    //Run Coroutine on seperate Ability Tracker Monobehaviour, possibly does not update the variable of a disabled object
    //Run the timer on Update, game redesign in order because Update method paused when object is inactive
    //Run cooldown on Player Object and reset skill actives on swap

    //REMINDER: Create a new empty on the scene under Player as a parent
    //Create a Cooldown Manager script
    //Create cooldown IEnumerators here with seperate cooldown timer checks and check on class scripts if able to use skill
    abilityTracker.useShieldBash();

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
}
