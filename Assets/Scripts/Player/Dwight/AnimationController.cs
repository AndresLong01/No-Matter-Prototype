using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject Arrow;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 DwightPosition = transform.position; // feels jank
    }

    public void Yeet() {
        Debug.Log("Yo!");
        GameObject ArrowIns = Instantiate(Arrow, transform.position, transform.rotation);
    }
}
