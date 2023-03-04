using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 releasedMousePos;
    // for now, just the transform.position
    // or, dwight's world space position
    private Vector3 arrowSpawnPos;
    private Transform ArrowSpawn;
    public GameObject SpawnPoint;
    private Animator myAnimator;
    private float launchForce;
    public GameObject Arrow;
    // mouse position in world space
    private Vector2 mousePos;
    private bool holdingLMB;
    // This is the angle from a flat horizontal line
    // extending to the right from dwight rotated counter-clockwise
    // towards the mouse position
    private float angle;

    void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;

        holdingLMB = false;

        launchForce = transform.GetComponentInParent<DwightController>().launchForce;

        arrowSpawnPos = SpawnPoint.transform.position;

        ArrowSpawn = SpawnPoint.transform.parent;
    }

    void Update()
    {
        holdingLMB = Input.GetMouseButton(0);

        UpdatePositions();

        if (myAnimator.GetBool("IsHoldingAttack") && !holdingLMB)
        {
            // we're in attack holding animation but aren't holding attack anymore, release the attack
            releasedMousePos = mousePos;
            myAnimator.SetTrigger("ReleasedAttack");
            myAnimator.SetBool("IsHoldingAttack", false);
        }
    }

    private void UpdatePositions()
    {
        arrowSpawnPos = SpawnPoint.transform.position;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrowSpawnPos;
        angle = Mathf.Atan2(releasedMousePos.y - arrowSpawnPos.y, releasedMousePos.x - arrowSpawnPos.x) * Mathf.Rad2Deg;
    }

    public void EnterHoldingState()
    {
        myAnimator.SetBool("IsReadyToAttack", false);

        if (holdingLMB)
        {
            myAnimator.SetBool("IsHoldingAttack", true);
        }
        else
        {
            releasedMousePos = mousePos;
            myAnimator.SetTrigger("ReleasedAttack");
            myAnimator.SetBool("IsHoldingAttack", false);
        }
    }

    public void Yeet()
    {
        Quaternion instantiationAngle = Quaternion.Euler(new Vector3(0, 0, angle));
        Vector3 instantiationPosition = SpawnPoint.transform.position;
        GameObject ArrowIns = Instantiate(Arrow, instantiationPosition, instantiationAngle);
        ArrowIns.GetComponent<Rigidbody2D>().AddForce(releasedMousePos.normalized * launchForce, ForceMode2D.Impulse);
    }
}
