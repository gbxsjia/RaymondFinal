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
        mainState = type;

        UIStateMaintain(type);

        if (MainStateChangeEvent != null)
        {
            MainStateChangeEvent();
        }
    }

    private void UIStateMaintain(MainStateType type)
    {
        switch (type)
        {
            case MainStateType.Home:
                BottomParent.SetActive(true);
                StatisticParent.SetActive(true);
                CalenderParent.SetActive(true);
                AccountParent.SetActive(true);
                PlanPageIntroParent.SetActive(false);
                PlanPageParent.SetActive(false);
                break;
            case MainStateType.PlanPage:
                BottomParent.SetActive(true);
                StatisticParent.SetActive(false);
                CalenderParent.SetActive(false);
                AccountParent.SetActive(false);
                PlanPageIntroParent.SetActive(true);
                PlanPageParent.SetActive(false);
                break;
            case MainStateType.PlanProcess:
                BottomParent.SetActive(false);
                StatisticParent.SetActive(false);
                CalenderParent.SetActive(false);
                AccountParent.SetActive(false);
                PlanPageIntroParent.SetActive(false);
                PlanPageParent.SetActive(true);
                break;
        }
    }
}
public enum MainStateType
{
    Home,
    PlanPage,
    PlanProcess,
    RunPage,
    RunProcess,
    GuideBook,
    Achievements
}