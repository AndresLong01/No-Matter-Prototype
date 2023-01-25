using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
  [SerializeField] int totalHealth ;
  [SerializeField] GameObject deathEffect;
  private Rigidbody2D myRigidBody;

  private void Start() {
    myRigidBody = GetComponent<Rigidbody2D>();
  }

  public void DamageEnemy(int damageAmount)
  {
    totalHealth -= damageAmount;

    if(totalHealth <= 0)
    {
      if(deathEffect != null)
      {
        Instantiate(deathEffect, transform.position, transform.rotation);
      }

      Destroy(gameObject);
    }
  }

  public void EnemyKnockback(float xValue, float yValue, float direction)
  {
    myRigidBody.velocity = new Vector2(0f, 0f);
    myRigidBody.velocity += new Vector2(direction * xValue, yValue);
  }
}
