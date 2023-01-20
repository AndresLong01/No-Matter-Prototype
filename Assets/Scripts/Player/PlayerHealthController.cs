using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
  private PlayerController player;

  // [HideInInspector]
  public int currentHealth;
  public int maxHealth;

  [Header("Invincibility Timers")]
  [SerializeField] float invincibilityLength;
  [SerializeField] float flashLength;

  private float flashCounter;
  private float invincibilityCounter;

  [Header("Invincibility Renders")]
  [SerializeField] SpriteRenderer[] playerSprites;

  private void Start()
  {
    player = PlayerController.instance;
    currentHealth = maxHealth;
  }

  private void Update()
  {
    if(invincibilityCounter > 0) 
    {
      invincibilityCounter -= Time.deltaTime;

      flashCounter -= Time.deltaTime;
      if(flashCounter <=0)
      {
        foreach (SpriteRenderer sprite in playerSprites)
        {
          sprite.enabled = !sprite.enabled;
        }
        flashCounter = flashLength;
      }

      if(invincibilityCounter <= 0)
      {
        foreach (SpriteRenderer sprite in playerSprites)
        {
          sprite.enabled = true;
        }
        flashCounter = 0f;
      }
    }
  }

  public void DamagePlayer(int damageAmount)
  {
    if (player.isPlayerRecovering)
    {
      return;
    }

    currentHealth -= damageAmount;
    PlayerAbilityTracker.instance.StartRecovery(invincibilityLength);

    if (currentHealth <= 0)
    {
      currentHealth = 0;
      gameObject.SetActive(false);
    }
    else
    {
      invincibilityCounter = invincibilityLength;
    }

    UIController.instance.UpdateHealth(currentHealth, maxHealth);
  }

  public void PlayerKnockback(float xValue, float yValue)
  {
    float currentPlayerVelocityVector = Mathf.Sign(player.myRigidBody.velocity.x);
    player.myRigidBody.velocity = new Vector2(0f, 0f);
    player.myRigidBody.velocity += new Vector2(currentPlayerVelocityVector * xValue, yValue);
  }

  public void fillHealth()
  {
    currentHealth = maxHealth;
    UIController.instance.UpdateHealth(currentHealth, maxHealth);
  }
}
