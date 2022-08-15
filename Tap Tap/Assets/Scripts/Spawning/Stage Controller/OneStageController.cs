using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OneStageController : MonoBehaviour, IStageController
{
    [SerializeField] private GameObject stage;
    private bool isOver;

    public void Reset()
    {
        isOver = false;
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
