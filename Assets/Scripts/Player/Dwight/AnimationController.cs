using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject Arrow;
    public GameObject ArrowSpawn;
    private Animator myAnimator;
    private LineRenderer lineRenderer;
    public float launchForce = 30f;
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
        holdingLMB = Input.GetMouseButton(0);

        UpdatePositions();

        if (myAnimator.GetBool("IsHoldingAttack") && holdingLMB)
        {
            lineRenderer.enabled = true;

            int points = 150;
            lineRenderer.positionCount = points;

            Vector2 arrowPosition = arrowSpawnPos;
            Vector2 arrowVelocity = mousePos.normalized * launchForce;
            
            for (var i = 0; i < points; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(arrowPosition.x, arrowPosition.y, 0));
                
                // add gravity vector * timedelta to velocity vector
                // or, add a frame of gravity influence to arrow velocity vector
                arrowVelocity += Physics2D.gravity * Time.fixedDeltaTime;

                // add velocity vector * timedelta to current position
                // or, add a frame of calculated velocity influence to arrow position vector
                arrowPosition += arrowVelocity * Time.fixedDeltaTime;

                if (arrowPosition.y < arrowSpawnPos.y)
                {
                    // jank ¯\_(ツ)_/¯
                    lineRenderer.positionCount = i;
                    break;
                }
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (myAnimator.GetBool("IsHoldingAttack") && !holdingLMB)
        {
            // we're in attack holding animation but aren't holding attack anymore, release the attack
            releasedMousePos = mousePos;
            myAnimator.SetTrigger("ReleasedAttack");
            myAnimator.SetBool("IsHoldingAttack", false);
        }
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
        mousePos = arrowSpawnPos + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrowSpawnPos) * 300f;
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
