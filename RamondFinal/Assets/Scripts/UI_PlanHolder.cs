using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlanHolder : MonoBehaviour
{
    public static UI_PlanHolder instance;

    public List<UI_MiniMap> plans;

    public Vector3 StartSize;
    public Vector3 EndSize;
    public float Duration;

    public float XOffset;
    public float YOffset = 400;
    public float Width;

    public bool adjust;
    private void Awake()
    {
        instance = this;        
    }

    private void Update()
    {
        if (adjust)
        {
            for (int i = 0; i < plans.Count; i++)
            {
                plans[i].transform.position = GetPlanTargetPosition(i);
            }
          
        }
    }
    public void NewPlanSave(UI_MiniMap minimap)
    {        
        plans.Add(minimap);
        minimap.transform.SetParent(transform);
        minimap.transform.position = new Vector3(540, 960, 0);
        StartCoroutine(NewPlanProcess(minimap));
    }

    public Vector3 GetPlanTargetPosition(int index)
    {
        return transform.position + Vector3.right * (XOffset + index * Width) + Vector3.up * YOffset;
    }
    
    private IEnumerator NewPlanProcess(UI_MiniMap minimap)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 startPos = minimap.transform.position;
        float timer = Duration;
        while (timer > 0)
        {
            minimap.transform.position = Vector3.Lerp(startPos, GetPlanTargetPosition(plans.Count - 1), 1 - timer / Duration);
            minimap.transform.localScale = Vector3.Lerp(StartSize, EndSize, 1 - timer / Duration);

            timer -= Time.deltaTime;
            yield return null;
        }
        minimap.transform.localScale = EndSize;
        minimap.transform.position = GetPlanTargetPosition(plans.Count - 1);
    }
}
