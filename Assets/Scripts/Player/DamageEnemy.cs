using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
  [SerializeField] int damageAmount;
  [SerializeField] float xKnockbackAmount, yKnockbackAmount;

  private void OnTriggerEnter2D(Collider2D other)
  {
    if(other.gameObject.tag == "Enemy")
    {
      float relativeDirectionOfCollision = Mathf.Sign(other.transform.position.x - transform.position.x);

      other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
      other.gameObject.GetComponent<EnemyHealthController>().EnemyKnockback(xKnockbackAmount, yKnockbackAmount, relativeDirectionOfCollision);
    }
  }
}
