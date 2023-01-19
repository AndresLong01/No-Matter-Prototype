using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
  [SerializeField] Camera myCamera;
  [SerializeField] float parallaxEffect;

  // private float length;
  private float startPos;

  void Start()
  {
    startPos = transform.position.x;
    // length = GetComponent<SpriteRenderer>().bounds.size.x;
  }

  void Update()
  {
    // float lengthBoundaryMax = myCamera.transform.position.x * (1 - parallaxEffect);
    float distance = (myCamera.transform.position.x * parallaxEffect);
    
    Vector3 changeTarget = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    transform.position = Vector3.Lerp(transform.position, changeTarget, 8f * Time.deltaTime);

    // if(lengthBoundaryMax > startPos + length)
    // {
    //   startPos += length;
    //   transform.position = changeTarget;
    // }
    // else if (lengthBoundaryMax < startPos - length)
    // {
    //   startPos -= length;
    //   transform.position = changeTarget;
    // }
  }
}
