using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlanHolder : MonoBehaviour
{
    public static UI_PlanHolder instance;
    private void Awake()
    {
        instance = this;
    }

}
