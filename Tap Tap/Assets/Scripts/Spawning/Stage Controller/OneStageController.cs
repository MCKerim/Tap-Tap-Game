using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OneStageController : MonoBehaviour, IStageController
{
    [SerializeField] private GameObject stage;
    [SerializeField] private bool isEndless;
    private bool isOver;

    public void Reset()
    {
        isOver = false;
        stage.GetComponent<IStage>().SetIsEndless(isEndless);
    }

    public IStage GetStage()
    {
        isOver = true;
        return stage.GetComponent<IStage>();
    }

    public bool IsOver()
    {
        return isOver;
    }
}
