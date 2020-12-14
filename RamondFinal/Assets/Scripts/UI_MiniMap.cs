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

    public RawImage image;

    public Vector3[] Positions;
    public void SaveMap()
    {
        foreach (Waypoint w in WaypointManager.instance.waypoints)
        {
            maxX = Mathf.Max(maxX, w.transform.position.x);
            minX = Mathf.Min(minX, w.transform.position.x);
            maxZ = Mathf.Max(maxZ, w.transform.position.z);
            minZ = Mathf.Min(minZ, w.transform.position.z);
        }
        float width = maxX - minX;
        float height = maxZ - minZ;
        if (width < 50)
        {
            width = 50;
        }
        else if (width > 200)
        {
            width = 20;
        }
        Vector3 targetposition = new Vector3((minX + maxX) / 2, width / 2 * Mathf.Tan(Mathf.Deg2Rad * 60)+50, (minZ + maxZ) / 2);
        StartCoroutine(SaveMapProcess(targetposition, 1));

        Positions = new Vector3[WaypointManager.instance.lineRenderer.positionCount];
        WaypointManager.instance.lineRenderer.GetPositions(Positions);     
    }

    private IEnumerator SaveMapProcess(Vector3 TargetPosition, float duration)
    {      
        CameraAvatar.instance.MoveCamera(TargetPosition, duration);
        yield return new WaitForSeconds(duration);
        ScreenShotHandler.instance.TakeScreenshot(540, 400);
        SelectPlan();
        UI_PlanHolder.instance.NewPlanSave(this);
        yield return new WaitForSeconds(0.5f);
        ScreenShotHandler.instance.ScreenShotCapturedEvent -= OnCaptured;
    }

    private void Start()
    {
        ScreenShotHandler.instance.ScreenShotCapturedEvent += OnCaptured;
    }


    private void OnCaptured(Texture2D t)
    {
        image.texture = t;
        image.enabled = true;
    }

    public void SetSelectState(bool select)
    {
        if (select)
        {
            image.color = new Color(1, 1, 1);
        }
        else
        {
            image.color = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    public void SelectPlan()
    {
        UIManager.instance.SelectPlan(this);
    }
}
