using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControllerController : MonoBehaviour, IStageController
{
    [SerializeField] private GameObject[] stageControllers;
    private int currentStageController;

    public IStage GetStage()
    {
        if (stageControllers[currentStageController].GetComponent<IStageController>().IsOver())
        {
            currentStageController++;
        }
        return stageControllers[currentStageController].GetComponent<IStageController>().GetStage();
    }

    public bool IsOver()
    {
        return false;
    }

    public void Reset()
    {
        currentStageController = 0;
        foreach (GameObject g in stageControllers)
        {
            g.GetComponent<IStageController>().Reset();
        }
    }

}
