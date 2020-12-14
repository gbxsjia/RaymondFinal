using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager instance;

    public GameObject WayPointPrefab;
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
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, point.transform.position-Vector3.up);
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
    public void ClearWaypoints()
    {
        for (int i = waypoints.Count - 1; i >= 0; i--)
        {
            Destroy(waypoints[i].gameObject);
        }
        waypoints.Clear();
        lineRenderer.positionCount = 0;
    }
   public void UpdateLastWayPointPosition(Vector3 pos)
    {
        if (waypoints.Count > 0)
        {
            waypoints[waypoints.Count - 1].transform.position = pos;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
        }
    }

    public void UsePlanPositions(Vector3[] poses)
    {
        ClearWaypoints();
        for (int i = 0; i < poses.Length; i++)
        {
            Instantiate(WayPointPrefab, poses[i], Quaternion.identity);
        }
        
    }
}
