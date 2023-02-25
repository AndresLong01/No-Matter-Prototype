using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject Arrow;
    private Animator myAnimator;
    private LineRenderer lineRenderer;
    public float LaunchForce = 30f;
    private bool HoldingLMB = false;

    private Vector3 mousePos;
    private Vector3 dwightPos;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        HoldingLMB = Input.GetMouseButton(0);

        UpdatePositions();

        if (myAnimator.GetBool("IsHoldingAttack") && HoldingLMB)
        {
            lineRenderer.enabled = true;

            Vector3 lineTarget = Quaternion.AngleAxis(angle, Vector3.up) * new Vector3(mousePos.x, mousePos.y, 1f);

            lineRenderer.SetPosition(0, new Vector3(transform.position.x, transform.position.y, 0f));
            lineRenderer.SetPosition(1, lineTarget);
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (myAnimator.GetBool("IsHoldingAttack") && !HoldingLMB)
        {
            // we're in attack holding animation but aren't holding attack anymore, release the attack
            myAnimator.SetTrigger("ReleasedAttack");
            myAnimator.SetBool("IsHoldingAttack", false);
        }
    }

    private void UpdatePositions() {
        dwightPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos = Input.mousePosition;
        mousePos.x = mousePos.x - dwightPos.x;
        mousePos.y = mousePos.y - dwightPos.y;
        mousePos.z = 0f;
        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
    }

    public void EnterHoldingState()
    {
        myAnimator.SetBool("IsReadyToAttack", false);

        if (HoldingLMB)
        {
            myAnimator.SetBool("IsHoldingAttack", true);
        }
        else {
            myAnimator.SetTrigger("ReleasedAttack");
            myAnimator.SetBool("IsHoldingAttack", false);
        }
    }

    public void Yeet()
    {
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
