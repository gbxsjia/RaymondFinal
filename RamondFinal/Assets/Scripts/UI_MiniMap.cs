using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MiniMap : MonoBehaviour
{
    float minX = 999999;
    float maxX = -999999;
    float minZ = 999999;
    float maxZ = -999999;

    public void SaveMap()
    {
        foreach (Waypoint w in WaypointManager.instance.waypoints)
        {
            maxX = Mathf.Max(maxX, w.transform.position.x);
            minX = Mathf.Min(minX, w.transform.position.x);
            maxZ = Mathf.Max(maxZ, w.transform.position.z);
            minZ = Mathf.Min(minZ, w.transform.position.z);
        }
    }

}
