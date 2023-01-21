using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPhysics : MonoBehaviour
{
  private PlayerController player;

  [SerializeField] float recoveryPeriod = .5f;
  [SerializeField] int shieldBashDamage = 12;
  [SerializeField] float shieldBashKnockbackX, shieldBashKnockbackY;

  private void Start() {
    player = PlayerController.instance;
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    PlayerAbilityTracker.instance.stopShieldBashEarly();
    if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
    {
      other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(shieldBashDamage);
      other.gameObject.GetComponent<EnemyHealthController>().EnemyKnockback(shieldBashKnockbackX, shieldBashKnockbackY);
    }

    player.GetComponent<PlayerHealthController>().PlayerKnockback(-8f, 10f);

    PlayerAbilityTracker.instance.StartRecovery(recoveryPeriod);
  }
}
