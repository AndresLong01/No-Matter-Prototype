using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
  PlayerController player;
  [SerializeField] int damageAmount = 5;
  [SerializeField] float xKnockbackAmount = 12f;

  private void Start()
  {
    player = PlayerController.instance;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    Debug.Log(other.gameObject);
    Debug.Log(other.gameObject.tag);
    if (other.gameObject.tag == "Player")
    {
      DealDamage();
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    //BUG: On the player using Bash, damage still gets read
    //Considerations, making player invincible on movement skills, bash lasts .3s while other movement options last much less therefore it won't make too much a difference
    Debug.Log(other.gameObject);
    Debug.Log(other.gameObject.tag);
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