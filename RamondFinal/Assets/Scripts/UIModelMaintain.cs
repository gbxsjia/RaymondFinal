using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModelMaintain : MonoBehaviour
{
    public List<MainStateType> ActiveList;
    public List<MainStateType> CloseList;
    void Start()
    {
        UIManager.instance.MainStateChangeEvent += OnStateChange;
        OnStateChange();
        print(1);
    }

    private void OnStateChange()
    {
        if (ActiveList.Count > 0)
        {
            gameObject.SetActive(ActiveList.Contains(UIManager.instance.mainState));
        }
        else
        {
            gameObject.SetActive(!CloseList.Contains(UIManager.instance.mainState));
        }
    }
}
