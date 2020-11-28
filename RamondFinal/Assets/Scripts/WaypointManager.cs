using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;

    public List<Waypoint> waypoints;
    public LineRenderer lineRenderer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNewPoint(Waypoint point)
    {
        waypoints.Add(point);
        if (lineRenderer != null)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point.transform.position);
        }
    }
    public void DeleteLastWayPoint()
    {
        Waypoint w = waypoints[waypoints.Count - 1];       
        waypoints.Remove(w);
        Destroy(w.gameObject);
        if (lineRenderer != null)
        {
            lineRenderer.positionCount--;
        }
    }

   
}
