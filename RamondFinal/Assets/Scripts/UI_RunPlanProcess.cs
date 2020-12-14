using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RunPlanProcess : MonoBehaviour
{
    public GameObject FailText;
    public GameObject ArriveText;

    public Button StartButton;

    public float ArriveRange;

    private bool Started;
    private bool inState;
    private void Start()
    {
        UIManager.instance.MainStateChangeEvent += OnStateChange;
    }

    private void OnStateChange()
    {
        switch (UIManager.instance.mainState)
        {
            case MainStateType.RunPlanProcess:
                inState = true;
                break;
            default: 
                if (inState)
                {
                    inState = false;
                }
                break;
        }
    }

    private void Update()
    {
        if(Vector3.Distance( CameraAvatar.instance.transform.position, UIManager.instance.selectedPlan.Positions[0]) <= ArriveRange)
        {
            FailText.SetActive(false);
            ArriveText.SetActive(true); 
            StartButton.interactable = true;
        }
        else
        {
            FailText.SetActive(true);
            ArriveText.SetActive(false);
            StartButton.interactable = false;
        }
    }
}
