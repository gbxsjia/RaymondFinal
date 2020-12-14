using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_RunPage : MonoBehaviour
{
    public TextMeshProUGUI TimeCountText;
    public TextMeshProUGUI RunMilesText;
    public TextMeshProUGUI TimerText;
    public GameObject CounterParent;

    public GameObject WaypointPrefab;

    public List<Vector3> Locations;
    private Vector3 CurrentLocation;

    private int Index=0;
    private float runMinute;
    private float runSecond;
    private float runDistance;

    public float MoveSpeed;


    public void StartRun()
    {
        StartCoroutine(RunProcess());
        UIManager.instance.MainStateChangeEvent += OnStateChange;
    }

    private void OnStateChange()
    {
        TimerText.text ="00:00";
        RunMilesText.text = 0.ToString();
        WaypointManager.instance.ClearWaypoints();

        UIManager.instance.MainStateChangeEvent -= OnStateChange;
    }


    private IEnumerator RunProcess()
    {
        if (UIManager.instance.selectedPlan != null)
        {
            Locations = new List<Vector3>();
            for (int i = 0; i < UIManager.instance.selectedPlan.Positions.Length; i++)
            {
                Locations.Add(UIManager.instance.selectedPlan.Positions[i]);
            }
        }

        CounterParent.SetActive(true);
        TimeCountText.text = "3";
        yield return new WaitForSeconds(1);
        TimeCountText.text = "2";
        yield return new WaitForSeconds(1);
        TimeCountText.text = "1";
        yield return new WaitForSeconds(1);
        TimeCountText.text = "GO!";
        yield return new WaitForSeconds(1);
        CounterParent.SetActive(false);

        Index = 0;
        runMinute = 0;
        runSecond = 0;
        runDistance = 0;
        CurrentLocation = Locations[0];
        Instantiate(WaypointPrefab, CurrentLocation, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        while (Index < Locations.Count)
        {
            CurrentLocation = Vector3.MoveTowards(CurrentLocation, Locations[Index], MoveSpeed * Time.deltaTime);
            WaypointManager.instance.UpdateLastWayPointPosition(CurrentLocation);
            CameraAvatar.instance.ForceCameraPosition(CurrentLocation);
            TimerText.text = Mathf.FloorToInt(runMinute) + ":" + Mathf.FloorToInt(runSecond);
            RunMilesText.text = runDistance.ToString("#0.0");

            runSecond += Time.deltaTime * 10;
            if (runSecond >= 60)
            {
                runSecond -= 60;
                runMinute++;
            }
            runDistance += Time.deltaTime * MoveSpeed * 0.02f;
            if (Vector3.Distance(CurrentLocation, Locations[Index])<=0.1f)
            {
                Instantiate(WaypointPrefab, CurrentLocation, Quaternion.identity);
                Index++;
            }       
            yield return null;
        }
       
    }
}
