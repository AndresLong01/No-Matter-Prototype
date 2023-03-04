using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePathDrawerController : MonoBehaviour
{
    private Vector3[] currentRayPoints;
    private Vector3[] newPointsInLine;
    private LineRenderer lineRenderer;
    private Transform arrowSpawnPos;
    private string projectileType;
    private Animator myAnimator;
    private LayerMask layerMask;
    private bool hitRegistered; // w/ bouncy projectiles, is this a bool[]?
    private int positionCount;
    private float launchForce;
    private Vector2 mousePos; // TODO: move this to parent context? Or leave it here? Currently duplicated on multiple scripts...

    void Start()
    {
        myAnimator = gameObject.GetComponentInParent<DwightController>().myAnimator;

        launchForce = gameObject.GetComponentInParent<DwightController>().launchForce;
        arrowSpawnPos = transform.parent.Find("ArrowSpawn").Find("SpawnPoint"); // why use transform? no gameObject?

        positionCount = 150;
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.positionCount = positionCount;
        currentRayPoints = new Vector3[positionCount];

        // tilde operator inverts the mask - or, collide with all layers except ones specified here
        layerMask = ~LayerMask.GetMask("No Collisions", "Player", "Projectiles");

        projectileType = "simple";
        hitRegistered = false;
    }

    void Update()
    {
        UpdatePositions();

        if (myAnimator.GetBool("IsHoldingAttack") && Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;

            if (projectileType == "simple")
            {
                DrawSimpleProjectilePath();
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void DrawSimpleProjectilePath()
    {
        // initial launch vector + position vector
        Vector2 arrowPosition = arrowSpawnPos.position;
        Vector2 arrowVelocity = mousePos.normalized * launchForce;

        // run through the max amount every time
        // TODO: See how we could increase the distance between the calculated points
        // which would allow us to reduce the # of points. Might be necessary if this is
        // computationally expensive?
        for (int i = 0; i < positionCount; i++)
        {
            if (i == 0)
            {
                currentRayPoints[i] = arrowPosition;
                continue;
            }

            // add gravity vector * timedelta to velocity vector
            // or, add a frame of gravity influence to arrow velocity vector
            arrowVelocity += Physics2D.gravity * Time.fixedDeltaTime;

            // add velocity vector * timedelta to current position
            // or, add a frame of calculated velocity influence to arrow position vector
            arrowPosition += arrowVelocity * Time.fixedDeltaTime;

            currentRayPoints[i] = arrowPosition;

            if (i >= 1)
            {
                RaycastHit2D hit = Physics2D.Linecast(currentRayPoints[i - 1], currentRayPoints[i], layerMask);
                if (hit)
                {
                    // if i === 50, then point 50 -> 51 had the collision (0-indexed)
                    // so make our new points 51 units in length
                    newPointsInLine = new Vector3[i + 1];

                    // only grab the i-1 points, because the i-th point had the collision,
                    // and is probably inside the collider
                    for (int j = 0; j < newPointsInLine.Length - 1; j++)
                    {
                        newPointsInLine[j] = currentRayPoints[j];
                    }

                    // set the last point to the raycast collision point, which lets us
                    // draw the line perfectly up to the collider
                    newPointsInLine[i] = hit.point;
                    hitRegistered = true;
                    break;
                }
            }
        }

        if (hitRegistered)
        {
            // we registered a hit, set the line points to equal the valid,
            // non-colliding points we just calculated.
            lineRenderer.positionCount = newPointsInLine.Length;
            lineRenderer.SetPositions(newPointsInLine);
        }
        else
        {
            // something is going wrong here, I think. We're getting a point sometimes that 
            // lands us back at the origin point. Also we get index out of range errors. Not sure why >:(
            lineRenderer.positionCount = currentRayPoints.Length;
            lineRenderer.SetPositions(currentRayPoints);

            // was getting an issue where if we log 90/100 points one time, then log 40 points another,
            // there are 50 lingering points that draw the line all over the place. Not sure if this is jank or not,
            // but resetting the point array seems to work alright.
            currentRayPoints = new Vector3[positionCount];
        }
    }

    private void UpdatePositions()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrowSpawnPos.position;
    }
}
