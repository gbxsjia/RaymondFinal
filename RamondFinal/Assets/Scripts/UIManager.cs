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
    Achievements
}