using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] GameObject dustEffect;
    [SerializeField] GameObject bloodEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);

        if (other.gameObject.tag == "Enemy") {
            float relativeDirectionOfCollision = other.transform.position.x - transform.position.x;

            Instantiate(bloodEffect, transform.position - new Vector3(0f, 0f, 0f), Quaternion.identity);

            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(5);
            other.gameObject.GetComponent<EnemyHealthController>().EnemyKnockback(10f, 10f, relativeDirectionOfCollision);
        }
        else {
            Instantiate(dustEffect, transform.position - new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }
}
