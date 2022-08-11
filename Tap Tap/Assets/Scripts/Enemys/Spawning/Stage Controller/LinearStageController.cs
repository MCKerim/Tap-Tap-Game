using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearStageController : MonoBehaviour, IStageController
{

    [SerializeField] private GameObject[] stages;
    private int nextStage;

    public void Reset()
    {
        nextStage = 0;
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
