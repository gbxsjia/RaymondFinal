using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniMap : MonoBehaviour
{
    float minX = 999999;
    float maxX = -999999;
    float minZ = 999999;
    float maxZ = -999999;

    public Image image;


    public void SaveMap()
    {
        foreach (Waypoint w in WaypointManager.instance.waypoints)
        {
            maxX = Mathf.Max(maxX, w.transform.position.x);
            minX = Mathf.Min(minX, w.transform.position.x);
            maxZ = Mathf.Max(maxZ, w.transform.position.z);
            minZ = Mathf.Min(minZ, w.transform.position.z);
        }

        ScreenShotHandler.instance.TakeScreenshot(500, 500);
    }

    private void Start()
    {
        ScreenShotHandler.instance.ScreenShotCapturedEvent += OnCaptured;
    }


    private void OnCaptured(Texture2D t)
    {
        image.sprite = Sprite.Create(t, new Rect(0, 0, 500, 500), new Vector2(0.5f, 0.5f), 100);

    }
}
