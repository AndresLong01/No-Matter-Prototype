using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPhysics : MonoBehaviour
{
  private PlayerController player;
  private FighterController fighterInstance;

  [SerializeField] float recoveryPeriod = .5f;
  [SerializeField] int shieldBashDamage = 12;
  [SerializeField] float shieldBashKnockbackX, shieldBashKnockbackY;

  private void Start() {
    player = PlayerController.instance;
    fighterInstance = FindObjectOfType<FighterController>();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    fighterInstance.shieldObject.SetActive(false);
    PlayerAbilityTracker.instance.StopAbilityOneEarly();
    if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
    {
      float relativeDirectionOfCollision = Mathf.Sign(other.transform.position.x - transform.position.x);

      other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(shieldBashDamage);
      other.gameObject.GetComponent<EnemyHealthController>().EnemyKnockback(shieldBashKnockbackX, shieldBashKnockbackY, relativeDirectionOfCollision);
    }

    player.GetComponent<PlayerHealthController>().PlayerKnockback(8f, 10f, 0f);

    PlayerAbilityTracker.instance.StartRecovery(recoveryPeriod);
  }
}
