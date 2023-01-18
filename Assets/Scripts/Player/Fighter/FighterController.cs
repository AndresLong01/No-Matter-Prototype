using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterController : MonoBehaviour
{
  private void OnEnable() {
    if(player != null)
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
  [SerializeField] float bashTimer = 0.3f;
  [SerializeField] float bashCooldownTimer = 3f;
  [SerializeField] float bashEffectLifetime, timeBetweenEffects;

  private PlayerController player;
  private float initialGravity;
  private bool canBash;
  private float bashEffectTimer;

  private void Start()
  {
    //accessing the player controller instance
    player = PlayerController.instance;
    player.SetClassPhysics(myBodyCollider, fighterAnimator);
    initialGravity = player.myRigidBody.gravityScale;
    canBash = true;
  }

  private void Update() {
    if(player.isUsingMovementSkill)
    {
      bashEffectTimer -= Time.deltaTime;
      if(bashEffectTimer <= 0)
      {
        ShowBashEffects();
      }
    }
  }

  public void UseSkillOne()
  {
    if (!canBash)
    {
      return;
    }

    shieldObject.SetActive(true);
    player.isUsingMovementSkill = true;

    //TODO: Maybe figure a way to QUEUE coroutines rather than instantiating 2 seperate routines for 1 skill
    //Possible Solutions to cooldown problem: 
    //Run Coroutine on seperate Ability Tracker Monobehaviour, possibly does not update the variable of a disabled object
    //Run the timer on Update, game redesign in order because Update method paused when object is inactive
    //Run cooldown on Player Object and reset skill actives on swap

    StartCoroutine(ShieldBash(bashTimer));
    StartCoroutine(ShieldBashCooldown(bashCooldownTimer));

    //resetting momentum and bashing
    player.myRigidBody.velocity = new Vector2(0f, 0f);
    //preventing gravity falloff
    player.myRigidBody.gravityScale = 0;
    player.myRigidBody.velocity += new Vector2(shieldBashDistance * player.transform.localScale.x * 1f, 0f);
  }

  private void ShowBashEffects()
  {
    Debug.Log(player.transform.position);
    SpriteRenderer image = Instantiate(bashEffect, player.transform.position, player.transform.rotation);
    image.sprite = mySpriteRenderer.sprite;
    image.transform.localScale = transform.localScale;
    image.color = bashEffectColor;

    Destroy(image.gameObject, bashEffectLifetime);

    bashEffectTimer = timeBetweenEffects;
  }

  IEnumerator ShieldBash(float bashTimer)
  {

    yield return new WaitForSeconds(bashTimer);

    player.isUsingMovementSkill = false;
    canBash = false;
    shieldObject.SetActive(false);
    player.myRigidBody.gravityScale = initialGravity;
  }

  IEnumerator ShieldBashCooldown(float bashCooldownTimer)
  {
    yield return new WaitForSeconds(bashCooldownTimer);

    Debug.Log("Off cooldown");
    canBash = true;
  }
}
