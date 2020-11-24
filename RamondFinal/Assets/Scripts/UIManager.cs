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
    public void ChangeState(MainStateType type)
    {
        if (mainState == type)
        {
            return;
        }
        mainState = type;
        if (MainStateChangeEvent != null)
        {
            MainStateChangeEvent();
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