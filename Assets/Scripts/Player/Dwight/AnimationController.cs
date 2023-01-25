using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject Arrow;
    public float LaunchForce = 30f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Yeet() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0f;

        Vector3 dwightPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - dwightPos.x;
        mousePos.y = mousePos.y - dwightPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        Quaternion instantiationAngle = Quaternion.Euler(new Vector3(0, 0, angle));

        // So this works, but having an issue figuring out best way to modify the
        // instantiation position upwards or downwards (if jumping) based on the angle
        // between mouse and player
        //
        // e.g. if you're aimed upwards, you should fire the arrow at a higher position
        // than if you were aiming straight or downwards
        // something like this:
        Vector3 instantiationPosition = transform.position;
        // but include
        // + transform.up * (some modifier based on angle)
        // and 
        // + transform.right * (some modifier left/right)

        GameObject ArrowIns = Instantiate(Arrow, instantiationPosition, instantiationAngle);

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = (Vector3)(Input.mousePosition-screenPoint);
        direction.Normalize();
        ArrowIns.GetComponent<Rigidbody2D>().AddForce(direction*LaunchForce, ForceMode2D.Impulse);
    }
}
