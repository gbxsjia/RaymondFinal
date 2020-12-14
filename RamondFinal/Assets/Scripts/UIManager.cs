using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject BottomParent;
    public GameObject StatisticParent;
    public GameObject CalenderParent;
    public GameObject AccountParent;
    public GameObject PlanPageIntroParent;
    public GameObject PlanPageParent;

    public GameObject MiniMapPrefab;

    public UI_MiniMap selectedPlan;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public MainStateType mainState;

    public event System.Action MainStateChangeEvent;
    public void ChangeState(int typeIndex)
    {
        ChangeState((MainStateType)typeIndex);
    }

    public bool IsMainState(MainStateType type)
    {
        return mainState == type;
    }
    public void ChangeState(MainStateType type)
    {
        if (mainState == type)
        {
            return;
        }
        UIStateEnd(mainState);

        mainState = type;

        UIStateStart(type);

        if (MainStateChangeEvent != null)
        {
            MainStateChangeEvent();
        }
    }

    private void UIStateStart(MainStateType type)
    {
        switch (type)
        {
            case MainStateType.Home:
                if (UI_PlanHolder.instance.plans.Count > 0)
                {
                    if (!selectedPlan)
                    {
                        SelectPlan(UI_PlanHolder.instance.plans[0]);
                    }
                }
                break;
            case MainStateType.PlanProcess:
                WaypointManager.instance.ClearWaypoints();
                break;
            case MainStateType.RunProcess:
                WaypointManager.instance.ClearWaypoints();
                break;
        }
       
    }
    private void UIStateEnd(MainStateType type)
    {
        switch (type)
        {
            case MainStateType.Home:

                break;
      

        }
    }
    public void NewPlanSaved()
    {
        GameObject g = Instantiate(MiniMapPrefab);
        g.GetComponentInChildren<UI_MiniMap>().SaveMap();
        g.transform.position = new Vector3(540, 960);
        g.transform.SetParent(transform);

       
    }
    public void PlanDelete()
    {
        WaypointManager.instance.ClearWaypoints();
    }
    
    public void SelectPlan(UI_MiniMap plan)
    {
        if(plan == selectedPlan)
        {
            ChangeState(MainStateType.RunPlanDetail);
        }
        else
        {
            if (selectedPlan)
            {
                selectedPlan.SetSelectState(false);
            }
           
            selectedPlan = plan;
            plan.SetSelectState(true);
            WaypointManager.instance.UsePlanPositions(plan.Positions);
            CameraAvatar.instance.MoveAvatar(plan.Positions[0], 0.5f);
        }
    }
}
public enum MainStateType
{
    Home,
    PlanPage,
    PlanProcess,
    PlanDone,
    RunPage,
    RunProcess,
    GuideBook,
    Achievements,
    RunPlanDetail,
    RunPlanProcess
}