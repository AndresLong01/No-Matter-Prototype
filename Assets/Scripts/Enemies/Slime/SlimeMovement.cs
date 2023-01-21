using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeMovement : MonoBehaviour
{
  private Rigidbody2D myRigidBody;
  [SerializeField] BoxCollider2D myEdgeCollider;

  [SerializeField] float moveSpeed = 1f;
  
  private bool isHit;
  [SerializeField] float reactionToHitTimer;

  void Start()
  {
    myRigidBody = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    if(isHit)
    {
      return;
    }
    
    myRigidBody.velocity = new Vector2(moveSpeed, myRigidBody.velocity.y);
  }

  void OnCollisionEnter2D(Collision2D other) {
    if(other.gameObject.tag == "Player")
    {
      isHit = true;
      StartCoroutine(StopMotion());
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    moveSpeed = -moveSpeed;
    FlipSprite();
  }

  void FlipSprite ()
  {
    transform.localScale = new Vector2(-(Mathf.Sign(myRigidBody.velocity.x)), 1f);
  }

  IEnumerator StopMotion()
  {
    yield return new WaitForSeconds(reactionToHitTimer);
    isHit = false;
  }
}
