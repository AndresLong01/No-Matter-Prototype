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
      float relativeDirectionOfCollision = other.transform.position.x - transform.position.x;
      DealDamage(relativeDirectionOfCollision);
    }
  }

  // consider trigger for projectiles
  // private void OnTriggerEnter2D(Collider2D other)
  // {
  //   if (other.gameObject.tag == "Player")
  //   {
  //     float relativeDirectionOfCollision = other.transform.position.x - transform.position.x;
  //     DealDamage(relativeDirectionOfCollision);
  //   }
  // }

  private void DealDamage(float relativeDirection)
  {
    if (FindObjectOfType<PlayerHealthController>().isPlayerInvulnerable)
    {
      return;
    }

    float direction = Mathf.Sign(relativeDirection);
    player.GetComponent<PlayerHealthController>().DamagePlayer(damageAmount);
    player.GetComponent<PlayerHealthController>().PlayerKnockback(xKnockbackAmount, 10f, direction);
  }
}
