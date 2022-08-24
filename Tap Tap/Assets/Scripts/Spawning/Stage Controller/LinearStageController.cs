using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearStageController : MonoBehaviour, IStageController
{

    [SerializeField] private GameObject[] stages;
    [SerializeField] private bool isEndless;

    private int nextStage;

    public void Reset()
    {
        nextStage = 0;

        foreach(GameObject stage in stages){
            stage.GetComponent<IStage>().SetIsEndless(false);
        }
        stages[stages.Length-1].GetComponent<IStage>().SetIsEndless(isEndless);
    }

    public IStage GetStage()
    {
        nextStage++;
        return stages[nextStage - 1].GetComponent<IStage>();
        
    }

    public bool IsOver()
    {
        return nextStage >= stages.Length;
    }
}
