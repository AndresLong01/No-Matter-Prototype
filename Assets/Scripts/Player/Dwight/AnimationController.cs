using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject ArrowSpawn;
    public CircleCollider2D ArrowCollisionCheck;
    private Animator myAnimator;
    private LineRenderer lineRenderer;
    private bool holdingLMB = false;
    private Vector3 releasedMousePos;

    // mouse position in world space
    private Vector2 mousePos;

    // for now, just the transform.position
    // or, dwight's world space position
    private Vector3 arrowSpawnPos;

    // This is the angle from a flat horizontal line
    // extending to the right from dwight rotated counter-clockwise
    // towards the mouse position
    private float angle;

    private float launchForce;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = gameObject.GetComponent<Animator>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        launchForce = transform.GetComponentInParent<DwightController>().launchForce;
    }

    // Update is called once per frame
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

    public void ChildCollided(ArrowColliderController childCollider)
    {
        Debug.Log("Yo!");
    }

    private float[] GetProjectileXPoints(float[] yPoints)
    {
        float[] xPoints = new float[yPoints.Length];

        float xStart = arrowSpawnPos.x;
        float xEnd = mousePos.x;

        float interpRate = (xEnd - xStart) / yPoints.Length;

        for (int i = 0; i < yPoints.Length; i++)
        {
            xPoints[i] = i * interpRate;
        }

        return xPoints;
    }

    private void GetProjectilePoints()
    {

    }

    private void UpdatePositions()
    {
        arrowSpawnPos = ArrowSpawn.transform.position;
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
            GetProjectilePoints();
        }
    }

    public void Yeet()
    {
        Quaternion instantiationAngle = Quaternion.Euler(new Vector3(0, 0, angle));
        Vector3 instantiationPosition = ArrowSpawn.transform.position;
        GameObject ArrowIns = Instantiate(Arrow, instantiationPosition, instantiationAngle);
        ArrowIns.GetComponent<Rigidbody2D>().AddForce(releasedMousePos.normalized * launchForce, ForceMode2D.Impulse);
    }
}
