using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
  private PlayerController player;

  [SerializeField] int damageAmount = 5;
  [SerializeField] float xKnockbackAmount = 12f;

  private void Start()
  {
    player = PlayerController.instance;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      DealDamage();
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "Player")
    {
      DealDamage();
    }
  }

  private void DealDamage()
  {
    player.GetComponent<PlayerHealthController>().DamagePlayer(damageAmount);
    player.GetComponent<PlayerHealthController>().PlayerKnockback(xKnockbackAmount, 10f);
  }
}
