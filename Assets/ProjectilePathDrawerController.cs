using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePathDrawerController : MonoBehaviour
{

    private Animator myAnimator;
    private LineRenderer lineRenderer;
    private Transform arrowSpawnPos;
    private Vector2 mousePos; // TODO: move this to parent context? Or leave it here? Currently duplicated on multiple scripts...
    private float launchForce;
    private int positionCount = 100;
    private Vector3[] currentRayPoints;
    private Vector3[] newPointsInLine;

    void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        myAnimator = gameObject.GetComponentInParent<DwightController>().myAnimator;
        arrowSpawnPos = transform.parent.Find("ArrowSpawn"); // why use transform? no gameObject?
        launchForce = gameObject.GetComponentInParent<DwightController>().launchForce;
        lineRenderer.positionCount = positionCount;
        currentRayPoints = new Vector3[positionCount];
    }

    void Update()
    {
        UpdatePositions();

        bool hitRegistered = false;

        if (myAnimator.GetBool("IsHoldingAttack") && Input.GetMouseButton(0))
        {
            lineRenderer.enabled = true;

            Vector2 arrowPosition = arrowSpawnPos.position;
            Vector2 arrowVelocity = mousePos.normalized * launchForce;

            for (int i=0; i < positionCount-1; i++)
            {
                lineRenderer.SetPosition(i, new Vector3(arrowPosition.x, arrowPosition.y, 0));
                // add gravity vector * timedelta to velocity vector
                // or, add a frame of gravity influence to arrow velocity vector
                arrowVelocity += Physics2D.gravity * Time.fixedDeltaTime;

                // add velocity vector * timedelta to current position
                // or, add a frame of calculated velocity influence to arrow position vector
                arrowPosition += arrowVelocity * Time.fixedDeltaTime;

                // store this
                currentRayPoints[i] = arrowPosition;

                // skip this if we don't have at least two points yet
                if (i >= 1)
                {
                    // null if we don't collide with anything between (start, end)
                    if (Physics2D.Linecast(currentRayPoints[i-1], currentRayPoints[i]))
                    {
                        newPointsInLine = new Vector3[i+1];

                        for (int j=0; j<newPointsInLine.Length; j++)
                        {
                            // transfer the points up to this point to the new points
                            newPointsInLine[j] = currentRayPoints[j];
                        }

                        hitRegistered = true;

                        break;
                    }
                }
            }

            if (hitRegistered)
            {
                lineRenderer.positionCount = newPointsInLine.Length;
                lineRenderer.SetPositions(newPointsInLine);
            }
            else
            {
                // something is going wrong here, I think. We're getting a point sometimes that 
                // lands us back at the origin point. Also we get index out of range errors. Not sure why >:(
                lineRenderer.positionCount = currentRayPoints.Length;
                lineRenderer.SetPositions(currentRayPoints);
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void UpdatePositions()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - arrowSpawnPos.position;
    }
}
